using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductData : ScriptableObject
{
  [SerializeField]
  string productId;
  [SerializeField]
  string itemId;
  [SerializeField]
  int value;

  public string ProductId { get { return productId; } set { productId = value; } }
  public string ItemId { get { return itemId; } set { itemId = value; } }
  public int Value { get { return value; }set { this.value = value; } }
}
