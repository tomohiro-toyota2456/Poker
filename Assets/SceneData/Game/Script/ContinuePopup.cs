using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using TMPro;
using System;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class ContinuePopup : PopupBase
{
  [SerializeField]
  TextMeshProUGUI bonusText;
  [SerializeField]
  NumberObject numberObject;
  [SerializeField]
  Button yesButton;
  [SerializeField]
  Button noButton;

  public void Init(long _getCoin,float _bonusVal,Action _yesAction,Action _noAction)
  {
    numberObject.SetNumber(_getCoin);
    SetYesButtonAction(_yesAction);
    SetNoButtonAction(_noAction);
    SetBonusText(_bonusVal);
  }

  void SetYesButtonAction(Action _action)
  {
    yesButton.OnClickAsObservable()
      .Take(1)
      .Subscribe(_ =>
      {
        if (_action != null) 
        _action();

        Close();
      }).AddTo(gameObject);
  }

  void SetNoButtonAction(Action _action)
  {
    noButton.OnClickAsObservable()
      .Take(1)
      .Subscribe(_ =>
      {
        if (_action != null) 
        _action();

        Close();
      }).AddTo(gameObject);
  }

  void SetBonusText(float _val)
  {
    bonusText.text = "継続ボーナス:次のゲームの役成立時の倍率を" + _val.ToString() + "加算します。";
  }

}
