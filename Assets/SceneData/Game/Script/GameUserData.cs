﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ゲーム中のユーザーデータ
public struct GameUserData
{
  long haveCoin;//所持Coin
  long betCoin;//現在BetCoin
  UserDB.SkillSlot userSkillSlot;
  bool isUseSkill1;
  bool isUseSkill2;
  bool isUseSkill3;
  UserDB userDB;

  public long HaveCoin { get { return haveCoin; }}
  public long BetCoin { get { return betCoin; }set { betCoin = value; } }
  public UserDB.SkillSlot UserSkillSlot { get { return userSkillSlot; } set { userSkillSlot = value; } }
  public bool IsUseSkill1 { get { return isUseSkill1; }set { isUseSkill1 = value; } }
  public bool IsUseSkill2 { get { return isUseSkill2; } set { isUseSkill2 = value; } }
  public bool IsUseSkill3 { get { return isUseSkill3; } set { isUseSkill3 = value; } }

  //初期ロード
  public void LoadUserDB(UserDB _db)
  {
    userDB = _db;
    haveCoin = userDB.GetCoin();
    betCoin = 0;
    userSkillSlot = userDB.GetSkillSlot();
    isUseSkill1 = false;
    isUseSkill2 = false;
    isUseSkill3 = false;
  }

  public void UseSkill(int _skillSlot)
  {
    switch(_skillSlot)
    {
      case 1:
        isUseSkill1 = false;
        break;

      case 2:
        isUseSkill2 = false;
        break;

      case 3:
        isUseSkill3 = false;
        break;
    }
  }

  //ゲームユーザーデータの所持コインの更新はこの二つの関数で行う
  public void UseCoinAndSave()
  {
    //現在のベット数から所持コインを引いてセーブする
    haveCoin = haveCoin - betCoin;
    userDB.SetCoin(haveCoin);
    userDB.SaveHaveCoin();
  }

  //追加
  public void AddBetAndSave(long _val)
  {
    betCoin += _val;
    haveCoin -= _val;
    userDB.SetCoin(haveCoin);
    userDB.SaveHaveCoin();
  }

  public void GetCoinAndSave()
  {
    //現在のベット数から所持コインを足して
    haveCoin = haveCoin + betCoin;

    if(haveCoin >= GameCommon.maxCoin)
    {
      haveCoin = GameCommon.maxCoin;
    }

    userDB.SetCoin(haveCoin);
    userDB.SaveHaveCoin();
  }

}
