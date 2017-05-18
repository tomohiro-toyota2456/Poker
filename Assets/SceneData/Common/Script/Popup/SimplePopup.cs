namespace Common
{
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  using UnityEngine.UI;
  using UniRx;
  using TMPro;
  using System;

  //タイトル + 説明文 + closeボタンのシンプルな構成
  public class SimplePopup : PopupBase
  {
    [SerializeField]
    TextMeshProUGUI titleText;
    [SerializeField]
    Image titleImage;
    [SerializeField]
    TextMeshProUGUI dist;
    [SerializeField]
    Button closeButton;
    [SerializeField]
    Button yesButton;
    [SerializeField]
    Button noButton;

    public enum PopupType
    {
      Close,
      YesNo,
    }

    // Use this for initialization
    public override void Start()
    {
      base.Start();

      //閉じるボタン
      closeButton.OnClickAsObservable()
        .Take(1)
        .Subscribe(_ =>
        {
          Close();
        }).AddTo(gameObject);
    }

    //初期設定　
    public void Init(PopupType _type,string _title,string _dist,Action _yesAction = null,Action _noAction = null)
    {
      InitButton(_type, _yesAction, _noAction);

      SetTitle(_title);
      SetDist(_dist);
    }

    public void Init(PopupType _type, Sprite _title, string _dist, Action _yesAction = null, Action _noAction = null)
    {
      InitButton(_type, _yesAction, _noAction);

      SetTitle(_title);
      SetDist(_dist);
    }

    public void InitButton(PopupType _type, Action _yesAction = null, Action _noAction = null)
    {
      switch (_type)
      {
        case PopupType.Close:

          closeButton.gameObject.SetActive(true);
          yesButton.gameObject.SetActive(false);
          noButton.gameObject.SetActive(false);

          break;

        case PopupType.YesNo:
          closeButton.gameObject.SetActive(false);
          yesButton.gameObject.SetActive(true);
          noButton.gameObject.SetActive(true);
          SetActionYesButton(_yesAction);
          SetActionNoButton(_noAction);
          break;
      }
    }


    //説明文をいれる
    public void SetDist(string _dist)
    {
      dist.text = _dist;
    }

    //タイトルテキストをセット
    //この関数を呼ぶとImageのほうのタイトルは無効になる
    public void SetTitle(string _title)
    {
      titleText.gameObject.SetActive(true);
      titleImage.gameObject.SetActive(false);
      titleText.text = _title;
    }

    //タイトルスプライトをセット
    //この関数を呼ぶとTextのほうのタイトルは無効になる
    public void SetTitle(Sprite _sprite)
    {
      titleText.gameObject.SetActive(false);
      titleImage.gameObject.SetActive(true);
      titleImage.sprite = _sprite;
    }

    //はい選択時の処理
    public void SetActionYesButton(Action _action)
    {
      yesButton.OnClickAsObservable()
        .Take(1)
        .Subscribe(_ =>
        {
          if(_action != null)
          {
            AddCloseEndAction(_action);
          }

          Close();

        }).AddTo(gameObject);
    }

    //いいえ選択時の処理
    public void SetActionNoButton(Action _action)
    {
      noButton.OnClickAsObservable()
        .Take(1)
        .Subscribe(_ =>
        {
          if (_action != null)
          {
            AddCloseEndAction(_action);
          }

          Close();

        }).AddTo(gameObject);
    }
  }
}