using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.DataBase;
using System.Linq;

public class MasterItemDB : DataBase
{
  [SerializeField]
  MasterItemData masterItemData;

  MasterItemData clone;

  public override void Init()
  {
    base.Init();
    clone = Instantiate<MasterItemData>(masterItemData);
  }

  public ItemData GetData(string _id)
  {
    return clone.ItemDataArray.FirstOrDefault(data => data.ItemId == _id);
  }

  public ItemData[] GetDataArray()
  {
    return clone.ItemDataArray;
  }

}
