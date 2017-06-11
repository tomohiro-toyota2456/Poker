using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShopListManager : MonoBehaviour
{
  [SerializeField]
  GameObject root;
  [SerializeField]
  SkillShopListItem item;

  public void AddItem(string _skillId,string _skillName,string _skillDist,int _skillCoolTime,SkillData.SkillType _skillType,int _value)
  {
    var listItem = Instantiate<SkillShopListItem>(item);
    listItem.transform.SetParent(root.transform);
    listItem.transform.localScale = Vector3.one;
    listItem.transform.localPosition = Vector3.zero;
    listItem.Init(_skillId, _skillName, _skillDist, _skillCoolTime, _skillType, _value);
  }

}
