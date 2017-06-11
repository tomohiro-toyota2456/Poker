using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Common.DataBase;
using UniRx;
using UniRx.Triggers;
using Common;

public class SkillShopListItem : MonoBehaviour
{
  [SerializeField]
  TextMeshProUGUI skillNameText;
  [SerializeField]
  TextMeshProUGUI skillCoolTimeNumText;
  [SerializeField]
  TextMeshProUGUI valueNumText;
  [SerializeField]
  Button buyButton;
  [SerializeField]
  SkillBuyConfirmPopup skillBuyConfirmPopup;
  [SerializeField]
  GameObject passiveObj;

  static HaveSkillDB haveSkillDB;
  static UserDB userDB;

  public void Init(string _skillId,string _skillName,string  _skillDist,int _skillCoolTime,SkillData.SkillType _skillType,int _value)
  {
    if (haveSkillDB == null)
    {
      haveSkillDB = DataBaseManager.Instance.GetDataBase<HaveSkillDB>();
    }

    if (userDB == null)
    {
      userDB = DataBaseManager.Instance.GetDataBase<UserDB>();
    }

    if(_skillType == SkillData.SkillType.Passive)
    {
      passiveObj.SetActive(true);
    }

    skillNameText.text = _skillName;
    skillCoolTimeNumText.text = _skillCoolTime.ToString();
    valueNumText.text = _value.ToString();

    buyButton.OnClickAsObservable()
      .Subscribe(_ =>
      {
        OpenBuyPopup(_skillId,_skillName,_skillDist,_value);
      }).AddTo(gameObject);

  }

  public void OpenBuyPopup(string _skillId,string _skillName,string _skillDist,int _value)
  {
    var ppManager = PopupManager.Instance;

    var pp = ppManager.Create<SkillBuyConfirmPopup>(skillBuyConfirmPopup);
    pp.Init(_skillName, _skillDist, _value,userDB.GetCoin(), () =>
       {
         haveSkillDB.SetData(_skillId);
         userDB.CalcCoin(-_value);
         haveSkillDB.Save();
         userDB.SaveHaveCoin();
         gameObject.SetActive(false);//親オブジェクトがこのスクリプトをつけている前提
       });

    ppManager.OpenPopup(pp, null);
  }

}
