using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeProductData : ScriptableObject
{
  [SerializeField]
  string productId;
  [SerializeField]
  string itemId;
  [SerializeField]
  int minVal;
  [SerializeField]
  int maxVal;

  public string ProductId { get { return productId; } set { productId = value; } }
  public string ItemId { get { return itemId; } set { itemId = value; } }
  public int MinVal { get { return minVal; }set { minVal = value; } }
  public int MaxVal { get { return maxVal; } set { maxVal = value; } }
}
