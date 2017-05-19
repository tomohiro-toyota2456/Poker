using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Common.DataBase;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using TMPro;

public class GameManager : MonoBehaviour
{
  [SerializeField]
  float cardMoveTime = 0.5f;
  [SerializeField]
  HandController handController;
  [SerializeField]
  SkillView playerSkillView;
  [SerializeField]
  HandImageView handImageView;
  [SerializeField]
  Button button;
  [SerializeField]
  TextMeshProUGUI buttonText;
  [SerializeField]
  GamePopupManager gamePopupManager;
  [SerializeField]
  TextMeshProUGUI haveCoinText;
  [SerializeField]
  TextMeshProUGUI betCoinText;

  HandChecker handChecker;　
  TrumpDistributeManager distributeManager;//配る用スクリプト
  PopupManager popupManager;
  GameUserData gameUserData;
  MasterSkillDB masterSkillDB;

  float bonusRate = 0;
  int ContinueCounter = 0;
  long bet = 0;

  public enum GamePhase
  {
    Bet,//賭ける
    Distribute,//配布
    EnemySkill,//ディーラースキル発動かどうか
    Change,
    SecondDistribute,
    Result,
    Continue//継続チェック
  }

  GamePhase gamePhase = GamePhase.Bet;

	// Use this for initialization
	void Start ()
  {
    //ゲームで使うデータのロード
    gameUserData.LoadUserDB(DataBaseManager.Instance.GetDataBase<UserDB>());

    SetViewBetCoin(gameUserData.BetCoin);
    SetViewHaveCoin(gameUserData.HaveCoin);

    //トランプ配布機能の作成
    distributeManager = new TrumpDistributeManager();
    //手札の役チェックの作成
    handChecker = new HandChecker();

    popupManager = PopupManager.Instance;

    button.gameObject.SetActive(false);

    //最初のフェーズへ
    ChangePhase(gamePhase);
    SceneChanger.Instance.IsInitialize = true;
	}


  void SetViewHaveCoin(long _haveCoin)
  {
    haveCoinText.text = _haveCoin.ToString();
  }

  void SetViewBetCoin(long _betCoin)
  {
    betCoinText.text = _betCoin.ToString();
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

      case GamePhase.SecondDistribute:
        SecondDistributePhase();
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

    gamePopupManager.OpenBetPopup(gameUserData.HaveCoin, 100, (val) =>
    {
      bet = val;
      isBet = false;
    });


    while(isBet)
    {
      yield return null;
    }

    //この時点でベット確定し、セーブ
    //ベットコイン決定
    gameUserData.BetCoin = bet;
    //セーブ
    gameUserData.UseCoinAndSave();

    gamePhase = GamePhase.Distribute;
    distributeManager.InitTrumpList();
    ChangePhase(gamePhase);

  }

  void DistributePhase()
  {
    //表示更新
    SetViewBetCoin(gameUserData.BetCoin);
    SetViewHaveCoin(gameUserData.HaveCoin);

    StartCoroutine(Distribute());
  }

  IEnumerator Distribute()
  {
    for (int i = 0; i < 5; i++)
    {
      handController.SetPosition(i, new Vector2(0, 10000));
    }

      for (int i = 0; i < 5; i++)
    {
      var data = distributeManager.DrawTrump();
      handController.SetHandData(i,data );

      //Debug
      Debug.Log(i.ToString() + ":" + data.mark + "," + data.number);

      handController.Move(true, i, cardMoveTime, null);
      yield return new WaitForSeconds(cardMoveTime);
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
    button.gameObject.SetActive(true);

    //チェンジボタン機能
    buttonText.text = "変更確定";
    button.OnClickAsObservable()
      .Take(1)
      .Subscribe(_ =>
      {
        gamePhase = GamePhase.SecondDistribute;
        ChangePhase(gamePhase);
        button.gameObject.SetActive(false);
      }).AddTo(gameObject);

  }

  void SecondDistributePhase()
  {
    StartCoroutine(SecondDistribute());
  }

  IEnumerator SecondDistribute()
  {
    int[] idxArray = handController.GetSelectTrumpIdxArray();

    for (int i = 0; i < idxArray.Length; i++)
    {
      int idx = idxArray[i];
      handController.SetSelect(idx, false);
      handController.SetPosition(idx, new Vector2(0, 10000));
    }

    for (int i = 0; i < idxArray.Length; i++)
    {
      int idx = idxArray[i];
      handController.SetHandData(idx, distributeManager.DrawTrump());
      handController.Move(true, idx, cardMoveTime, null);
      Debug.Log(i);
      yield return new WaitForSeconds(cardMoveTime);
    }

    while (handController.IsMove())
    {
      yield return null;
    }

    gamePhase = GamePhase.Result;
    ChangePhase(gamePhase);

  }

  void ResultPhase()
  {
    StartCoroutine(Result());
  }

  IEnumerator Result()
  {
    var type = handChecker.CheckHand(handController.GetHandData());
    handImageView.SetActive(true);
    handImageView.SetSprite(type.GetHashCode());
    handImageView.InAnimationScl(0.5f, null);
    
    //役が成立していればボーナス値も適用する
    float bonusVal = type != HandChecker.HandType.NoPair ? bonusRate : 0;
    gameUserData.BetCoin = (long)(gameUserData.BetCoin * (GameCommon.GetHandScale(type) + bonusVal));

    Debug.Log(type);
    yield return new WaitForSeconds(2.5f);

    //確認ボタン機能
    button.gameObject.SetActive(true);
    buttonText.text = "次へ";
    button.OnClickAsObservable()
      .Take(1)
      .Subscribe(_ =>
      {
        handImageView.OutAnimationScl(0.3f, ()=>
         {
           gamePhase = GamePhase.Continue;
           ChangePhase(gamePhase);
           button.gameObject.SetActive(false);
         });
      }).AddTo(gameObject);



  }
  
  void ContinuePhase()
  {
    //所持金Max処理
    long haveCoin = gameUserData.HaveCoin;
    long haveMoney = -1000;

    if (haveCoin >= GameCommon.maxCoin)
    {
      haveCoin = GameCommon.maxCoin;

      //未クリアなので借金を返すように促す
      if (haveMoney < 0)
      {
        gamePopupManager.OpenHaveCoinMaxPopup(false, () => FinishGame());
      }
      else
      {
        gamePopupManager.OpenHaveCoinMaxPopup(true, () =>Continue());
      }
    }
    else if (haveCoin < 100 && gameUserData.BetCoin == 0)//コイン不足 かつ負けている
    {
      gamePopupManager.OpenNoMoneyContinuePopup(null, () => FinishGame());
    }
    else
    {
      Continue();
    }
  }

  void Continue()
  {
    var type = handChecker.CheckHand(handController.GetHandData());

    if(type != HandChecker.HandType.NoPair)
    {
      gamePopupManager.OpenContinuePopup(gameUserData.BetCoin,bonusRate, () =>
      {
        gamePhase = GamePhase.Distribute;
        ChangePhase(gamePhase);
      },
      () =>
      {
        FinishGame();
      });
    }
    else
    {
      gamePopupManager.OpenConfirmPopup(()=>
      {
        gamePhase = GamePhase.Bet;
        ChangePhase(gamePhase);
      },
      ()=>
      {
        FinishGame();
      });
    }
  }

  void FinishGame()
  {
    gameUserData.GetCoinAndSave();
    SceneChanger.Instance.ChangeScene("Home");
  }

}
