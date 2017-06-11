using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Common.DataBase;
using System.Linq;

public class SkillShopManager : MonoBehaviour
{
  [SerializeField]
  SkillShopListManager listManager;

  HaveSkillDB haveSkillDB;
  MasterSkillShopDB masterSkillShopDB;
  MasterSkillDB masterSkillDB;
	// Use this for initialization
	void Start ()
  {
    haveSkillDB = DataBaseManager.Instance.GetDataBase<HaveSkillDB>();
    masterSkillShopDB = DataBaseManager.Instance.GetDataBase<MasterSkillShopDB>();
    masterSkillDB = DataBaseManager.Instance.GetDataBase<MasterSkillDB>();

    InitList();

    SceneChanger.Instance.IsInitialize = true;
		
	}

  public void InitList()
  {
    List<string> idList = haveSkillDB.GetDataList();

    var shopList = masterSkillShopDB.GetProductList().Where(data =>
    {
      for (int i = 0; i < idList.Count; i++)
      {
        if (data.SkillId == idList[i])
        {
          idList.RemoveAt(i);
          return false;
        }
      }

      return true;
    }).ToArray();

    for(int i = 0; i < shopList.Length; i++)
    {
      var product = shopList[i];
      var skillData = masterSkillDB.GetData(product.SkillId);
      listManager.AddItem(product.SkillId, skillData.SkillName, skillData.Dist, skillData.CoolTime,skillData.Type, product.Value);
    }

  }
}
