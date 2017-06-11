using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UniRx.Triggers;
using Common;

public class SkillSlotObj : MonoBehaviour
{
  [SerializeField]
  TextMeshProUGUI skillName;
  [SerializeField]
  GameObject passiveObj;
  [SerializeField]
  Button moveSetSkillButton;

  public void Init(int _slotNum,string _skillName,SkillData.SkillType _skillType)
  {
    if (string.IsNullOrEmpty(_skillName))
    {
      skillName.text = "Empty";
    }
    else
    {
      skillName.text = _skillName;
    }

    //パッシブスキルのときのみパッシブ表記する
    if(_skillType == SkillData.SkillType.Passive)
    {
      passiveObj.SetActive(true);
    }

    moveSetSkillButton.OnClickAsObservable()
      .Take(1)
      .Subscribe(_ =>
      {
        SkillSetManager.SlotId = _slotNum;//変更するスロットを教える
        SceneChanger.Instance.ChangeScene("SkillSet");
      }).AddTo(gameObject);

  }

}
