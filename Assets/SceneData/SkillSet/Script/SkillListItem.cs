using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;
using TMPro;

public class SkillListItem : MonoBehaviour
{
  [SerializeField]
  TextMeshProUGUI skillNameText;
  [SerializeField]
  TextMeshProUGUI skillCoolTimeNumText;
  [SerializeField]
  TextMeshProUGUI skillSetText;
  [SerializeField]
  GameObject passiveObj;
  [SerializeField]
  Button skillSetButton;

  int setSlot = -1;
  public int SetSlot { get { return setSlot; } set { setSlot = value; }}
  //-1ならセットされてない
  public void Init(string _skillId,string _skillName,int _coolTime,SkillData.SkillType _type,Action<string,SkillListItem> _setAction,int _setSlot = -1)
  {
    setSlot = _setSlot;
    skillNameText.text = _skillName;
    skillCoolTimeNumText.text = _coolTime.ToString();

    if(_type == SkillData.SkillType.Passive)
    {
      passiveObj.SetActive(true);
    }

    skillSetButton.OnClickAsObservable()
      .Subscribe(_ =>
      {
          if (_setAction != null)
          _setAction(_skillId,this);
      }).AddTo(gameObject);

    SetSkillSetText(_setSlot);
  }

  public void SetSkillSetText(int _setSlot = -1)
  {
    if(_setSlot == - 1)
    {
      skillSetText.text = "";
      return;
    }

    skillSetText.text = "スロット" + (_setSlot+1).ToString() + "にセットされています";
  }

}
