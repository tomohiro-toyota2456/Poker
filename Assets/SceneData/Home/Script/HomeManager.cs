using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using TMPro;
using Common.DataBase;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;

public class HomeManager : MonoBehaviour
{
  [SerializeField]
  TextMeshProUGUI haveCoin;
  [SerializeField]
  TextMeshProUGUI haveMoney;
  [SerializeField]
  OptionPopup optionPopup;
  [SerializeField]
  SimplePopup simplePopup;
  [SerializeField]
  Button optionButton;

  UserDB userDB;
  PopupManager ppManager;
	// Use this for initialization
	void Start ()
  {
    userDB = DataBaseManager.Instance.GetDataBase<UserDB>();
    ppManager = PopupManager.Instance;

    optionButton.OnClickAsObservable()
      .Subscribe(_ =>
      {
        OpenOptionPopup();
      }).AddTo(gameObject);

    //ログインボーナスチェックとログイン日時更新
    if(CheckLogin())
    {
      userDB.CalcCoin(GameCommon.LoginBonus);
      userDB.SaveHaveCoin();
      OpenLoginBonusPopup();
    }

    long coin = userDB.GetCoin();
    long money = userDB.GetMoney();

    haveCoin.text = coin.ToString() + "枚";
    haveMoney.text = money.ToString() + "円";

    SceneChanger.Instance.IsInitialize = true;
  }

  public bool CheckLogin()
  {
    DateTime now = DateTime.Now;
    string login = now.ToString();

    if (string.IsNullOrEmpty(userDB.GetLoginDate()))
    {
      userDB.SetLoginDate(login);
      userDB.SaveLoginDate();
      Debug.Log(login);
      return true;
    }


    DateTime prevLogin = DateTime.Parse(userDB.GetLoginDate());
    var diff = now - prevLogin;

    userDB.SetLoginDate(login);
    userDB.SaveLoginDate();
    Debug.Log(login);

    if (diff.Days >= 1)
    {
      return true;
    }

    return false;
  }

  public void OpenOptionPopup()
  {
    var pp = ppManager.Create<OptionPopup>(optionPopup);
    pp.Init(userDB);
    ppManager.OpenPopup(pp,null);
  }

  public void OpenLoginBonusPopup()
  {
    string nl = System.Environment.NewLine;

    var pp = ppManager.Create<SimplePopup>(simplePopup);
    pp.Init(SimplePopup.PopupType.Close, "ログインボーナス", "ログインボーナス!" + nl + "コイン" + GameCommon.LoginBonus.ToString() + "枚ゲット!");
    ppManager.OpenPopup(pp, null);
  }
}
