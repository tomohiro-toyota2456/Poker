using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.DataBase;
using System.Linq;

public class HaveSkillDB : DataBase
{
  string key = "whPHFPFNHWocj[-j";

  HaveSkillList haveList;

  [System.Serializable]
  public struct HaveSkillList
  {
    public List<string> list;
  }

  public void Load()
  {
    string json = PlayerPrefs.GetString(key);

    if (string.IsNullOrEmpty(json))
    {
      haveList.list = new List<string>();
      json = JsonUtility.ToJson(haveList);
      PlayerPrefs.SetString(key, json);
    }

    haveList = JsonUtility.FromJson<HaveSkillList>(json);
  }

  public void Save()
  {
    string json = JsonUtility.ToJson(haveList);
    PlayerPrefs.SetString(key, json);
  }

  public bool CheckHave(string _skillId)
  {
    return haveList.list.Any(data => data == _skillId);
  }

  public void SetData(string _skillId)
  {
    if (!haveList.list.Any(data => data == _skillId))
    {
      string buff = _skillId.Substring(0);
      haveList.list.Add(buff);
    }
  }

  public string[] GetDataArray()
  {
    return haveList.list.ToArray();
  }

  public List<string> GetDataList()
  {
    return haveList.list.ToList();
  }

}
