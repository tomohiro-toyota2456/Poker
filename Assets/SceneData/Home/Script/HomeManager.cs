using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using TMPro;
using Common.DataBase;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class HomeManager : MonoBehaviour
{
  [SerializeField]
  TextMeshProUGUI haveCoin;
  [SerializeField]
  TextMeshProUGUI haveMoney;
  [SerializeField]
  OptionPopup optionPopup;
  [SerializeField]
  Button optionButton;

  UserDB userDB;
  PopupManager ppManager;
	// Use this for initialization
	void Start ()
  {
    userDB = DataBaseManager.Instance.GetDataBase<UserDB>();
    ppManager = PopupManager.Instance;

    long coin = userDB.GetCoin();
    long money = userDB.GetMoney();

    haveCoin.text = coin.ToString()+"枚";
    haveMoney.text = money.ToString()+"円";
    SceneChanger.Instance.IsInitialize = true;

    optionButton.OnClickAsObservable()
      .Subscribe(_ =>
      {
        OpenOptionPopup();
      }).AddTo(gameObject);

	}

  public void OpenOptionPopup()
  {
    var pp = ppManager.Create<OptionPopup>(optionPopup);
    pp.Init(userDB);
    ppManager.OpenPopup(pp,null);
  }
}
