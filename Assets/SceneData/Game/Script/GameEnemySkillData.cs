using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ゲーム中に所持している敵のスキル
public class GameEnemySkillData
{
  EnemySkillData[] skillData = new EnemySkillData[3];
  int useSkillCount = 0;
  public int UseSkillCount {get { return useSkillCount; } }

  int coolTimeCnt = 0;
  int coolTime = 3;

  public void SetSkillData(int _idx,EnemySkillData _data)
  {
    skillData[_idx] = _data;
  }

  public void Reset()
  {
    coolTime = coolTime = Random.Range(3, 6);
    coolTimeCnt = 0;
    useSkillCount = 0;
  }

  public EnemySkillData UseSkill()
  {
    if (coolTimeCnt < coolTime)
      return null;

    if (useSkillCount >= skillData.Length)
      return null;

    int idx = useSkillCount;
    useSkillCount++;

    coolTimeCnt = 0;
    coolTime = Random.Range(3, 6);

    return skillData[idx];
  }
   
  public void AddCoolTimeCnt()
  {
    coolTimeCnt++;
  }
}
