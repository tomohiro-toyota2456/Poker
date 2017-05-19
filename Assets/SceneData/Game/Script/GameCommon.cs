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
        return 1.5f;
      case HandChecker.HandType.ThreeCard:
        return 2.0f;
      case HandChecker.HandType.Straight:
        return 3.0f;
      case HandChecker.HandType.Flush:
        return 4.0f;
      case HandChecker.HandType.FullHouse:
        return 5.0f;
      case HandChecker.HandType.FourCard:
        return 7.0f;
      case HandChecker.HandType.StraightFlush:
        return 9.0f;
      case HandChecker.HandType.RoyalStraightFlush:
        return 120.0f;
    }

    return 0;//ここはないはず
  }
}
