﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;

public class BetPopup : PopupBase
{
  [SerializeField]
  NumberObject numberObject;
  [SerializeField]
  Button[] plusButtonArray;
  [SerializeField]
  Button[] minusButtonArray;
  [SerializeField]
  Button okButton;
  [SerializeField]
  Button cancelButton;

  long maxBet;
  long bet =0;
  long minBet;

  public void Init(long _maxBet,long _minBet,long _curBet,Action<long> _endOkAction,Action _endCancelAction)
  {
    maxBet = _maxBet;
    minBet = _minBet;

    bet = _curBet;

    if(_curBet >= maxBet)
    {
      bet = maxBet;
    }

    if(_curBet <= minBet)
    {
      bet = minBet;
    }

    numberObject.SetNumber(bet,true);

    long plus = 10;
    long minus= 10;
    for (int i = 0; i < plusButtonArray.Length; i++)
    {
      plus *= 10;
      long plusNum = plus;  

      plusButtonArray[i].OnClickAsObservable()
        .Subscribe(_ =>
        {
          bet += plusNum;

          if(bet > maxBet)
          {
            bet = maxBet;
          }

          numberObject.SetNumber(bet,true);

        }).AddTo(gameObject);
    }

    for(int i = 0; i < minusButtonArray.Length;i++)
    {
      minus *= 10;
      long minusNum = minus;

      minusButtonArray[i].OnClickAsObservable()
        .Subscribe(_ =>
        {
          bet -= minusNum;

          if (bet < minBet)
          {
            bet = minBet;
          }


          numberObject.SetNumber(bet,true);

        }).AddTo(gameObject);
    }


    okButton.OnClickAsObservable()
      .Take(1)
      .Subscribe(_ =>
      {
        if(_endOkAction != null)
        {
          _endOkAction(bet);
        }

        Close();
      }).AddTo(gameObject);

    cancelButton.OnClickAsObservable()
      .Take(1)
      .Subscribe(_ =>
      {
        if(_endCancelAction != null)
        {
          _endCancelAction();
        }
        Close();
      }).AddTo(gameObject);
  }

}
