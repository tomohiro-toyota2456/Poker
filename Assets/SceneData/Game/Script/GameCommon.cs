using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 static public class GameCommon
{
  static public readonly long maxMoney = 100000000;
  static public readonly long maxCoin  = 999999999;
  static public readonly int skillSlot = 3;
  static public readonly long maxBet = 5000000000;
  static public readonly int maxItem = 99;
  static public readonly int LoginBonus = 500;

  static public float GetHandScale(HandChecker.HandType _type)
  {
    switch(_type)
    {
      case HandChecker.HandType.NoPair:
        return 0;
      case HandChecker.HandType.OnePair:
        return 1.0f;
      case HandChecker.HandType.TwoPair:
        return 1.0f;
      case HandChecker.HandType.ThreeCard:
        return 1.0f;
      case HandChecker.HandType.Straight:
        return 3.0f;
      case HandChecker.HandType.Flush:
        return 4.0f;
      case HandChecker.HandType.FullHouse:
        return 7.0f;
      case HandChecker.HandType.FourCard:
        return 10.0f;
      case HandChecker.HandType.StraightFlush:
        return 25.0f;
      case HandChecker.HandType.RoyalStraightFlush:
        return 500.0f;
    }

    return 0;//ここはないはず
  }

  static public float GetBunus(int _counter)
  {
    float per = 0.1f;

    int offset = _counter / 5;

    per = per * (float)offset;

    return per;
  }

  static public HandChecker.MarkType[] ConvertMarkFromColor(EnemySkillData.ColorType _colorType)
  {
    HandChecker.MarkType[] markType = new HandChecker.MarkType[2];

    switch(_colorType)
    {
      case EnemySkillData.ColorType.Red:
        markType[0] = HandChecker.MarkType.Heart;
        markType[1] = HandChecker.MarkType.Dia;
        break;

      case EnemySkillData.ColorType.Black:
        markType[0] = HandChecker.MarkType.Clover;
        markType[1] = HandChecker.MarkType.Spade;
        break;
    }

    return markType;
  }

}
