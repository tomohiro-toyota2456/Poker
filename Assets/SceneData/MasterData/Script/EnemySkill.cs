using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySkill : ScriptableObject
{
  [SerializeField]
  string skillId;
  [SerializeField]
  string skillName;
  [SerializeField]
  string dist;
  [SerializeField]
  float effect;
  [SerializeField]
  EnemySkillType sType;
  [SerializeField]
  OrderType oType;
  [SerializeField]
  ColorType cType;
  [SerializeField]
  HandChecker.HandType hType;
  [SerializeField]
  SkillData.SkillType targetSkillType;

  string SkillId{get{ return skillId; }set{ skillId = value; }}
  string SkillName{get{ return skillName; }set{ skillName = value; }}
  string Dist{get{ return dist; }set{ dist = value; }}
  float Effect{get{ return effect; }set{ effect = value; }}
  EnemySkillType Stype{get{ return sType; }set{ sType = value; }}
  OrderType OType{get{ return oType; }set{ oType = value; }}
  ColorType CType{get{ return cType; }set{ cType = value; }}
  HandChecker.HandType HType{get{ return hType; }set{ hType = value; }}
  SkillData.SkillType TargetSkillType{get{ return targetSkillType; }set{ targetSkillType = value; }}


  public enum EnemySkillType
  {
    SealSkill,
    Killer,
    ForceContinue,
    ForceAllChange,
    Order
  }


  public enum OrderType
  {
    Color,
    Hand,
    All
  }


  public enum ColorType
  {
    Black,
    Red
  }


}