using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using TMPro;
using Common.DataBase;

public class HomeManager : MonoBehaviour
{
  [SerializeField]
  TextMeshProUGUI haveCoin;
  [SerializeField]
  TextMeshProUGUI haveMoney;

  UserDB userDB;
	// Use this for initialization
	void Start ()
  {
    userDB = DataBaseManager.Instance.GetDataBase<UserDB>();

    long coin = userDB.GetCoin();
    long money = userDB.GetMoney();

    haveCoin.text = coin.ToString()+"枚";
    haveMoney.text = money.ToString()+"円";
    SceneChanger.Instance.IsInitialize = true;
	}
	
	// Update is called once per frame
	void Update ()
  {
		
	}
}
