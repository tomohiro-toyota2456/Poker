using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.DataBase;
using System.Linq;

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

  public ExchangeProductData GetData(string itemId)
  {
    return clone.ExchangeProductDataArray.FirstOrDefault(_ => _.ItemId == itemId);
  }
}
