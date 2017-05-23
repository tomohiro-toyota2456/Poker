using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class TrumpDistributeManager
{
  List<HandChecker.TrumpData> trumpList = new List<HandChecker.TrumpData>();

  int[] numberSumArray = new int[13];
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void InitTrumpList()
  {
    if(trumpList != null)
    {
      trumpList.Clear();
    }

    trumpList = new List<HandChecker.TrumpData>();
    var trumpList2 = new List<HandChecker.TrumpData>();

    for(int i = 0; i <= HandChecker.MarkType.Clover.GetHashCode();i++)
    {
      for(int j = 0; j < 13; j++)
      {
        HandChecker.TrumpData data;
        data.mark = (HandChecker.MarkType)i;
        data.number = j + 1;

        trumpList.Add(data);
      }
    }

    while(trumpList.Count != 0)
    {
      trumpList2.Add(DrawTrump());
    }

    trumpList = trumpList2;

    for(int i = 0; i < 13; i++)
    {
      numberSumArray[i] = 4;
    }
  }

  //特定の数値のカードを捨てる
  public void KillTrumpFromNumber(int _number)
  {
    var numArray = trumpList.Where(_ => _.number == _number).ToArray();

    foreach(var data in numArray)
    {
      trumpList.Remove(data);
      UpdateNumberSum(data.number);
    }
  }

  public HandChecker.TrumpData DrawTrump()
  {
    int idx = Random.Range(0, trumpList.Count);

    HandChecker.TrumpData data = trumpList[idx];
    trumpList.RemoveAt(idx);

    //残りナンバー数を更新
    UpdateNumberSum(data.number);
    return data;
  }

  void UpdateNumberSum(int _number)
  {
    numberSumArray[_number - 1]--;
  }

  public HandChecker.TrumpData[] DrawTrumpSkill(SkillData _skillData)
  {
    if(_skillData == null)
    {
      return DrawRandomFive();
    }

    int per = Random.Range(0, 100);

    if(per >= _skillData.Effect)
    {
      return DrawRandomFive();
    }

    //パッシブか確率アップであれば
    if(_skillData.Type == SkillData.SkillType.Passive || _skillData.Type == SkillData.SkillType.ProbabilityUp)
    {
      switch (_skillData.Hand)
      {
        case HandChecker.HandType.OnePair:
          return DrawOnePair();
        case HandChecker.HandType.TwoPair:
          return DrawTwoPair();
        case HandChecker.HandType.ThreeCard:
          return DrawThreeCard();
        case HandChecker.HandType.Straight:
          return DrawStraight();
        case HandChecker.HandType.FourCard:
          return DrawFourCard();
        case HandChecker.HandType.FullHouse:
          return DrawFullHouse();
        case HandChecker.HandType.StraightFlush:
          return DrawStraightFlush();
        case HandChecker.HandType.RoyalStraightFlush:
          return DrawRoyalStraightFlush();
      }

    }

    return DrawRandomFive();
  }

  //固定数値を引きます。しかしない場合はランダム引きになります。
  /*
  HandChecker.TrumpData DrawFixedNumber(int _number)
  {
    var array = trumpList.Where(data => data.number == _number).ToArray();

    if (array != null && array.Length != 0)
    {
      int selectCardIdx = Random.Range(0, array.Length);
      HandChecker.TrumpData trumpData = array[selectCardIdx];
      trumpList.Remove(trumpData);
      return trumpData;
    }

    return DrawTrump();
  }

  HandChecker.TrumpData DrawFixedMark(HandChecker.MarkType _type)
  {
    var array = trumpList.Where(data => data.mark == _type).ToArray();

    if (array != null && array.Length != 0)
    {
      int selectCardIdx = Random.Range(0, array.Length);
      HandChecker.TrumpData trumpData = array[selectCardIdx];
      trumpList.Remove(trumpData);
      return trumpData;
    }

    return DrawTrump();
  }
  */
  HandChecker.TrumpData[] DrawFlush()
  {
    HandChecker.TrumpData[] handArray = new HandChecker.TrumpData[HandData.handMax];

    List<HandChecker.MarkType> markList = new List<HandChecker.MarkType>();
    markList.Add(HandChecker.MarkType.Clover);
    markList.Add(HandChecker.MarkType.Dia);
    markList.Add(HandChecker.MarkType.Heart);
    markList.Add(HandChecker.MarkType.Spade);

    while (markList.Count != 0)
    {
      int markIdx = Random.Range(0, markList.Count);
      HandChecker.MarkType mark = markList[markIdx];
      var list = trumpList.Where(data => data.mark == mark).ToList();

      if (list.Count >= HandData.handMax)
      {
        for(int i = 0; i < HandData.handMax; i++)
        {
          int idx = Random.Range(0,list.Count);
          handArray[i] = list[idx];
          trumpList.Remove(list[idx]);
          list.RemoveAt(idx);
          //残り数値更新
          UpdateNumberSum(handArray[i].number);
        }

        break;
      }

      markList.RemoveAt(markIdx);
    }

    return handArray;
  }

  HandChecker.TrumpData[] GetOnePair(int _removeNumber = -1)
  {
    HandChecker.TrumpData[] PairArray = new HandChecker.TrumpData[2];
    List<int> numberList = new List<int>();
    for(int i = 1; i <= 13; i++)
    {
      if(i != _removeNumber)
      numberList.Add(i);
    }

    while(numberList.Count != 0)
    {
      int numberIdx = Random.Range(0, numberList.Count);
      int number = numberList[numberIdx];
      var list = trumpList.Where(data => data.number == number).ToList();

      if(list != null && list.Count >= 2)
      {
        for(int i = 0; i < 2; i++)
        {
          int idx = Random.Range(0, list.Count);
          PairArray[i] = list[idx];
          trumpList.Remove(list[idx]);
          list.RemoveAt(idx);

          //残り数値更新
          UpdateNumberSum(PairArray[i].number);

        }

        break;
      }


      numberList.RemoveAt(numberIdx);
    }

    return PairArray;
  }

  HandChecker.TrumpData[] DrawOnePair()
  {
    HandChecker.TrumpData[] handArray = new HandChecker.TrumpData[HandData.handMax];

    var pair = GetOnePair();

    handArray[0] = pair[0];
    handArray[1] = pair[1];
    handArray[2] = DrawTrump();
    handArray[3] = DrawTrump();
    handArray[4] = DrawTrump();

    return handArray;
  }

  HandChecker.TrumpData[] DrawTwoPair()
  {
    HandChecker.TrumpData[] handArray = new HandChecker.TrumpData[HandData.handMax];

    var pair = GetOnePair();
    var pair2 = GetOnePair(pair[0].number);
    handArray[0] = pair[0];
    handArray[1] = pair[1];
    handArray[2] = pair2[0];
    handArray[3] = pair2[1];
    handArray[4] = DrawTrump();

    return handArray;
  }

  HandChecker.TrumpData[] DrawStraight()
  {
    return GetStraight();
  }

  HandChecker.TrumpData[] GetStraight(List<HandChecker.TrumpData> _list = null)
  {
    List<HandChecker.TrumpData> list = _list;
    if (list == null)
    {
      list = trumpList;
    }

    int min = 1;
    int max = 11;

    int cnt = 0;
    while (cnt < 10)
    {
      int startNumber = Random.Range(min,max);//11を含まない

      int pattern1 = startNumber;
      int pattern2 = startNumber + 1;
      int pattern3 = startNumber + 2;
      int pattern4 = startNumber + 3;
      int pattern5 = startNumber + 4;

      var outdata = list.Where(data =>
      {
        int number = data.number;

        if (number == pattern1)
        {
          pattern1 = -1;
          return true;
        }
        else if (number == pattern2)
        {
          pattern2 = -1;
          return true;
        }
        else if (number == pattern3)
        {
          pattern3 = -1;
          return true;
        }
        else if (number == pattern4)
        {
          pattern4 = -1;
          return true;
        }
        else if (number == pattern5)
        {
          pattern5 = -1;
          return true;
        }

        return false;
      }).ToArray();

      //確定
      if(outdata.Length == 5)
      {
        for(int i = 0; i < 5; i++)
        {
          UpdateNumberSum(outdata[i].number);
          trumpList.Remove(outdata[i]);
        }
        return outdata;
      }

      pattern1 = startNumber;
      pattern2 = startNumber + 1;
      pattern3 = startNumber + 2;
      pattern4 = startNumber + 3;
      pattern5 = startNumber + 4;

      //無い場合 ２枚以上ないパターンは存在しないはず
      for (int i = 0; i < outdata.Length; i++)
      {
        int number = outdata[i].number;
        if (number == pattern1)
        {
          pattern1 = -1;
        }
        else if (number == pattern2)
        {
          pattern2 = -1;
        }
        else if (number == pattern3)
        {
          pattern3 = -1;
        }
        else if (number == pattern4)
        {
          pattern4 = -1;
        }
        else if (number == pattern5)
        {
          pattern5 = -1;
        }
      }

      int maxNum = 1;
      if(pattern1 >= 1)
      {
        if(maxNum < pattern1)
        maxNum = pattern1;
      }

      if (pattern2 >= 1)
      {
        if (maxNum < pattern2)
          maxNum = pattern2;
      }

      if (pattern3 >= 1)
      {
        if (maxNum < pattern3)
          maxNum = pattern3;
      }

      if (pattern4 >= 1)
      {
        if (maxNum < pattern4)
          maxNum = pattern4;
      }

      if (pattern5 >= 1)
      {
        if (maxNum < pattern5)
          maxNum = pattern5;
      }

      if(maxNum >= 7)
      {
        max = maxNum;
      }
      else
      {
        min = maxNum+1;
      }

      cnt++;

    }

    if (_list != null)
    {
      return null;
    }
    else
    {
      return DrawRandomFive();
    }
  }

  HandChecker.TrumpData[] DrawRandomFive()
  {
    HandChecker.TrumpData[] data = new HandChecker.TrumpData[5];

    for(int i = 0; i < 5; i++)
    {
      data[i] = DrawTrump();
    }

    return data;
  }

  HandChecker.TrumpData[] DrawFullHouse()
  {
    var pair = GetOnePair();

    //サーチ範囲
    List<int> rangeList = new List<int>();

    //枚数が残ってるものだけを範囲にいれておく
    for(int i = 0; i < numberSumArray.Length;i++)
    {
      if(numberSumArray[i] >= 3)
      {
        rangeList.Add(i + 1);
      }
    }

    int idx = Random.Range(0, rangeList.Count);

    var data = trumpList.Where(_ => _.number == rangeList[idx]).ToArray();

    HandChecker.TrumpData[] outData = new HandChecker.TrumpData[5];
    
    for(int i = 0; i < 3; i++)
    {
      outData[i] = data[i];
      trumpList.Remove(outData[i]);
      UpdateNumberSum(outData[i].number);
    }

    outData[3] = pair[0];
    outData[4] = pair[1];


    return outData;
  }

  HandChecker.TrumpData[] DrawFourCard()
  {
    //サーチ範囲
    List<int> rangeList = new List<int>();

    //枚数が残ってるものだけを範囲にいれておく
    for (int i = 0; i < numberSumArray.Length; i++)
    {
      if (numberSumArray[i] >= 4)
      {
        rangeList.Add(i + 1);
      }
    }

    int idx = Random.Range(0, rangeList.Count);

    var data = trumpList.Where(_ => _.number == rangeList[idx]).ToArray();
    HandChecker.TrumpData[] outData = new HandChecker.TrumpData[5];

    for (int i = 0; i < 4; i++)
    {
      outData[i] = data[i];
      trumpList.Remove(outData[i]);
      UpdateNumberSum(outData[i].number);
    }

    outData[4] = DrawTrump();

    return outData;
  }

  HandChecker.TrumpData[] DrawThreeCard()
  {
    //サーチ範囲
    List<int> rangeList = new List<int>();

    //枚数が残ってるものだけを範囲にいれておく
    for (int i = 0; i < numberSumArray.Length; i++)
    {
      if (numberSumArray[i] >= 3)
      {
        rangeList.Add(i + 1);
      }
    }

    int idx = Random.Range(0, rangeList.Count);

    var data = trumpList.Where(_ => _.number == rangeList[idx]).ToArray();
    HandChecker.TrumpData[] outData = new HandChecker.TrumpData[5];

    for (int i = 0; i < 3; i++)
    {
      outData[i] = data[i];
      trumpList.Remove(outData[i]);
      UpdateNumberSum(outData[i].number);
    }

    outData[3] = DrawTrump();
    outData[4] = DrawTrump();

    return outData;
  }
  
  HandChecker.TrumpData[] DrawRoyalStraightFlush()
  {
    HandChecker.TrumpData[] outData = new HandChecker.TrumpData[5];

    List<HandChecker.MarkType> markList = new List<HandChecker.MarkType>();
    markList.Add(HandChecker.MarkType.Clover);
    markList.Add(HandChecker.MarkType.Dia);
    markList.Add(HandChecker.MarkType.Heart);
    markList.Add(HandChecker.MarkType.Spade);

    int idx = Random.Range(0, markList.Count);

    outData[0].mark = markList[idx];
    outData[1].mark = markList[idx];
    outData[2].mark = markList[idx];
    outData[3].mark = markList[idx];
    outData[4].mark = markList[idx];

    outData[0].number = 10;
    outData[1].number = 11;
    outData[2].number = 12;
    outData[3].number = 13;
    outData[4].number = 1;

    for (int i = 0; i < outData.Length; i++)
    {
      trumpList.Remove(outData[i]);
      UpdateNumberSum(outData[i].number);
    }

    return outData;
  }

  HandChecker.TrumpData[] DrawStraightFlush()
  {
    List<HandChecker.MarkType> markList = new List<HandChecker.MarkType>();
    markList.Add(HandChecker.MarkType.Clover);
    markList.Add(HandChecker.MarkType.Dia);
    markList.Add(HandChecker.MarkType.Heart);
    markList.Add(HandChecker.MarkType.Spade);

    while(markList.Count != 0)
    {
      int idx = Random.Range(0, markList.Count);

      var markTrumpList = trumpList.Where(data => data.mark == markList[idx]).ToList();

      var fiveAndTen = markTrumpList.Where(data => data.number == 5 || data.number == 10).ToArray();

      if(fiveAndTen.Length >= 2)
      {
        var outData = GetStraight(markTrumpList);

        if (outData != null)
        {
          return outData;
        }
      }

      markList.RemoveAt(idx);
    }

    return DrawRandomFive();
  }






}
