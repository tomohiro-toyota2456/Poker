using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class OptionPopup : PopupBase
{
  [SerializeField]
  Slider bgmSlider;
  [SerializeField]
  Slider seSlider;
  [SerializeField]
  Button closeButton;

  UserDB userDB;
  float seVol;
  float bgmVol;

  public void Init(UserDB _userDB)
  {
    userDB = _userDB;
    seVol = userDB.GetSeVol();
    bgmVol = userDB.GetBgmVol();

    seSlider.value = seVol;
    bgmSlider.value = bgmVol;

    bgmSlider.OnValueChangedAsObservable()
      .Subscribe(val =>
      {
        bgmVol = val;
      }).AddTo(gameObject);

    seSlider.OnPointerUpAsObservable()
      .Subscribe(_ =>
      {
        seVol = seSlider.value;
      }).AddTo(gameObject);

    closeButton.OnClickAsObservable()
      .Take(1)
      .Subscribe(_ =>
      {
        userDB.SetSeVol(seVol);
        userDB.SetBgmVol(bgmVol);
        userDB.SaveSeVol();
        userDB.SaveBgmVol();
        Close();
      }).AddTo(gameObject);
  }

}
