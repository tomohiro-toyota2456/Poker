using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : ScriptableObject
{
  [SerializeField]
  string skillName;
  [SerializeField]
  string dist;
  [SerializeField]
  SkillType skillType;
  [SerializeField]
  SkillDetail skillDetail;
  [SerializeField]
  int number;
  [SerializeField]
  HandChecker.MarkType markType;

  public string SkillName { get { return skillName; } set { skillName = value; } }
  public string Dist { get { return dist; }set { dist = value; } }
  public SkillType Type { get { return skillType; } set { skillType = value; } }
  public SkillDetail Detail { get { return skillDetail; } set { skillDetail = value; } }
  public int Number { get { return number; }set { number = value; } }
  public HandChecker.MarkType MarkType { get { return markType; }set { markType = value; } } 

  public enum SkillType
  {
    Draw,
    Raise
  }

  public enum SkillDetail
  {
    FixedNumber,
    FiexedMark,
    AllChangeOnePair,
    AllChangeTwoPair,
    AllChangeFlush,
    Raise,
    ForceRaise,
  }
}
