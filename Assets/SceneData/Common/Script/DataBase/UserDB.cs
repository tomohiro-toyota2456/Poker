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

  public struct UserData
  {
    public long haveMoney;
    public long haveCoin;
    public string loginDate;
    public string loginStore;
    public SkillSlot skillSlot;
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

    userData.haveCoin = long.Parse(haveCoinStr);

    //ストアへのログインはデータ作成時は空っぽにしておく
    userData.loginStore = PlayerPrefs.GetString(loginStoreKey, "");


    string json = PlayerPrefs.GetString(skillSlotDataKey, "");

    if (string.IsNullOrEmpty(json))
    {
      SkillSlot slot;

      slot.skillSlot1 = "s00001";
      slot.skillSlot2 = "s00002";
      slot.skillSlot3 = "s00003";
      slot.skillSlot4 = "";

      userData.skillSlot = slot;
    }
    else
    {
      userData.skillSlot = JsonUtility.FromJson<SkillSlot>(json);
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

  public SkillSlot GetSkillSlot()
  {
    return userData.skillSlot;
  }

  public void SetSkillSlot(SkillSlot _skillSlot)
  {
    userData.skillSlot = _skillSlot;
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

  public void AllSave()
  {
    SaveHaveCoin();
    SaveHaveMoney();
    SaveLoginDate();
    SaveLoginStore();
    SaveSkillSlot();
  }

  public void DeleteUserData()
  {
    PlayerPrefs.DeleteAll();
  }
}