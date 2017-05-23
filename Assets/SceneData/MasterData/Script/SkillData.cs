using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : ScriptableObject
{
  [SerializeField]
  string skillId;
  [SerializeField]
  string skillName;
  [SerializeField]
  string dist;
  [SerializeField]
  SkillType skillType;
  [SerializeField]
  float effect;
  [SerializeField]
  HandChecker.HandType handType;
  [SerializeField]
  int coolTime;

  public string SkillId { get { return skillId; } set { skillId = value; } }
  public string SkillName { get { return skillName; } set { skillName = value; } }
  public string Dist { get { return dist; }set { dist = value; } }
  public SkillType Type { get { return skillType; } set { skillType = value; } }
  public float Effect { get { return effect; }set { effect = value; } }
  public int CoolTime { get { return coolTime; }set { coolTime = value; } }
  public HandChecker.HandType Hand { get { return handType; }set { handType = value; } }

  public enum SkillType
  {
    Passive,//使わなくても発動するスキル Hand Effect
    ProbabilityUp,//確率上昇 Hand Effect CoolTime
    Bet,//ついか賭け (int)effect
    AllBet,//全追加賭け AllChange
    Magnification,//倍率up系 Effect CoolTime
  }
}
