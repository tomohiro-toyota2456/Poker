using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillProductData : ScriptableObject
{
  [SerializeField]
  string productId;
  [SerializeField]
  string skillId;
  [SerializeField]
  int value;

  public string ProductId { get { return productId; } set { productId = value; } }
  public string SkillId { get { return skillId; }set { skillId = value; } }
  public int Value { get { return value; }set { this.value = value; } }

}
