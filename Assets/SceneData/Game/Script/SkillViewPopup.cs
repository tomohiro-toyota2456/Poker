using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UnityEngine.UI;
using TMPro;
using System;
using UniRx;

public class SkillViewPopup : PopupBase
{
  [SerializeField]
  Button yesButton;
  [SerializeField]
  Button noButton;
  [SerializeField]
  TextMeshProUGUI distText;
  [SerializeField]
  TextMeshProUGUI coolTimeText;
  [SerializeField]
  TextMeshProUGUI titleText;

  public void Init(string _skillName,string _dist,int _coolTime,int _curCount,Action _yesAction,Action _noAction)
  {
    titleText.text = _skillName;
    distText.text = _dist;

    int count = _coolTime-_curCount;

    coolTimeText.text = count <= 0 ? "使用できます" : "残り" + count.ToString() + "ゲーム";
    SetYesButtonAction(_yesAction);
    SetNoButtonAction(_noAction);

    yesButton.interactable = count <= 0 ? true : false;
  }

  void SetYesButtonAction(Action _action)
  {
    yesButton.OnClickAsObservable()
      .Take(1)
      .Subscribe(_ =>
      {
        if (_action != null) ;
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

}
