using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//ポーカーの役のチェックを行うクラス
public class HandChecker
{
  public enum MarkType
  {
    Heart,
    Spade,
    Dia,
    Clover
  }

  public enum HandType
  {
    NoPair,
    OnePair,
    TwoPair,
    ThreeCard,
    Straight,
    Flush,
    FullHouse,
    FourCard,
    StraightFlush,
    RoyalStraightFlush,
  }

  public struct TrumpData
  {
    public MarkType mark;
    public int number; 
  }

  public HandType CheckHand(TrumpData[] _handArray)
  {
    // false 0 true 1 ロイヤルストレート 2
    int resultNum = CheckStraight(_handArray);

    //ストレートは確定
    if (resultNum >= 1)
    {
      //フラッシュかどうかチェック
      if(CheckFlush(_handArray))
      {
        return resultNum == 2 ? HandType.RoyalStraightFlush : HandType.StraightFlush;
      }
      else
      {
        return HandType.Straight;
      }
    }

    resultNum = CheckSameNumberMax(_handArray);

    if(resultNum == 4)
    {
      return HandType.FourCard;
    }
    else if(resultNum == 3)
    {
      //ペアが一組あればフルハウス確定
      return CheckPair(_handArray) == 1 ? HandType.FullHouse : HandType.ThreeCard;
    }

    if(CheckFlush(_handArray))
    {
      return HandType.Flush;
    }

    resultNum = CheckPair(_handArray);

    //ペア数をみる
    if(resultNum == 2)
    {
      return HandType.TwoPair;
    }
    else if(resultNum == 1)
    {
      return HandType.OnePair;
    }

    return HandType.NoPair;
  }

  #region Checker
  //ペアの数をチェックする
  //戻り値はペア数
  int CheckPair(TrumpData[] _handArray)
  {
    Dictionary<int, int> dict = new Dictionary<int, int>();

    for(int i = 0; i < _handArray.Length; i++)
    {
      int number = _handArray[i].number;
      if (dict.ContainsKey(number))
      {
        dict[number] ++;
      }
      else
      {
        dict.Add(number, 1);
      }
    }

    int pairNum = 0;

    foreach(var items in dict)
    {
      if(items.Value == 2)
      {
        pairNum++;
      }
    }

    return pairNum;
  }

  //同じ数値のカードの枚数が一番多いのを返す
  //スリーカード　フォーカード検出で使用
  int CheckSameNumberMax(TrumpData[] _handArray)
  {
    Dictionary<int, int> dict = new Dictionary<int, int>();

    for (int i = 0; i < _handArray.Length; i++)
    {
      int number = _handArray[i].number;
      if (dict.ContainsKey(number))
      {
        dict[number]++;
      }
      else
      {
        dict.Add(number, 1);
      }
    }

    int sum = 0;

    foreach (var items in dict)
    {
      if (items.Value >= sum)
      {
        sum = items.Value;
      }
    }

    return sum;
  }

  //フラッシュチェック
  //マークが一つでも違えばfalseを返す
  public bool CheckFlush(TrumpData[] _handArray)
  {
    for(int i = 1; i < _handArray.Length; i++)
    {
      if(_handArray[i].mark != _handArray[0].mark)
      {
        return false;
      }
    }

    return true;
  }

  //　false = 0 true = 1 特殊ストレート 2
  public int CheckStraight(TrumpData[] _handArray)
  {
    TrumpData[] clone = null;
    _handArray.CopyTo(clone, 0);

    //数値が低い順にソート
    //ポーカーの場合は数値の並び＝強さにならないが今回の場合は気にする必要がない
    //役があるかどうかのため
    Array.Sort<TrumpData>(clone, (x, y) =>
     {
       if(x.number < y.number )
       {
         return -1;
       }
       else if(x.number > y.number)
       {
         return 1;
       }

       return 0;
     });

    int checkNum = clone[0].number+1;
    bool isStraight = true;
    for(int i = 1 ; i < clone.Length ; i++)
    {
      if(clone[i].number != checkNum)
      {
        isStraight = false;
        break;
      }
      checkNum++;
    }

    
    if(!isStraight)
    {
      //唯一連番でなくてもつながるのは 10 11 12 13 1という組み合わせのみなのでそれをチェック
      if(clone[0].number == 1 && clone[1].number == 10 && clone[2].number == 11 && clone[3].number == 12 && clone[4].number == 13)
      {
        return 2;
      }
    }

    return isStraight ? 1 : 0;
  }

  #endregion

}
