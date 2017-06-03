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
  [SerializeField]
  ContinueMagMasterData magMasterData;
  [SerializeField]
  GameObject particleObj;
  [SerializeField]
  SkillCutIn userSkillCutIn;
  [SerializeField]
  SkillCutIn enemySkillCutIn;
  [SerializeField]
  GameObject blockSheet;
  [SerializeField]
  GameObject keepOutObj;

  HandChecker handChecker;　
  TrumpDistributeManager distributeManager;//配る用スクリプト
  GameUserData gameUserData;
  MasterSkillDB masterSkillDB;
  GameEnemySkillData gameEnemySkillData;

  float bonusRate = 0;
  int ContinueCounter = 0;
  long bet = 0;
  long firstBet = 0;

  int enableChangeNum = 5;//変更できる数
  bool isForceChange = false;
  bool isForceContinue = false;

  bool isEnablePassiveSkill = true;
  bool isEnanleMagnificationSkill = true;
  bool isEnableProbablityUpSkill = true;

  float magnification = 1;//倍率

  public enum GamePhase
  {
    Bet,//賭ける
    TrumpInit,
    EnemySkill,//ディーラースキル発動かどうか
    Distribute,//配布
    Change,
    SecondDistribute,
    Result,
    Continue//継続チェック
  }

  GamePhase gamePhase = GamePhase.Bet;
  SkillData skillData;
  SkillData passiveSkillData = null;

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
    //
    gameEnemySkillData = new GameEnemySkillData();

    button.gameObject.SetActive(false);

    SetSkill();
    playerSkillView.SetActiveSkillView(false);
    //最初のフェーズへ
    ChangePhase(gamePhase);
    SceneChanger.Instance.IsInitialize = true;
	}

  void SetEnemySkill()
  {
    var dataArray = masterSkillDB.GetEnemyDataArray();

    int[] idxArray = new int[3] { -1, -1, -1 };

    for(int i = 0; i < 3; i++)
    {
      while(true)
      {
        int idx = UnityEngine.Random.Range(0, dataArray.Length);

        if (idx != idxArray[0] && idx != idxArray[1] && idx != idxArray[2])
        {
          idxArray[i] = idx;
          break;
        }
      }
    }

    gameEnemySkillData.SetSkillData(0, dataArray[idxArray[0]]);
    gameEnemySkillData.SetSkillData(1, dataArray[idxArray[1]]);
    gameEnemySkillData.SetSkillData(2, dataArray[idxArray[2]]);
  }

  void SetSkill()
  {
    var skill1 = masterSkillDB.GetData(gameUserData.UserSkillSlot.skillSlot1);

    if (skill1 == null)
    {
      playerSkillView.SetButtonText(0, "Empty");
      playerSkillView.SetButtonInteractable(0, false);
    }
    else
    {
      if (skill1.Type == SkillData.SkillType.Passive)
      {
        passiveSkillData = skill1;
      }
      playerSkillView.SetButtonText(0, skill1.SkillName);

      Action action = () =>
      {
        gamePopupManager.OpenSkillDetailPopup(skill1,ContinueCounter, () =>
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
      if (skill2.Type == SkillData.SkillType.Passive)
      {
        passiveSkillData = skill2;
      }

      playerSkillView.SetButtonText(1, skill2.SkillName);
      Action action = () =>
      {
        gamePopupManager.OpenSkillDetailPopup(skill2,ContinueCounter,() =>
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
      if (skill3.Type == SkillData.SkillType.Passive)
      {
        passiveSkillData = skill3;
      }

      playerSkillView.SetButtonText(2, skill3.SkillName);

      Action action = () =>
      {
        gamePopupManager.OpenSkillDetailPopup(skill3,ContinueCounter, () =>
        {
          UseSkill(skill3,3);
        }, null);
      };

      playerSkillView.SetButtonAction(2, action);

    }

    var skill4 = masterSkillDB.GetData(gameUserData.UserSkillSlot.skillSlot4);


    if (skill4 == null)
    {
      playerSkillView.SetButtonText(3, "Empty");
      playerSkillView.SetButtonInteractable(3, false);
    }
    else
    {
      if (skill4.Type == SkillData.SkillType.Passive)
      {
        passiveSkillData = skill4;
      }

      playerSkillView.SetButtonText(3, skill4.SkillName);

      Action action = () =>
      {
        gamePopupManager.OpenSkillDetailPopup(skill4, ContinueCounter, () =>
        {
          UseSkill(skill4, 4);
        }, null);
      };

      playerSkillView.SetButtonAction(3, action);

    }


  }

  public void UseSkill(SkillData _skillData,int _useSlot)
  {
    skillData = _skillData;

    switch(skillData.Type)
    {
      case SkillData.SkillType.ProbabilityUp:

        handController.SetSelect(0, true);
        handController.SetSelect(1, true);
        handController.SetSelect(2, true);
        handController.SetSelect(3, true);
        handController.SetSelect(4, true);
        handController.SetAllLock(true);
        break;

      case SkillData.SkillType.Magnification:
        magnification = skillData.Effect;
        handController.SetSelect(0, true);
        handController.SetSelect(1, true);
        handController.SetSelect(2, true);
        handController.SetSelect(3, true);
        handController.SetSelect(4, true);
        handController.SetAllLock(true);
        break;
      case SkillData.SkillType.Bet:

        if(gameUserData.HaveCoin < skillData.Effect)
        {
          gameUserData.AddBetAndSave(gameUserData.HaveCoin);
        }
        else
        {
          gameUserData.AddBetAndSave((long)skillData.Effect);
        }

        SetViewHaveCoin(gameUserData.HaveCoin);
        SetViewBetCoin(gameUserData.BetCoin);
        skillData = null;
        break;

      case SkillData.SkillType.AllBet:
        handController.SetSelect(0, true);
        handController.SetSelect(1, true);
        handController.SetSelect(2, true);
        handController.SetSelect(3, true);
        handController.SetSelect(4, true);
        handController.SetAllLock(true);

        gameUserData.AddBetAndSave(gameUserData.HaveCoin);

        SetViewHaveCoin(gameUserData.HaveCoin);
        SetViewBetCoin(gameUserData.BetCoin);
        skillData = null;
        break;
    }

    playerSkillView.SetButtonInteractable(_useSlot - 1, false);

    //スキルカットイン中は画面操作できないようにする。
    blockSheet.SetActive(true);
    userSkillCutIn.Init(skillData.SkillName, "",null);
    userSkillCutIn.StartInAnimation(0.5f, () =>
    {
      userSkillCutIn.StartOutAnimation(2f, 0.5f, ()
          =>
        {
          blockSheet.SetActive(false);
        });
    });

  }

  public void UseEnemySkill(EnemySkillData _skillData)
  {
    if (_skillData == null)
    {
      gamePhase = GamePhase.Distribute;
      ChangePhase(gamePhase);
      return;
    }

    switch (_skillData.SType)
    {
      case EnemySkillData.EnemySkillType.SealSkill:
        Debug.Log("UseSeal");
        switch(_skillData.TargetSkillType)
        {
          case SkillData.SkillType.Passive:
            isEnablePassiveSkill = false;
            break;
          case SkillData.SkillType.ProbabilityUp:
            var slot = gameUserData.UserSkillSlot;

            if(!string.IsNullOrEmpty(slot.skillSlot1))
            {
              if (masterSkillDB.GetData(slot.skillSlot1).Type == SkillData.SkillType.ProbabilityUp)
              {
                playerSkillView.SetButtonKeepOut(0, true);
              }
            }

            if (!string.IsNullOrEmpty(slot.skillSlot2))
            {
              if (masterSkillDB.GetData(slot.skillSlot2).Type == SkillData.SkillType.ProbabilityUp)
              {
                playerSkillView.SetButtonKeepOut(1, true);
              }
            }

            if (!string.IsNullOrEmpty(slot.skillSlot3))
            {
              if (masterSkillDB.GetData(slot.skillSlot3).Type == SkillData.SkillType.ProbabilityUp)
              {
                playerSkillView.SetButtonKeepOut(2, true);
              }
            }

            if (!string.IsNullOrEmpty(slot.skillSlot4))
            {
              if (masterSkillDB.GetData(slot.skillSlot4).Type == SkillData.SkillType.ProbabilityUp)
              {
                playerSkillView.SetButtonKeepOut(3, true);
              }
            }

            break;
          case SkillData.SkillType.Magnification:
            var slot2 = gameUserData.UserSkillSlot;

            if (!string.IsNullOrEmpty(slot2.skillSlot1))
            {
              if (masterSkillDB.GetData(slot2.skillSlot1).Type == SkillData.SkillType.Magnification)
              {
                playerSkillView.SetButtonKeepOut(0, true);
              }
            }

            if (!string.IsNullOrEmpty(slot2.skillSlot2))
            {
              if (masterSkillDB.GetData(slot2.skillSlot2).Type == SkillData.SkillType.Magnification)
              {
                playerSkillView.SetButtonKeepOut(1, true);
              }
            }

            if (!string.IsNullOrEmpty(slot2.skillSlot3))
            {
              if (masterSkillDB.GetData(slot2.skillSlot3).Type == SkillData.SkillType.Magnification)
              {
                playerSkillView.SetButtonKeepOut(2, true);
              }
            }

            if (!string.IsNullOrEmpty(slot2.skillSlot4))
            {
              if (masterSkillDB.GetData(slot2.skillSlot4).Type == SkillData.SkillType.Magnification)
              {
                playerSkillView.SetButtonKeepOut(3, true);
              }
            }

            break;
        }

        break;
      case EnemySkillData.EnemySkillType.Killer:
        Debug.Log("UseKiller");
        distributeManager.KillTrumpFromNumber((int)_skillData.Effect);
        break;
      case EnemySkillData.EnemySkillType.ForceContinue:
        Debug.Log("UseContinue");
        isForceContinue = true;
        break;
      case EnemySkillData.EnemySkillType.ForceAllChange:
        Debug.Log("AllChange");
        isForceChange = true;
        break;
      case EnemySkillData.EnemySkillType.Order:
        Debug.Log("UseOrder");

        break;
    }

    //スキルカットイン中は画面操作できないようにする。
    blockSheet.SetActive(true);
    enemySkillCutIn.Init(_skillData.SkillName, _skillData.Dist, null);
    enemySkillCutIn.StartInAnimation(0.5f, () =>
     {
       enemySkillCutIn.StartOutAnimation(1.5f,0.5f,() =>
       {
         blockSheet.SetActive(false);
         gamePhase = GamePhase.Distribute;
         ChangePhase(gamePhase);
       });
     });
       
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

      case GamePhase.TrumpInit:
        TrumpInitPhase();
        break;

      case GamePhase.EnemySkill:
        EnemySkillPhase();
        break;

      case GamePhase.Distribute:
        DistributePhase();
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
    //仮
    gameUserData.BetCoin = 0;
    SetViewBetCoin(gameUserData.BetCoin);
    SetViewHaveCoin(gameUserData.HaveCoin);

    playerSkillView.SetButtonInteractable(0, true);
    playerSkillView.SetButtonInteractable(1, true);
    playerSkillView.SetButtonInteractable(2, true);
    playerSkillView.SetButtonInteractable(3, true);

    playerSkillView.SetButtonKeepOut(0, false);
    playerSkillView.SetButtonKeepOut(1, false);
    playerSkillView.SetButtonKeepOut(2, false);
    playerSkillView.SetButtonKeepOut(3, false);

    gameEnemySkillData.Reset();
    SetEnemySkill();


    gamePopupManager.OpenBetPopup(gameUserData.HaveCoin, 100,firstBet, (val) =>
    {
      bet = val;

      //この時点でベット確定し、セーブ
      //ベットコイン決定
      gameUserData.BetCoin = bet;
      firstBet = bet;
      //セーブ
      gameUserData.UseCoinAndSave();

      gamePhase = GamePhase.TrumpInit;
      ChangePhase(gamePhase);

    },
    ()=>
    {
      SceneChanger.Instance.ChangeScene("Home");
    });
  }

  void TrumpInitPhase()
  {

    isEnablePassiveSkill = true;
    isEnanleMagnificationSkill = true;
    isEnableProbablityUpSkill = true;
    magnification = 1;
    isForceContinue = false;
    isForceChange = false;
    keepOutObj.SetActive(false);

    playerSkillView.SetButtonKeepOut(0, false);
    playerSkillView.SetButtonKeepOut(1, false);
    playerSkillView.SetButtonKeepOut(2, false);
    playerSkillView.SetButtonKeepOut(3, false);

    distributeManager.InitTrumpList();
    for (int i = 0; i < 5; i++)
    {
      handController.SetPosition(i, new Vector2(0, 10000));
    }

    handController.SetSelect(0, false);
    handController.SetSelect(1, false);
    handController.SetSelect(2, false);
    handController.SetSelect(3, false);
    handController.SetSelect(4, false);

    //表示更新
    handController.SetAllLock(false);

    SetViewBetCoin(gameUserData.BetCoin);
    SetViewHaveCoin(gameUserData.HaveCoin);

    gamePhase = GamePhase.EnemySkill;
    ChangePhase(gamePhase);
  }


  void EnemySkillPhase()
  {
    var skill =  gameEnemySkillData.UseSkill();
    gameEnemySkillData.AddCoolTimeCnt();
    UseEnemySkill(skill);

  }

  void DistributePhase()
  {
    StartCoroutine(Distribute());
  }

  IEnumerator Distribute()
  {
    HandChecker.TrumpData[] drawArray = null;

    //パッシブスキルが有効の場合はスキル使用
    if (isEnablePassiveSkill)
    {
      drawArray = distributeManager.DrawTrumpSkill(passiveSkillData);
    }
    else
    {
      drawArray = distributeManager.DrawTrumpSkill(null);
    }

     for (int i = 0; i < 5; i++)
    {
      handController.SetHandData(i,drawArray[i]);

      handController.Move(true, i, cardMoveTime, null);
      yield return new WaitForSeconds(cardMoveTime);
    }


    while (handController.IsMove())
    {
      yield return null;
    }

    gamePhase = GamePhase.Change;
    ChangePhase(gamePhase);

  }

  void ChangePhase()
  {
    //強制チェンジなら
    if(isForceChange)
    {
      keepOutObj.SetActive(true);
      handController.SetSelect(0, true);
      handController.SetSelect(1, true);
      handController.SetSelect(2, true);
      handController.SetSelect(3, true);
      handController.SetSelect(4, true);
      handController.SetAllLock(true);
    }

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
    //制限シートは戻す
    keepOutObj.SetActive(false);

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
        yield return new WaitForSeconds(cardMoveTime);
      }

    }
    else
    {
      var hand = distributeManager.DrawTrumpSkill(skillData);
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
    //スキルビューを見えなくする
    playerSkillView.SetActiveSkillView(false);

    var type = handChecker.CheckHand(handController.GetHandData());
    handImageView.SetActive(true);
    handImageView.SetSprite(type.GetHashCode());
    handImageView.InAnimationScl(0.5f, null);
    
    //役が成立していればボーナス値も適用する
    float bonusVal = type != HandChecker.HandType.NoPair ? bonusRate : ContinueCounter = 0;
    float magnificationSum = (GameCommon.GetHandScale(type) + bonusVal) * magnification;
    gameUserData.BetCoin = (long)(gameUserData.BetCoin * magnificationSum);

    //コインの数がMaxの場合
    if(gameUserData.BetCoin >= GameCommon.maxBet)
    {
      gameUserData.BetCoin = GameCommon.maxBet;
    }

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
    long haveCoin = gameUserData.HaveCoin + gameUserData.BetCoin;
    long haveMoney = -1000;

    if (haveCoin >= GameCommon.maxCoin)
    {
      haveCoin = GameCommon.maxCoin;

      //未クリアなので借金を返すように促す
      if (haveMoney < 0)
      {
        gamePopupManager.OpenHaveCoinMaxPopup(false, () =>
        {
          gameUserData.GetCoinAndSave();
          SceneChanger.Instance.ChangeScene("Home");   
        });
      }
      else
      {
        gamePopupManager.OpenHaveCoinMaxPopup(true, () =>Continue());
      }
    }
    else if (haveCoin < 100 && gameUserData.BetCoin == 0)//コイン不足 かつ負けている
    {
      //セーブして終了
      gamePopupManager.OpenNoMoneyContinuePopup(null, () =>
      {
        gameUserData.GetCoinAndSave();
        SceneChanger.Instance.ChangeScene("Home");
      });
    }
    else
    {
      Continue();
    }
  }

  void Continue()
  {
    var type = handChecker.CheckHand(handController.GetHandData());

    if (type != HandChecker.HandType.NoPair)
    {
      ContinueCounter++;
      bonusRate = magMasterData.GetContinueMag(ContinueCounter);
      gamePopupManager.OpenContinuePopup(gameUserData.BetCoin, bonusRate, () =>
      {
        gamePhase = GamePhase.TrumpInit;
        ChangePhase(gamePhase);
      },
      () =>
      {
        FinishGame();
      },isForceContinue);
    }
    else
    {
      //外したときは一旦セーブしてベット画面に戻る
      FinishGame();
    }
  }

  void FinishGame()
  {
    ContinueCounter = 0;
    gameUserData.GetCoinAndSave();
    gamePhase = GamePhase.Bet;
    ChangePhase(gamePhase);
  }

}
