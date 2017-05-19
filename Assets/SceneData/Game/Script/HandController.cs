using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HandController : MonoBehaviour
{
  [SerializeField]
  TrumpObj[] trumpObj = new TrumpObj[HandData.handMax];
  [SerializeField]
  TrumpSpriteContainer container;

  HandData handData = new HandData();
	// Use this for initialization
	void Start ()
  {
    if(handData == null)
    {
      handData = new HandData();
    }
	}

  //ハンドデータの入力（対応するカードのスプライトも変更
  public void SetHandData(int _idx,HandChecker.TrumpData _handData)
  {
    handData.SetData(_idx, _handData);
    trumpObj[_idx].SetSprite(container.GetSprite(_handData.mark, _handData.number));
  }

  public HandChecker.TrumpData[] GetHandData()
  {
    return handData.GetData();
  }

  //移動アニメーション
  public void Move(bool _isDistribute,int _idx,float _time,Action _endAction)
  {
    trumpObj[_idx].Move(_isDistribute, _time, _endAction);
  }

  //トランプが動いているかどうか
  public bool IsMove()
  {
    for(int i = 0; i < trumpObj.Length; i++)
    {
      if(trumpObj[i].IsMove)
      {
        return true;
      }
    }

    return false;
  }

  //選択されているトランプのidxを取得
  //尚Handのidxと一致する
  public int[] GetSelectTrumpIdxArray()
  {
    List<int> list = new List<int>();

    for(int  i = 0; i < trumpObj.Length;i++)
    {
      if(trumpObj[i].IsSelect)
      {
        list.Add(i);
      }
    }

    return list.ToArray();
  }

  public void SetSelect(int _idx,bool _flag)
  {
    trumpObj[_idx].SetSelect(_flag);
  }

  public void SetPosition(int _idx,Vector2 _pos)
  {
    trumpObj[_idx].SetPosition(_pos);
  }

}
