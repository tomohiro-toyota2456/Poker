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
  Button[] skillSlotArray = new Button[4];
  [SerializeField]
  TextMeshProUGUI[] skillNameArray = new TextMeshProUGUI[4];//ボタンの表示テキスト
  [SerializeField]
  Image bg;
  [SerializeField]
  GameObject[] keepOutObjArray = new GameObject[4];

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
    for(int i = 0; i < 4; i ++)
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

  //ボタンを覆うカバーを付けるかどうか
  public void SetButtonKeepOut(int _idx,bool _flag)
  {
    keepOutObjArray[_idx].SetActive(_flag);
  }

}
