using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ContinueMagMaster",menuName ="ScriptableObject/MagMaster",order =200)]
public class ContinueMagMasterData : ScriptableObject
{
  [SerializeField]
  float[] magArray;

  public float GetContinueMag(int _continueCnt)
  {
    int idx = _continueCnt <= magArray.Length ? _continueCnt : magArray.Length;
    idx--;

    return magArray[idx];
  }

}