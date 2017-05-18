using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UniRx;
using UniRx.Triggers;

//スキルの表示系とボタン動作制御クラス
public class SkillView : MonoBehaviour
{
  [SerializeField]
  Button[] skillSlotArray = new Button[3];
  [SerializeField]
  TextMeshProUGUI[] skillNameArray = new TextMeshProUGUI[3];//ボタンの表示テキスト
  [SerializeField]
  Image bg;

  public void SetButtonText(int _idx,string _text)
  {
    skillNameArray[_idx].text = _text;
  }

  public void SetButtonAction(int _idx,Action _action)
  {
    skillSlotArray[_idx].OnClickAsObservable()
      .Subscribe(_ =>
      {
        _action();
      }).AddTo(gameObject);
  }

  //スキルの表示制御
  public void SetActiveSkillView(bool _flag)
  {
    for(int i = 0; i < 3; i ++)
    {
      skillSlotArray[i].gameObject.SetActive(_flag);
      skillNameArray[i].gameObject.SetActive(_flag);
    }

    bg.gameObject.SetActive(_flag);
  }

  //ボタンのインタラクティブを制御
  public void SetButtonInteractable(int _idx,bool _flag)
  {
    skillSlotArray[_idx].interactable = _flag;
  }

}
