using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

//スキル時にカットインを出す機構
public class SkillCutIn : MonoBehaviour
{
  [SerializeField]
  TextMeshProUGUI skillNameText;
  [SerializeField]
  TextMeshProUGUI skillDistText;
  [SerializeField]
  Image charaImage;
  [SerializeField]
  SimpleUIAnimation skillNameAnim;
  [SerializeField]
  SimpleUIAnimation skillDistAnim;
  [SerializeField]
  SimpleUIAnimation charaImageAnim;
  [SerializeField]
  Vector2 charaStPos;
  [SerializeField]
  Vector2 charaEdPos;


  public void Init(string _skillName,string _skillDist,Sprite _charaSprite)
  { 
    skillNameText.text = _skillName;
    skillDistText.text = _skillDist;
    charaImage.sprite = _charaSprite;
  }

  public void StartInAnimation(float _time,Action _endAction)
  {
    float time = _time / 2;

    charaImageAnim.AnimationMove(charaStPos, charaEdPos, time, () =>
       {
         skillNameAnim.AnimationScl(Vector3.zero, Vector3.one, time, null);
         skillDistAnim.AnimationScl(Vector3.zero, Vector3.one, time, _endAction);
       });
  }

  public void StartOutAnimation(float _delayTime,float _time,Action _endAction)
  {
    float time = _time;

    charaImageAnim.AnimationMove(charaEdPos, charaStPos, time,null,_delayTime);
    skillNameAnim.AnimationScl(Vector3.one, Vector3.zero, time, null, _delayTime);
    skillDistAnim.AnimationScl(Vector3.one, Vector3.zero, time, _endAction, _delayTime);
  }


}
