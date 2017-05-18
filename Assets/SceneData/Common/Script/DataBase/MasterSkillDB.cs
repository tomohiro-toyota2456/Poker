using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.DataBase;
using System.Linq;

public class MasterSkillDB : DataBase
{
  [SerializeField]
  MasterSkillData masterSkillData;

  MasterSkillData cloneSkillData;
  public override void Init()
  {
    cloneSkillData = Instantiate<MasterSkillData>(masterSkillData);
  }

  public SkillData GetData(string _id)
  {
    return cloneSkillData.SkillDataArray.FirstOrDefault(data => data.SkillId == _id);
  }

}
