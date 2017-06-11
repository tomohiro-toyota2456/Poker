using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.DataBase;
using System.Linq;

public class MasterSkillShopDB : DataBase
{
  [SerializeField]
  MasterSkillShopData master;

  MasterSkillShopData clone;

  public override void Init()
  {
    clone = Instantiate(master);
  }

  public List<SkillProductData> GetProductList()
  {
    return clone.SkillProductDataArray.ToList();
  }
}
