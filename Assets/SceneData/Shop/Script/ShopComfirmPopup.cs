using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;
using TMPro;
using System;

public class ShopComfirmPopup : PopupBase
{
  [SerializeField]
  Button[] oneDigits = new Button[2];
  [SerializeField]
  Button[] tenDigits = new Button[2];
  [SerializeField]
  TextMeshProUGUI oneDigitNum;
  [SerializeField]
  TextMeshProUGUI tenDigitNum;
  [SerializeField]
  TextMeshProUGUI title;//購入　売却
  [SerializeField]
  TextMeshProUGUI itemName;
  [SerializeField]
  TextMeshProUGUI desc;
  [SerializeField]
  TextMeshProUGUI confirmText;//購入　売却
  [SerializeField]
  Button yesButton;
  [SerializeField]
  Button noButton;
  [SerializeField]
  Image icon;
  [SerializeField]
  TextMeshProUGUI haveNumText;//購入売却前
  [SerializeField]
  TextMeshProUGUI haveNumAfter;//購入売却後
  [SerializeField]
  TextMeshProUGUI haveNumTitleAfter;
  [SerializeField]
  TextMeshProUGUI haveMoneyTitle;
  [SerializeField]
  TextMeshProUGUI haveMoneyNumText;
  [SerializeField]
  TextMeshProUGUI haveMoneyTitleAfter;
  [SerializeField]
  TextMeshProUGUI haveMoneyNumAfter;

  int maxSelectNum = 0;
  int selectNum = 0;

  long haveMoney;
  int haveNum;
  int value;

  ShopType type;

  public enum ShopType
  {
    Buy,
    Sell
  }

	// Use this for initialization
	void Start ()
  {
    //増減ボタン設定
    oneDigits[0].OnClickAsObservable()
      .Subscribe(_ =>
      {
        selectNum++;
        if(selectNum > maxSelectNum)
        {
          selectNum = maxSelectNum;
        }

        UpdateHaveMoneyAfter();
        UpdateHaveNumAfter();
        UpdateDigit();

      }).AddTo(gameObject);

        oneDigits[1].OnClickAsObservable()
      .Subscribe(_ =>
      {
        selectNum--;
        if(selectNum < 0)
        {
          selectNum = 0;
        }
        UpdateHaveMoneyAfter();
        UpdateHaveNumAfter();
        UpdateDigit();

      }).AddTo(gameObject);

    tenDigits[0].OnClickAsObservable()
      .Subscribe(_ =>
      {
        selectNum+=10;
        if(selectNum > maxSelectNum)
        {
          selectNum = maxSelectNum;
        }

        UpdateHaveMoneyAfter();
        UpdateHaveNumAfter();
        UpdateDigit();

      }).AddTo(gameObject);

      tenDigits[1].OnClickAsObservable()
      .Subscribe(_ =>
      {
        selectNum-=10;
        if(selectNum < 0)
        {
          selectNum = 0;
        }

        UpdateHaveMoneyAfter();
        UpdateHaveNumAfter();
        UpdateDigit();

      }).AddTo(gameObject);

	}

  //所持金額　所持数 値段（売り値　買値) 成立時デリゲート（引数には購入・売却数)
  public void Init(string itemName,string itemDesc,Sprite itemIcon,ShopType _type,long _haveMoney,int _haveNum,int _value,Action<int> _yesAction)
  {
    type = _type;
    haveMoney = _haveMoney;
    haveNum = _haveNum;
    value = _value;

    selectNum = 0;
    UpdateDigit();
    UpdateHaveMoneyAfter();
    UpdateHaveNumAfter();

    this.itemName.text = itemName;
    this.desc.text = itemDesc;
    icon.sprite = itemIcon;

    haveMoneyNumText.text = haveMoney.ToString();
    haveNumText.text = haveNum.ToString();

    switch(_type)
    {
      case ShopType.Buy:
        SetUpBuyType();
        maxSelectNum = CalcMaxNumBuyType();
        break;

      case ShopType.Sell:
        SetUpSellType();
        maxSelectNum = CalcMaxNumSellType();
        break;
    }

    noButton.OnClickAsObservable()
      .Take(1)
      .Subscribe(_ =>
      {
        Close();
      }).AddTo(gameObject);

    yesButton.OnClickAsObservable()
      .Take(1)
      .Subscribe(_ =>
      {
        if(_yesAction!=null)
        {
          _yesAction(selectNum);
        }

        Close();
      }).AddTo(gameObject);

  }
	
  void SetUpBuyType()
  {
    title.text = "購入確認";
    confirmText.text = "購入しますか";
    haveNumTitleAfter.text = "購入後所持数";
    haveMoneyTitle.text = "所持コイン";
    haveMoneyTitleAfter.text = "購入後コイン";
  }

  void SetUpSellType()
  {
    title.text = "売却確認";
    confirmText.text = "売却しますか";
    haveNumTitleAfter.text = "売却後所持数";
    haveMoneyTitle.text = "所持金額";
    haveMoneyTitleAfter.text = "購入後金額";
  }

  void UpdateHaveMoneyAfter()
  {
    switch(type)
    {
      case ShopType.Buy:
        haveMoneyNumAfter.text = (haveMoney - value * selectNum).ToString();
        break;

      case ShopType.Sell:
        haveMoneyNumAfter.text = (haveMoney + value * selectNum).ToString();
        break;
    }
  }

  void UpdateHaveNumAfter()
  {
    switch (type)
    {
      case ShopType.Buy:
        haveNumAfter.text = (haveNum + selectNum).ToString();
        break;

      case ShopType.Sell:
        haveNumAfter.text = (haveNum - selectNum).ToString();
        break;
    }
  }

  void UpdateDigit()
  {
    int ten = selectNum / 10;
    int one = selectNum - ten;

    oneDigitNum.text = one.ToString();
    tenDigitNum.text = ten.ToString();
  }

  int CalcMaxNumBuyType()
  {
    int num = (int)(haveMoney / (long)value);

    if(num >= (GameCommon.maxItem-haveNum))
    {
      num = (GameCommon.maxItem-haveNum);
    }

    if(num < 0)
    {
      num = 0;
    }

    return num;
  }

  int CalcMaxNumSellType()
  {
    long money = haveMoney + haveNum * value;

    int num = haveNum;
    if(money > GameCommon.maxMoney)
    {
      while(money > GameCommon.maxMoney)
      {
        num--;
        money = haveMoney + num * value;

        if(num <= 0)
        {
          num = 0;
          break;
        }
      }
    }

    return num;
  }
}
