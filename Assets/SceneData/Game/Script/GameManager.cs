using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Common.DataBase;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using TMPro;
using System;

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
  GameUserData gameUserData;
  MasterSkillDB masterSkillDB;

  float bonusRate = 0;
  int ContinueCounter = 0;
  long bet = 0;

  int enableChangeNum = 5;//変更できる数
  bool isForceChange = false;

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
  SkillData skillData;

	// Use this for initialization
	void Start ()
  {
    //ゲームで使うデータのロード
    gameUserData.LoadUserDB(DataBaseManager.Instance.GetDataBase<UserDB>());
    masterSkillDB = DataBaseManager.Instance.GetDataBase<MasterSkillDB>();

    SetViewBetCoin(gameUserData.BetCoin);
    SetViewHaveCoin(gameUserData.HaveCoin);

    //トランプ配布機能の作成
    distributeManager = new TrumpDistributeManager();
    //手札の役チェックの作成
    handChecker = new HandChecker();

    button.gameObject.SetActive(false);

    SetSkill();
    playerSkillView.SetActiveSkillView(false);
    //最初のフェーズへ
    ChangePhase(gamePhase);
    SceneChanger.Instance.IsInitialize = true;
	}

  void SetSkill()
  {
    var skill1 = masterSkillDB.GetData(gameUserData.UserSkillSlot.skillSlot1);

    skill1.MarkType = HandChecker.MarkType.Dia;
    skill1.Detail = SkillData.SkillDetail.AllChangeFlush ;

    if (skill1 == null)
    {
      playerSkillView.SetButtonText(0, "Empty");
      playerSkillView.SetButtonInteractable(0, false);
    }
    else
    {
      playerSkillView.SetButtonText(0, skill1.SkillName);

      Action action = () =>
      {
        gamePopupManager.OpenSkillDetailPopup(skill1.SkillName, skill1.Dist, () =>
          {
            UseSkill(skill1,1);
          }, null);
      };

      playerSkillView.SetButtonAction(0, action);
    }

    var skill2 = masterSkillDB.GetData(gameUserData.UserSkillSlot.skillSlot2);

    if (skill2 == null)
    {
      playerSkillView.SetButtonText(1, "Empty");
      playerSkillView.SetButtonInteractable(1, false);
    }
    else
    {
      playerSkillView.SetButtonText(1, skill2.SkillName);

      Action action = () =>
      {
        gamePopupManager.OpenSkillDetailPopup(skill2.SkillName, skill2.Dist, () =>
        {
          UseSkill(skill2,2);
        }, null);
      };

      playerSkillView.SetButtonAction(1, action);

    }

    var skill3 = masterSkillDB.GetData(gameUserData.UserSkillSlot.skillSlot3);

    if (skill3 == null)
    {
      playerSkillView.SetButtonText(2, "Empty");
      playerSkillView.SetButtonInteractable(2, false);
    }
    else
    {
      playerSkillView.SetButtonText(2, skill3.SkillName);

      Action action = () =>
      {
        gamePopupManager.OpenSkillDetailPopup(skill3.SkillName, skill3.Dist, () =>
        {
          UseSkill(skill3,3);
        }, null);
      };

      playerSkillView.SetButtonAction(2, action);

    }

  }

  public void UseSkill(SkillData _skillData,int _useSlot)
  {
    skillData = _skillData;

    //固定引き系の場合
    if(skillData.Detail == SkillData.SkillDetail.FixedNumber || skillData.Detail == SkillData.SkillDetail.FiexedMark)
    {
      enableChangeNum = 1; 
    }
    else if( skillData.Detail == SkillData.SkillDetail.AllChangeOnePair || skillData.Detail == SkillData.SkillDetail.AllChangeTwoPair || skillData.Detail == SkillData.SkillDetail.AllChangeFlush)
    {
      enableChangeNum = 5;

      handController.SetSelect(0, true);
      handController.SetSelect(1, true);
      handController.SetSelect(2, true);
      handController.SetSelect(3, true);
      handController.SetSelect(4, true);
      handController.SetAllLock(true);
    }
    else if(skillData.Detail == SkillData.SkillDetail.Raise)
    {

    }

    switch(_useSlot)
    {
      case 1:
        gameUserData.IsUseSkill1 = true;
        playerSkillView.SetButtonInteractable(0, false);
        break;

      case 2:
        gameUserData.IsUseSkill2 = true;
        playerSkillView.SetButtonInteractable(1, false);
        break;

      case 3:
        gameUserData.IsUseSkill3 = true;
        playerSkillView.SetButtonInteractable(2, false);
        break;
    }

    playerSkillView.SetActiveSkillView(false);
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
    handController.SetAllLock(false);
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

    //スキルビューを有効
    playerSkillView.SetActiveSkillView(true);

    //条件
    var dispose = this.UpdateAsObservable()
      .Subscribe(_ =>
      {
        if(handController.GetSelectTrumpIdxArray().Length > enableChangeNum)
        {
          button.interactable = false;
        }
        else
        {
          button.interactable = true;
        }
      });

    //チェンジボタン機能
    buttonText.text = "変更確定";
    button.OnClickAsObservable()
      .Take(1)
      .Subscribe(_ =>
      {
        gamePhase = GamePhase.SecondDistribute;
        ChangePhase(gamePhase);
        button.gameObject.SetActive(false);
        dispose.Dispose();
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

    if (skillData == null)
    {

      for (int i = 0; i < idxArray.Length; i++)
      {
        int idx = idxArray[i];
        handController.SetHandData(idx, distributeManager.DrawTrump());
        handController.Move(true, idx, cardMoveTime, null);
        Debug.Log(i);
        yield return new WaitForSeconds(cardMoveTime);
      }

    }
    else
    {
      var hand = distributeManager.DrawTrump(skillData, handController.GetHandData(), idxArray);
      handController.SetHandData(hand);
      for (int i = 0; i < idxArray.Length; i++)
      {
        int idx = idxArray[i];
        handController.Move(true, idx, cardMoveTime, null);
        Debug.Log(i);
        yield return new WaitForSeconds(cardMoveTime);
      }

    }

    while (handController.IsMove())
    {
      yield return null;
    }

    skillData = null;
    enableChangeNum = 5;

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
