using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 static public class GameCommon
{
  static public readonly long maxMoney = 100000000;
  static public readonly long maxCoin  = 999999999;
  static public readonly int skillSlot = 3;
  static public readonly long maxBet = 5000000000;

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
        return 3.0f;
      case HandChecker.HandType.Straight:
        return 5.0f;
      case HandChecker.HandType.Flush:
        return 7.0f;
      case HandChecker.HandType.FullHouse:
        return 10.0f;
      case HandChecker.HandType.FourCard:
        return 20.0f;
      case HandChecker.HandType.StraightFlush:
        return 50.0f;
      case HandChecker.HandType.RoyalStraightFlush:
        return 100.0f;
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

}
