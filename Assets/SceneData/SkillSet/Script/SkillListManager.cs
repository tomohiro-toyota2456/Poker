using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillListManager : MonoBehaviour
{
  [SerializeField]
  GameObject root;
  [SerializeField]
  SkillListItem item;

  public void AddItem(string _skillId, string _skillName, int _coolTime, SkillData.SkillType _type, Action<string, SkillListItem> _setAction, int _setSlot = -1)
  {
    var listItem = Instantiate<SkillListItem>(item);
    listItem.transform.SetParent(root.transform);
    listItem.transform.localScale = Vector3.one;
    listItem.transform.localPosition = Vector3.zero;

    listItem.Init(_skillId, _skillName, _coolTime, _type, _setAction, _setSlot);
  }

}
