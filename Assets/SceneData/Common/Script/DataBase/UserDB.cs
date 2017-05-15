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

  public struct UserData
  {
    public long haveMoney;
    public long haveCoin;
    public string loginDate;
    public SkillSlot skillSlot;
  }

  public struct SkillSlot
  {
    public string skillSlot1;
    public string skillSlot2;
    public string skillSlot3;
  }

  public bool IsExistData()
  {
    isExistData = PlayerPrefs.GetInt(isExsistDataKey, 0);
    return isExistData == 1 ? true : false;
  }

  public void LoadUserData()
  {
    string haveMoneyStr = PlayerPrefs.GetString(haveMoneyKey, "");

    if (string.IsNullOrEmpty(haveMoneyKey))
    {
      haveMoneyStr = "-1000000000";
    }

    userData.haveMoney = long.Parse(haveMoneyStr);

    string haveCoinStr = PlayerPrefs.GetString(haveCoinKey, "");

    if (string.IsNullOrEmpty(haveCoinKey))
    {
      haveCoinStr = "1000";
    }

    userData.haveCoin = long.Parse(haveCoinStr);

    userData.loginDate = PlayerPrefs.GetString(loginDateKey, "");

    if (string.IsNullOrEmpty(userData.loginDate))
    {
      userData.loginDate = DateTime.Now.ToString();
    }

    string json = PlayerPrefs.GetString(skillSlotDataKey, "");

    if (string.IsNullOrEmpty(json))
    {
      SkillSlot slot;

      slot.skillSlot1 = "";
      slot.skillSlot2 = "";
      slot.skillSlot3 = "";

      userData.skillSlot = slot;
    }
    else
    {
      userData.skillSlot = JsonUtility.FromJson<SkillSlot>(json);
    }

    isExistData = 1;
    PlayerPrefs.SetInt(isExsistDataKey,1);

  }
}