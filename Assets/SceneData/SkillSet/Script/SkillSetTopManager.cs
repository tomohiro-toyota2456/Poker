using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Common.DataBase;
public class SkillSetTopManager : MonoBehaviour
{
  [SerializeField]
  SkillSlotObj[] skillSlotObj = new SkillSlotObj[4];//スキルスロット数

  MasterSkillDB masterSkillDB;
  // Use this for initialization
  void Start ()
  {
    var skillSlot = DataBaseManager.Instance.GetDataBase<UserDB>().GetSkillSlot();
    masterSkillDB = DataBaseManager.Instance.GetDataBase<MasterSkillDB>();
    InitSkillSlot(0, skillSlot.skillSlot1);
    InitSkillSlot(1, skillSlot.skillSlot2);
    InitSkillSlot(2, skillSlot.skillSlot3);
    InitSkillSlot(3, skillSlot.skillSlot4);
    SceneChanger.Instance.IsInitialize = true;
	}

  public void InitSkillSlot(int _idx,string _skillId)
  {
    if(string.IsNullOrEmpty(_skillId))
    {
      skillSlotObj[_idx].Init(_idx, "", SkillData.SkillType.ProbabilityUp);
      return;
    }

    var skillData = masterSkillDB.GetData(_skillId);
    skillSlotObj[_idx].Init(_idx, skillData.SkillName, skillData.Type);

  }
}
