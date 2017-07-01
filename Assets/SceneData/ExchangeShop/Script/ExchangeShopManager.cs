using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Common.DataBase;
using Common;
using System;


public class ExchangeShopManager : MonoBehaviour
{
  [SerializeField]
  ExchangeShopListManager listManager;
  [SerializeField]
  TextMeshProUGUI[] sellTitleArray = new TextMeshProUGUI[8];
  [SerializeField]
  TextMeshProUGUI[] sellValArray = new TextMeshProUGUI[8];
  [SerializeField]
  ShopComfirmPopup shopComfirmPopup;


  MasterExchangeShopDB exchangeDB;
  MasterItemDB itemDB;
  HaveItemDB haveDB;
  UserDB userDB;

	// Use this for initialization
	void Start ()
  {
    userDB = DataBaseManager.Instance.GetDataBase<UserDB>();
    itemDB = DataBaseManager.Instance.GetDataBase<MasterItemDB>();
    exchangeDB = DataBaseManager.Instance.GetDataBase<MasterExchangeShopDB>();
    haveDB = DataBaseManager.Instance.GetDataBase<HaveItemDB>();

    var dataArray = exchangeDB.GetDataArray();
    for (int i = 0; i < dataArray.Length; i++)
    {
      string itemId = dataArray[i].ItemId;
      string itemName = itemDB.GetData(itemId).ItemName;
      int haveNum = haveDB.GetData(itemId).num;

      sellTitleArray[i].text = itemName;
      sellValArray[i].text = CalcSellValFromUserData(dataArray[i].MinVal, dataArray[i].MaxVal).ToString();
      listManager.AddList(itemId, itemName, haveNum,OpenCellConfirmPopup);
    }

    SceneChanger.Instance.IsInitialize = true;
	}

  public int CalcSellValFromUserData(int _min,int _max)
  {
    string date = userDB.GetLoginStore();

    //初入場ならとりあえずランダムでとってステータス保存
    if (string.IsNullOrEmpty(date))
    {
      userDB.SetLoginStore(DateTime.Now.ToString());
      userDB.SaveLoginStore();

      userDB.SetExchangeShopRandomState(UnityEngine.Random.state);
      userDB.SaveExchangeShopRandomState();
      return UnityEngine.Random.Range(_min, _max + 1);
    }

    DateTime prevLogin = DateTime.Parse(date);

    var diff = DateTime.Now - prevLogin;

    //一日以上差がある場合は新規にランダムを行っておく
    if(diff.Days >= 1)
    {
      userDB.SetLoginStore(DateTime.Now.ToString());
      userDB.SaveLoginStore();
      userDB.SetExchangeShopRandomState(UnityEngine.Random.state);
      userDB.SaveExchangeShopRandomState();
      return UnityEngine.Random.Range(_min, _max + 1);
    }

    //一日たっていない場合はユーザーデータのstateを利用し、同じ結果を出す
    UnityEngine.Random.state = userDB.GetExchangeShopRandomState();
    return UnityEngine.Random.Range(_min, _max + 1); 
  }

  //ランダムのステートを更新＋表示更新
  public void ChangeSellVal()
  {
    userDB.SetExchangeShopRandomState(UnityEngine.Random.state);
    userDB.SaveExchangeShopRandomState();

    var dataArray = exchangeDB.GetDataArray();
    for (int i = 0; i < dataArray.Length; i++)
    {
      sellValArray[i].text = CalcSellValFromUserData(dataArray[i].MinVal, dataArray[i].MaxVal).ToString();
    }
  }

  void OpenCellConfirmPopup(ExchangeShopListItem item)
  {
    var pp = PopupManager.Instance.Create<ShopComfirmPopup>(shopComfirmPopup);

    var data = itemDB.GetData(item.Id);
    var exData = exchangeDB.GetData(item.Id);
    string itemName = data.ItemName;
    string itemDesc = data.Dist;
    int value = CalcSellValFromUserData(exData.MinVal, exData.MaxVal); 
    long haveMoney = userDB.GetMoney();
    int haveNum = haveDB.GetData(item.Id).num;
    Sprite sprite = ResourcesLoader.LoadItemSprite(item.Id);

    pp.Init(itemName, itemDesc, sprite, ShopComfirmPopup.ShopType.Sell, haveMoney, haveNum, value, null);

    PopupManager.Instance.OpenPopup(pp, null);
  }
}
