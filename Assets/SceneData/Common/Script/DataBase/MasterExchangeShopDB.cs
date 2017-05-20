using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.DataBase;

public class MasterExchangeShopDB : DataBase
{
  [SerializeField]
  MasterExchangeShopData masterExchangeShopData;

  MasterExchangeShopData clone;
  public override void Init()
  {
    base.Init();
    clone = Instantiate<MasterExchangeShopData>(masterExchangeShopData);
  }

  public ExchangeProductData[] GetDataArray()
  {
    return clone.ExchangeProductDataArray;
  }

}
