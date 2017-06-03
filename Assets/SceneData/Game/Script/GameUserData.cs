using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ゲーム中のユーザーデータ
public struct GameUserData
{
  long haveCoin;//所持Coin
  long betCoin;//現在BetCoin
  UserDB.SkillSlot userSkillSlot;
  UserDB userDB;

  public long HaveCoin { get { return haveCoin; }}
  public long BetCoin { get { return betCoin; }set { betCoin = value; } }
  public UserDB.SkillSlot UserSkillSlot { get { return userSkillSlot; } set { userSkillSlot = value; } }
  
  //初期ロード
  public void LoadUserDB(UserDB _db)
  {
    userDB = _db;
    haveCoin = userDB.GetCoin();
    betCoin = 0;
    userSkillSlot = userDB.GetSkillSlot();

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
