using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class TrumpDistributeManager
{
  List<HandChecker.TrumpData> trumpList = new List<HandChecker.TrumpData>();

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
  }

  public HandChecker.TrumpData DrawTrump()
  {
    int idx = Random.Range(0, trumpList.Count);

    HandChecker.TrumpData data = trumpList[idx];
    trumpList.RemoveAt(idx);
    return data;
  }

  public HandChecker.TrumpData[] DrawTrump(SkillData _skillData, HandChecker.TrumpData[] _handDataArray,int[] _changeIdxArray)
  {
    if(_skillData.Type != SkillData.SkillType.Draw)
    {
      return null;
    }

    if (_changeIdxArray.Length == 0)
    {
      return _handDataArray;
    }

    switch(_skillData.Detail)
    {
      case SkillData.SkillDetail.FixedNumber:

        _handDataArray[_changeIdxArray[0]] = DrawFixedNumber(_skillData.Number);
        return _handDataArray;

      case SkillData.SkillDetail.FiexedMark:
        _handDataArray[_changeIdxArray[0]] = DrawFixedMark(_skillData.MarkType);
        return _handDataArray;

      case SkillData.SkillDetail.AllChangeOnePair:
        return DrawOnePair();

      case SkillData.SkillDetail.AllChangeTwoPair:
        return DrawTwoPair();
      case SkillData.SkillDetail.AllChangeFlush:
        return DrawFlush();
    }

    return null;
  }

  //固定数値を引きます。しかしない場合はランダム引きになります。
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



}
