using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System;

public class GamePopupManager : MonoBehaviour
{
  [SerializeField]
  SimplePopup simplePopup;
  [SerializeField]
  BetPopup betPopup;
  [SerializeField]
  ContinuePopup continuePopup;
  [SerializeField]
  SkillViewPopup skillViewPopup;

  PopupManager popupManager;

	// Use this for initialization
	void Start ()
  {
    popupManager = PopupManager.Instance;	
	}

  public void OpenBetPopup(long _maxBet,long _minBet,long _curBet,Action<long> _endOkAction,Action _endCancelAction)
  {
    var pp = popupManager.Create<BetPopup>(betPopup);
    pp.Init(_maxBet, _minBet,_curBet, _endOkAction,_endCancelAction);
    popupManager.OpenPopup(pp,null);
  }

  public void OpenContinuePopup(long _getCoin,float _bonus,Action _yesAction,Action _noAction)
  {
    var pp = popupManager.Create<ContinuePopup>(continuePopup);
    pp.Init(_getCoin, _bonus, _yesAction, _noAction);
    popupManager.OpenPopup(pp, null);
  }

  //確認
  public void OpenConfirmPopup(Action _yesAction, Action _noAction)
  {
    var pp = popupManager.Create<SimplePopup>(simplePopup);

    string nl = Environment.NewLine;
    string dist = "再びBetをしゲームを継続しますか";

    pp.Init(SimplePopup.PopupType.YesNo, "確認", dist, _yesAction, _noAction);
    popupManager.OpenPopup(pp, null);
  }

  //所持コインMax
  public void OpenHaveCoinMaxPopup(bool isClear,Action _closeAction)
  {
    var pp = popupManager.Create<SimplePopup>(simplePopup);
    string nl = Environment.NewLine;
    string dist = "";
    if (isClear)
    {
      dist = "コインが所持限界に達しました。" + nl + "コインは所持限界以上になりませんが、ゲームは継続できます。";
      pp.Init(SimplePopup.PopupType.Close, "警告",dist);
    }
    else
    {
      dist = "コインが所持限界に達しました。" + nl + "借金を返しましょう。";
      pp.Init(SimplePopup.PopupType.Close, "警告", dist);
    }

    pp.AddCloseEndAction(_closeAction);

    popupManager.OpenPopup(pp,null);
  }

  public void OpenNoMoneyContinuePopup(Action _yesAction, Action _noAction)
  {
    var pp = popupManager.Create<SimplePopup>(simplePopup);
    string nl = Environment.NewLine;
    string dist = "コインが足りません。"+nl+"広告を見てコインを取得しますか？";
    pp.Init(SimplePopup.PopupType.YesNo, "コイン不足", dist, _yesAction, _noAction);
    popupManager.OpenPopup(pp, null);
  }

  public void OpenSkillDetailPopup(SkillData _skillData,int _count,Action _yesAction,Action _noAction)
  {
    var pp = popupManager.Create<SkillViewPopup>(skillViewPopup);
    pp.Init(_skillData,_count, _yesAction, _noAction);
    popupManager.OpenPopup(pp, null);
  }
	
}
