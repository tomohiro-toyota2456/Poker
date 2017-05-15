using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandData
{
  static public readonly int handMax = 5;
  HandChecker.TrumpData[] handDataArray = new HandChecker.TrumpData[handMax];

  public void SetData(int _idx,HandChecker.TrumpData _handData)
  {
    handDataArray[_idx] = _handData;
  }

  public HandChecker.TrumpData[] GetData()
  {
    return handDataArray;
  }

  public HandChecker.TrumpData GetData(int _idx)
  {
    return handDataArray[_idx];
  }

}
