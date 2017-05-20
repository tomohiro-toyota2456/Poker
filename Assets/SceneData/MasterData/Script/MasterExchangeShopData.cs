using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterExchangeShopData : ScriptableObject
{
  [SerializeField]
  ExchangeProductData[] exchangeProductDataArray;

  public ExchangeProductData[] ExchangeProductDataArray { get { return exchangeProductDataArray; }set { exchangeProductDataArray = value; } }
}
