using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.DataBase;
using System.Linq;

public class MasterShopDB : DataBase
{
  [SerializeField]
  MasterShopData masterShopData;

  MasterShopData clone;

  public override void Init()
  {
    base.Init();
    clone = Instantiate<MasterShopData>(masterShopData);
  }

  public ProductData[] GetDataArray()
  {
    return clone.ProductDataArray.ToArray();
  }

}
