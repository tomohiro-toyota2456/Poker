using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UniRx.Triggers;
using System;

public class ExchangeShopListItem : MonoBehaviour
{
  [SerializeField]
  Image iconImage;
  [SerializeField]
  TextMeshProUGUI nameText;
  [SerializeField]
  TextMeshProUGUI haveNumText;
  [SerializeField]
  Button sellButton;

  public string Id { get; set; }

  public void Init(string _id,string _name,int _haveNum,Action<ExchangeShopListItem> _sellAction)
  {
    Id = _id;
    nameText.text = _name;
    haveNumText.text = _haveNum.ToString();

    iconImage.sprite = ResourcesLoader.LoadItemSprite(_id);
    
    sellButton.OnClickAsObservable()
      .Subscribe(_ =>
      {
        if (_sellAction != null)
        {
          _sellAction(this);
        }
      }).AddTo(gameObject);
  }

}
