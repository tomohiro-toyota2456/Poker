using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySkillData : ScriptableObject
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

  public string SkillId{get{ return skillId; }set{ skillId = value; }}
  public string SkillName{get{ return skillName; }set{ skillName = value; }}
  public string Dist{get{ return dist; }set{ dist = value; }}
  public float Effect{get{ return effect; }set{ effect = value; }}
  public EnemySkillType SType{get{ return sType; }set{ sType = value; }}
  public OrderType OType{get{ return oType; }set{ oType = value; }}
  public ColorType CType{get{ return cType; }set{ cType = value; }}
  public HandChecker.HandType HType{get{ return hType; }set{ hType = value; }}
  public SkillData.SkillType TargetSkillType{get{ return targetSkillType; }set{ targetSkillType = value; }}


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