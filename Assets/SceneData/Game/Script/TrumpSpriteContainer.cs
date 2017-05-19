using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpSpriteContainer : MonoBehaviour
{
  [SerializeField]
  Sprite[] heartSpriteArray;
  [SerializeField]
  Sprite[] spadeSpriteArray;
  [SerializeField]
  Sprite[] diaSpriteArray;
  [SerializeField]
  Sprite[] cloverSpriteArray;


  //マークと数値から対応するスプライトを取得する
  public Sprite GetSprite(HandChecker.MarkType _type,int _number)
  {
    int idx = _number - 1;
    switch(_type)
    {
      case HandChecker.MarkType.Heart:
        return heartSpriteArray[idx];
      case HandChecker.MarkType.Spade:
        return spadeSpriteArray[idx];
      case HandChecker.MarkType.Dia:
        return diaSpriteArray[idx];
      case HandChecker.MarkType.Clover:
        return cloverSpriteArray[idx];      
    }
    return null;
  }


}
