using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSkillData : ScriptableObject
{
  [SerializeField]
  SkillData[] skillDataArray;
  [SerializeField]
  EnemySkillData[] enemySkillDataArray;

  public SkillData[] SkillDataArray { get { return skillDataArray; }set { skillDataArray = value; } }
  public EnemySkillData[] EnemySkillArray { get { return enemySkillDataArray; }set { enemySkillDataArray = value; } }
}
