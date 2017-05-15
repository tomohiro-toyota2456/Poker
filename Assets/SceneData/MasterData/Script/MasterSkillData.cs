using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSkillData : ScriptableObject
{
  [SerializeField]
  SkillData[] skillDataArray;

  public SkillData[] SkillDataArray { get { return skillDataArray; }set { skillDataArray = value; } }
}
