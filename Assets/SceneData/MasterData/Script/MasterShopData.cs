using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterShopData : ScriptableObject
{
  [SerializeField]
  ProductData[] productDataArray;

  public ProductData[] ProductDataArray { get { return productDataArray; }set { productDataArray = value; } }
}
