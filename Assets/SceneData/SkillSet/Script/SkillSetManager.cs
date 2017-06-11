using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.DataBase;
using Common;

public class SkillSetManager : MonoBehaviour
{
  [SerializeField]
  SkillListManager manager;
  public static int slotId = -1;
  public static int SlotId { set { slotId = value; } }
  // Use this for initialization

  UserDB userDB;
  HaveSkillDB haveSkillDB;
  MasterSkillDB masterSkillDB;
	void Start ()
  {
    userDB = DataBaseManager.Instance.GetDataBase<UserDB>();
    haveSkillDB = DataBaseManager.Instance.GetDataBase<HaveSkillDB>();
    masterSkillDB = DataBaseManager.Instance.GetDataBase<MasterSkillDB>();

    var idArray = haveSkillDB.GetDataArray();

    for(int i = 0; i < idArray.Length; i++)
    {
      var skillData = masterSkillDB.GetData(idArray[i]);
      manager.AddItem(idArray[i], skillData.SkillName, skillData.CoolTime, skillData.Type, null, -1);
    }

    SceneChanger.Instance.IsInitialize = true;
  }
	
}
