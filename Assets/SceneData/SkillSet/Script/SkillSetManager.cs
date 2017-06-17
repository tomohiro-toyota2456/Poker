using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.DataBase;
using Common;

public class SkillSetManager : MonoBehaviour
{
  [SerializeField]
  SkillListManager manager;
  [SerializeField]
  SimplePopup simplePopup;
  public static int slotId = -1;
  public static int SlotId { set { slotId = value; } }
  // Use this for initialization

  UserDB userDB;
  HaveSkillDB haveSkillDB;
  MasterSkillDB masterSkillDB;
  PopupManager ppManager;
  int passiveSlot = -1;
	void Start ()
  {
    userDB = DataBaseManager.Instance.GetDataBase<UserDB>();
    haveSkillDB = DataBaseManager.Instance.GetDataBase<HaveSkillDB>();
    masterSkillDB = DataBaseManager.Instance.GetDataBase<MasterSkillDB>();
    ppManager = PopupManager.Instance;
    var idArray = haveSkillDB.GetDataArray();

    //パッシブスキルをつけているスロットを探す
    passiveSlot = GetSettingPassiveSlot();
    //持っているスキルをリスト表示
    for(int i = 0; i < idArray.Length; i++)
    {
      var skillData = masterSkillDB.GetData(idArray[i]);

      //今使ってるスキルかどうかをチェック
      int usedSlotNum = CheckUsedSkillId(idArray[i]);
      manager.AddItem(idArray[i], skillData.SkillName, skillData.CoolTime, skillData.Type,SetSkill,usedSlotNum );
    }

    SceneChanger.Instance.IsInitialize = true;
  }

  int GetSettingPassiveSlot()
  {
    var skillSlots = userDB.GetSkillSlot();

    if (!string.IsNullOrEmpty(skillSlots.skillSlot1))
    {
      if (masterSkillDB.GetData(skillSlots.skillSlot1).Type == SkillData.SkillType.Passive)
      {
        return 0;
      }
    }

    if (!string.IsNullOrEmpty(skillSlots.skillSlot2))
    {
      if (masterSkillDB.GetData(skillSlots.skillSlot2).Type == SkillData.SkillType.Passive)
      {
        return 1;
      }
    }

    if (!string.IsNullOrEmpty(skillSlots.skillSlot3))
    {
      if (masterSkillDB.GetData(skillSlots.skillSlot3).Type == SkillData.SkillType.Passive)
      {
        return 2;
      }
    }

    if (!string.IsNullOrEmpty(skillSlots.skillSlot4))
    {
      if (masterSkillDB.GetData(skillSlots.skillSlot4).Type == SkillData.SkillType.Passive)
      {
        return 3;
      }
    }

    return -1;
  }

  int CheckUsedSkillId(string _id)
  {
    var skillSlots = userDB.GetSkillSlot();
    
    if(skillSlots.skillSlot1 == _id)
    {
      return 0;
    }

    if (skillSlots.skillSlot2 == _id)
    {
      return 1;
    }

    if (skillSlots.skillSlot3 == _id)
    {
      return 2;
    }

    if (skillSlots.skillSlot4 == _id)
    {
      return 3;
    }

    return -1;
  }

  public void SetSkill(string _skillId,SkillListItem _item)
  {
    bool isPassiveSkill = masterSkillDB.GetData(_skillId).Type == SkillData.SkillType.Passive;

    if (isPassiveSkill)
    {
      //すでにパッシブがセットされているか
      if(passiveSlot !=-1 && passiveSlot != slotId)
      {
        var pp = ppManager.Create<SimplePopup>(simplePopup);
        pp.Init(SimplePopup.PopupType.YesNo, "警告", "すでに別スロットにパッシブスキルがセットされています。別スロットのパッシブスキルは外れますがセットしますか？", () =>
           {
             //特に何もない場合
             var ppok = ppManager.Create<SimplePopup>(simplePopup);
             ppok.Init(SimplePopup.PopupType.Close, "確認", "スロット" + (slotId + 1).ToString() + "にセットしました");
             ppManager.OpenPopup(ppok, null);

             //元のスロットは空にし、今のスロットに装着
             userDB.SetSkillSlot(slotId, _skillId);
             userDB.SetSkillSlot(passiveSlot, "");
             userDB.SaveSkillSlot();

             //入れ替え前についていたスキル表示の更新
             var prevItem = manager.SearchItem(slotId);
             if (prevItem != null)
             {
               prevItem.SetSlot = -1;
               prevItem.SetSkillSetText(-1);
             }

             _item.SetSkillSetText(slotId);
             _item.SetSlot = slotId;

             //パッシブがついているスロットの更新
             passiveSlot = slotId;
           }, null);

        ppManager.OpenPopup(pp, null);

        return;
      }
    }


    int id = CheckUsedSkillId(_skillId);
    if(id != slotId && id != -1)
    {
      var pp = ppManager.Create<SimplePopup>(simplePopup);
      pp.Init(SimplePopup.PopupType.YesNo, "警告", "すでに別スロットにセットされているスキルです。別スロットのスキルは外れますがセットしますか?", () =>
      {
        //特に何もない場合
        var ppok = ppManager.Create<SimplePopup>(simplePopup);
        ppok.Init(SimplePopup.PopupType.Close, "確認", "スロット" + (slotId + 1).ToString() + "にセットしました");
        ppManager.OpenPopup(ppok, null);

        //元のスロットは空にし、今のスロットに装着
        userDB.SetSkillSlot(slotId, _skillId);
        userDB.SetSkillSlot(id, "");
        userDB.SaveSkillSlot();

        //入れ替え前についていたスキル表示の更新
        var prevItem = manager.SearchItem(slotId);
        if (prevItem != null)
        {
          prevItem.SetSlot = -1;
          prevItem.SetSkillSetText(-1);
        }

        _item.SetSkillSetText(slotId);
        _item.SetSlot = slotId;

      }, null);
      ppManager.OpenPopup(pp, null);
    }
    else
    {
      //特に何もない場合
      var pp = ppManager.Create<SimplePopup>(simplePopup);
      pp.Init(SimplePopup.PopupType.Close, "確認", "スロット" + (slotId + 1).ToString() + "にセットしました");
      ppManager.OpenPopup(pp, null);
      userDB.SetSkillSlot(slotId, _skillId);
      userDB.SaveSkillSlot();

      //入れ替え前についていたスキル表示の更新
      var prevItem = manager.SearchItem(slotId);
      if (prevItem != null)
      {
        prevItem.SetSlot = -1;
        prevItem.SetSkillSetText(-1);
      }

      _item.SetSkillSetText(slotId);
      _item.SetSlot = slotId;

      if (isPassiveSkill)
      {
        passiveSlot = slotId;
      }
    }
  }
	
}
