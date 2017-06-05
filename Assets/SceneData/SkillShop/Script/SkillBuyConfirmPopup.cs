using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using TMPro;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;

public class SkillBuyConfirmPopup : PopupBase
{
  [SerializeField]
  TextMeshProUGUI skillNameText;
  [SerializeField]
  TextMeshProUGUI skillDistText;
  [SerializeField]
  TextMeshProUGUI valueText;
  [SerializeField]
  Button yesButton;
  [SerializeField]
  Button noButton;

  public void Init(string _skillName,string _skillDist,int _value,long _haveCoin,Action _yesAction)
  {
    skillNameText.text = _skillName;
    skillDistText.text = _skillDist;

    noButton.OnClickAsObservable()
      .Take(1)
      .Subscribe(_ =>
      {
        Close();
      }).AddTo(gameObject);

    if (_haveCoin - _value >= 0)
    {
      valueText.text = "コイン" + _value.ToString() + "で交換しますか";

      yesButton.OnClickAsObservable()
        .Take(1)
        .Subscribe(_ =>
        {
          if (_yesAction != null)
          {
            _yesAction();
          }
          Close();
        }).AddTo(gameObject);
    }
    else
    {
      valueText.text = "コインが" + (_value - _haveCoin).ToString() + "不足しています";
      yesButton.interactable = false;
    }
  }

}
