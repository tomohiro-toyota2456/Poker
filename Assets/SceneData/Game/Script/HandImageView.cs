using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HandImageView : MonoBehaviour
{
  [SerializeField]
  Image handImage;
  [SerializeField]
  SimpleUIAnimation target;
  [SerializeField]
  Sprite[] handSpriteArray;

  //表示・非表示
  public void SetActive(bool _flag)
  {
    handImage.gameObject.SetActive(_flag);
  }

  public void SetSprite(int _idx)
  {
    handImage.sprite = handSpriteArray[_idx];
  }
  
  //登場アニメーション
  public void InAnimationScl(float _time,Action _endAction)
  {
    Action act = () =>
     {
       target.AnimationScl(new Vector3(1.2f, 1.2f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), 0.3f, _endAction);
     };

    target.AnimationScl(Vector3.zero, new Vector3(1.2f, 1.2f, 1.0f), _time, act);
  }

  //退場アニメーション
  public void OutAnimationScl(float _time, Action _endAction)
  {
    target.AnimationScl(Vector3.one,Vector3.zero, _time,_endAction);
  }

}
