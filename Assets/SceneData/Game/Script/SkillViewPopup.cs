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
  Button closeButton;
  [SerializeField]
  TextMeshProUGUI distText;
  [SerializeField]
  TextMeshProUGUI coolTimeText;
  [SerializeField]
  TextMeshProUGUI titleText;

  public void Init(SkillData _skillData,int _curCount,Action _yesAction,Action _noAction)
  {
    titleText.text = _skillData.SkillName;
    distText.text = _skillData.Dist;

    int count = _skillData.CoolTime-_curCount;

    if (_skillData.Type == SkillData.SkillType.Passive)
    {
      coolTimeText.text = "常時効果発動中です。";
      SetCloseButton();
      closeButton.gameObject.SetActive(true);
      yesButton.gameObject.SetActive(false);
      noButton.gameObject.SetActive(false);
    }
    else
    {
      closeButton.gameObject.SetActive(false);
      yesButton.gameObject.SetActive(true);
      noButton.gameObject.SetActive(true);

      coolTimeText.text = count <= 0 ? "使用できます" : "残り" + count.ToString() + "ゲーム";
      SetYesButtonAction(_yesAction);
      SetNoButtonAction(_noAction);
      yesButton.interactable = count <= 0 ? true : false;
    }
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

  void SetCloseButton()
  {
    closeButton.OnClickAsObservable()
      .Take(1)
      .Subscribe(_ =>
      {
        Close();
      }).AddTo(gameObject);
  }

}
