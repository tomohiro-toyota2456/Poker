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
    cloneSkillData.hideFlags = HideFlags.DontSave;
  }

  public SkillData GetData(string _id)
  {
    return cloneSkillData.SkillDataArray.FirstOrDefault(data => data.SkillId == _id);
  }

  public EnemySkillData GetEnemyData(string _id)
  {
    return cloneSkillData.EnemySkillArray.FirstOrDefault(data => data.SkillId == _id);
  }

  public EnemySkillData[] GetEnemyDataArray()
  {
    EnemySkillData[] dataArray = new EnemySkillData[cloneSkillData.EnemySkillArray.Length];
    cloneSkillData.EnemySkillArray.CopyTo(dataArray, 0);
    return dataArray;
  }

}
