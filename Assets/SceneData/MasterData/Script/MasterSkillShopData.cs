using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSkillShopData : ScriptableObject
{
  [SerializeField]
  SkillProductData[] productDataArray;

  public SkillProductData[] SkillProductDataArray { get { return productDataArray; } set { productDataArray = value; } }
}
