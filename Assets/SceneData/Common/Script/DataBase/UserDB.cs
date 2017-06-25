using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.DataBase;
using System;

public class UserDB : DataBase
{
  int isExistData = 0;
  UserData userData;

  string isExsistDataKey = "pfojwo23fjqwfj424qfqf";
  string skillSlotDataKey = "ojwfo[qfjowfwfqfwfwf";
  string haveMoneyKey = "29-9rhj9-12rj2-rj20r";
  string haveCoinKey = "200ujwwfnwpdnq-02-[-";
  string loginDateKey = "ojfwopfpwqofmqfo2-213";
  string loginStoreKey = "u0u-3dj-9[3r32hjr[2";
  string seKey = "ki00j0kwjq[3021.12";
  string bgmKey = "2wfwqfq-m2[[b";
  string exchangeShopRandomStateKey = "ojwfoqfiqhwfg@.[]/@[";

  public struct UserData
  {
    public long haveMoney;
    public long haveCoin;
    public string loginDate;
    public string loginStore;
    public SkillSlot skillSlot;
    public float bgmVol;
    public float seVol;
    public ExchangeRandomInfo exRandomInfo;//換金所のランダムシード
  }

  [System.Serializable]
  public struct ExchangeRandomInfo
  {
    public UnityEngine.Random.State exchangeShopRandomState;
  }

  public struct SkillSlot
  {
    public string skillSlot1;
    public string skillSlot2;
    public string skillSlot3;
    public string skillSlot4;
  }

  public bool IsExistData()
  {
    isExistData = PlayerPrefs.GetInt(isExsistDataKey, 0);
    return isExistData == 1 ? true : false;
  }

  public void LoadUserData()
  {
    string haveMoneyStr = PlayerPrefs.GetString(haveMoneyKey, "");

    if (string.IsNullOrEmpty(haveMoneyStr))
    {
      haveMoneyStr = "-1000000000";
    }

    userData.haveMoney = long.Parse(haveMoneyStr);

    string haveCoinStr = PlayerPrefs.GetString(haveCoinKey, "");

    if (string.IsNullOrEmpty(haveCoinStr))
    {
      haveCoinStr = "10000";
    }

    //音量
    userData.bgmVol = PlayerPrefs.GetFloat(seKey, 0.5f);
    userData.seVol = PlayerPrefs.GetFloat(bgmKey, 0.5f);

    userData.haveCoin = long.Parse(haveCoinStr);

    //ストアへのログインはデータ作成時は空っぽにしておく
    userData.loginStore = PlayerPrefs.GetString(loginStoreKey, "");
    //前回ログイン
    userData.loginDate = PlayerPrefs.GetString(loginDateKey, "");


    string json = PlayerPrefs.GetString(skillSlotDataKey, "");

    if (string.IsNullOrEmpty(json))
    {
      SkillSlot slot;

      slot.skillSlot1 = "";
      slot.skillSlot2 = "";
      slot.skillSlot3 = "";
      slot.skillSlot4 = "";

      userData.skillSlot = slot;
    }
    else
    {
      userData.skillSlot = JsonUtility.FromJson<SkillSlot>(json);
    }

    json = PlayerPrefs.GetString(exchangeShopRandomStateKey, "");

    if(!string.IsNullOrEmpty(json))
    {
      userData.exRandomInfo = JsonUtility.FromJson<ExchangeRandomInfo>(json);
    }


    isExistData = 1;
    PlayerPrefs.SetInt(isExsistDataKey,1);

  }

  public long GetMoney()
  {
    return userData.haveMoney;
  }

  public void SetMoney(long _money)
  {
    userData.haveMoney = _money;
  }

  public long GetCoin()
  {
    return userData.haveCoin;
  }

  public void SetCoin(long _coin)
  {
    userData.haveCoin = _coin;
  }

  public void CalcCoin(long _val)
  {
    userData.haveCoin += _val;
  }

  public void CalcMoney(long _val)
  {
    userData.haveMoney += _val;
  }

  public SkillSlot GetSkillSlot()
  {
    return userData.skillSlot;
  }

  //0スタート
  public string GetSkillIdFromSkillSlot(int _slotIdx)
  {
    switch(_slotIdx)
    {
      case 0:
        return userData.skillSlot.skillSlot1;
      case 1:
        return userData.skillSlot.skillSlot2;
      case 2:
        return userData.skillSlot.skillSlot3;
      case 3:
        return userData.skillSlot.skillSlot4;
    }
    return null;
  }

  public void SetSkillSlot(SkillSlot _skillSlot)
  {
    userData.skillSlot = _skillSlot;
  }

  //0スタート
  public void SetSkillSlot(int _slotIdx,string _skillId)
  {
    switch (_slotIdx)
    {
      case 0:
        userData.skillSlot.skillSlot1 = _skillId;
        break;

      case 1:
        userData.skillSlot.skillSlot2 = _skillId;
        break;

      case 2:
        userData.skillSlot.skillSlot3 = _skillId;
        break;

      case 3:
        userData.skillSlot.skillSlot4 = _skillId;
        break;
    }
  }

  public void SetLoginDate(string _date)
  {
    userData.loginDate = _date;
  }

  public string GetLoginDate()
  {
    return userData.loginDate;
  }

  public void SetLoginStore(string _date)
  {
    userData.loginStore = _date;
  }

  public string GetLoginStore()
  {
    return userData.loginStore;
  }

  public void SetSeVol(float _vol)
  {
    userData.seVol = _vol;
  }

  public float GetSeVol()
  {
    return userData.seVol;
  }

  public void SetBgmVol(float _vol)
  {
    userData.bgmVol = _vol;
  }

  public float GetBgmVol()
  {
    return userData.bgmVol;
  }

  public void SetExchangeShopRandomState(UnityEngine.Random.State state)
  {
    userData.exRandomInfo.exchangeShopRandomState = state;
  }

  public UnityEngine.Random.State GetExchangeShopRandomState()
  {
    return userData.exRandomInfo.exchangeShopRandomState;
  }

  //Save

  public void SaveHaveMoney()
  {
    PlayerPrefs.SetString(haveMoneyKey, userData.haveMoney.ToString());
  }

  public void SaveHaveCoin()
  {
    PlayerPrefs.SetString(haveCoinKey, userData.haveCoin.ToString());
  }

  public void SaveLoginDate()
  {
    PlayerPrefs.SetString(loginDateKey, userData.loginDate);
  }

  public void SaveSkillSlot()
  {
    string json = JsonUtility.ToJson(userData.skillSlot);
    PlayerPrefs.SetString(skillSlotDataKey, json);
  }

  public void SaveLoginStore()
  {
    PlayerPrefs.SetString(loginStoreKey, userData.loginStore);
  }

  public void SaveSeVol()
  {
    PlayerPrefs.SetFloat(seKey, userData.seVol);
  }

  public void SaveBgmVol()
  {
    PlayerPrefs.SetFloat(bgmKey, userData.bgmVol);
  }

  public void SaveExchangeShopRandomState()
  {
    string json = JsonUtility.ToJson(userData.exRandomInfo);
    PlayerPrefs.SetString(exchangeShopRandomStateKey, json);
  }

  public void AllSave()
  {
    SaveHaveCoin();
    SaveHaveMoney();
    SaveLoginDate();
    SaveLoginStore();
    SaveSkillSlot();
    SaveBgmVol();
    SaveSeVol();
    SaveExchangeShopRandomState();
  }

  public void DeleteUserData()
  {
    PlayerPrefs.DeleteAll();
  }
}