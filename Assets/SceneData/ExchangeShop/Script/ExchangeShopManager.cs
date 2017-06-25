using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Common.DataBase;
using Common;


public class ExchangeShopManager : MonoBehaviour
{
  [SerializeField]
  ExchangeShopListManager listManager;
  [SerializeField]
  TextMeshProUGUI[] sellTitleArray = new TextMeshProUGUI[8];
  [SerializeField]
  TextMeshProUGUI[] sellValArray = new TextMeshProUGUI[8];

	// Use this for initialization
	void Start ()
  {
    var itemDB = DataBaseManager.Instance.GetDataBase<MasterItemDB>();
    var exchangeDB = DataBaseManager.Instance.GetDataBase<MasterExchangeShopDB>();
    var haveDB = DataBaseManager.Instance.GetDataBase<HaveItemDB>();

    var dataArray = exchangeDB.GetDataArray();
    for (int i = 0; i < 8; i++)
    {
      string itemId = dataArray[i].ItemId;
      string itemName = itemDB.GetData(itemId).ItemName;
      int haveNum = haveDB.GetData(itemId).num;

      sellTitleArray[i].text = itemName;
      listManager.AddList(itemId, itemName, haveNum, null);
    }

    SceneChanger.Instance.IsInitialize = true;
	}
}
