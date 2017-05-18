using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Common.DataBase;

public class GameManager : MonoBehaviour
{
  [SerializeField]
  HandController handController;
  [SerializeField]
  SkillView playerSkillView;
  [SerializeField]
  BetPopup betPopup;

  HandChecker handChecker;　
  TrumpDistributeManager distributeManager;//配る用スクリプト
  PopupManager popupManager;

  long bet = 0;

  public enum GamePhase
  {
    Bet,//賭ける
    Distribute,//配布
    EnemySkill,//ディーラースキル発動かどうか
    Change,
    Result,
    Continue//継続チェック
  }

  GamePhase gamePhase = GamePhase.Bet;

	// Use this for initialization
	void Start ()
  {
    distributeManager = new TrumpDistributeManager();
    handChecker = new HandChecker();
    popupManager = PopupManager.Instance;

    ChangePhase(gamePhase);
    SceneChanger.Instance.IsInitialize = true;
	}

  void ChangePhase(GamePhase _phase)
  {
    switch(_phase)
    {
      case GamePhase.Bet:
        BetPhase();
        break;

      case GamePhase.Distribute:
        DistributePhase();
        break;

      case GamePhase.EnemySkill:
        EnemySkillPhase();
        break;

      case GamePhase.Change:
        ChangePhase();
        break;

      case GamePhase.Result:
        ResultPhase();
        break;

      case GamePhase.Continue:
        ContinuePhase();
        break;
    }
  }

  void BetPhase()
  {
    StartCoroutine(Bet());
  }

  IEnumerator Bet()
  {
    bool isBet = true;
    var popup = popupManager.Create<BetPopup>(betPopup);
    popup.Init(100000000, 100, (val) =>
    {
      isBet = false;
      bet = val;
    });
    popupManager.OpenPopup(popup, null);

    while(isBet)
    {
      yield return null;
    }

    gamePhase = GamePhase.Distribute;
    distributeManager.InitTrumpList();
    ChangePhase(gamePhase);

  }

  void DistributePhase()
  {
    StartCoroutine(Distribute());
  }

  IEnumerator Distribute()
  {
    for (int i = 0; i < 5; i++)
    {
      handController.SetHandData(i, distributeManager.DrawTrump());
      handController.Move(true, i, 1.0f, null);
    }

    while(handController.IsMove())
    {
      yield return null;
    }

    gamePhase = GamePhase.EnemySkill;
    ChangePhase(gamePhase);

  }

  void EnemySkillPhase()
  {
    gamePhase = GamePhase.Change;
    ChangePhase(gamePhase);
  }

  void ChangePhase()
  {

  }

  void ResultPhase()
  {

  }
  
  void ContinuePhase()
  {

  }

}
