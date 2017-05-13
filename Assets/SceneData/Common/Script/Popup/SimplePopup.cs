namespace Common
{
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  using UnityEngine.UI;
  using UniRx;
  using TMPro;

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
  }
}