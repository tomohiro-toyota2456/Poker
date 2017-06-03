using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.DataBase;
using System.Linq;
public class HaveItemDB : DataBase
{
  string key = "ojop@j03jqfj-[f";

  HaveItemList haveList;
  [System.Serializable]
  public struct HaveItemList
  {
    public List<HaveItemData> list;
  }

  [System.Serializable]
  public struct HaveItemData
  {
    public string itemId;
    public int num;
  }

  public void LoadData()
  {
    string json = PlayerPrefs.GetString(key);

    if(string.IsNullOrEmpty(json))
    {
      haveList.list = new List<HaveItemData>();
      json = JsonUtility.ToJson(haveList);
      PlayerPrefs.SetString(key, json);
    }

    haveList = JsonUtility.FromJson<HaveItemList>(json);
  }

  //無い場合はidが空欄のはず
  public HaveItemData GetData(string _itemId)
  {
    return haveList.list.FirstOrDefault(data => data.itemId == _itemId);
  }

  public HaveItemData[] GetDataArray()
  {
    return haveList.list.ToArray();
  }

  //データをセットするすでにある場合は所持数を更新
  //無い場合はリストに追加
  public void SetData(string _itemId,int _num)
  {
    HaveItemData itemData;
    itemData.itemId = _itemId;
    itemData.num = _num;
    if (haveList.list.Any(data=>data.itemId == _itemId))
    {
      for(int i = 0; i < haveList.list.Count; i++)
      {
        if(haveList.list[i].itemId == _itemId)
        {
          haveList.list[i] = itemData;
          return;
        }
      }
    }

    haveList.list.Add(itemData);
  }

  //所持数を加算する（引数マイナスで減算)
  //ある場合は加算・減算　ない場合はデータの追加処理を行う
  public void CalcItemNum(string _itemId,int _num)
  {
    HaveItemData itemData;
    itemData.itemId = _itemId;
    if (haveList.list.Any(data => data.itemId == _itemId))
    {
      for (int i = 0; i < haveList.list.Count; i++)
      {
        if (haveList.list[i].itemId == _itemId)
        {
          itemData.num = haveList.list[i].num + _num;
          haveList.list[i] = itemData;
          return;
        }
      }
    }

    itemData.num = _num;
    haveList.list.Add(itemData);
  }

  public void Save()
  {
    string json = JsonUtility.ToJson(haveList);
    PlayerPrefs.SetString(key, json);
  }



}
