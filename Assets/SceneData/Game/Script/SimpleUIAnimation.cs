using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;
using UniRx.Triggers;

public class SimpleUIAnimation : MonoBehaviour
{
  [SerializeField]
  RectTransform rectTransform;

  bool isMove = false;
  bool isSclAnim = false;

  public void AnimationScl(Vector3 _stScl,Vector3 _edScl,float _time,Action _endAction,float _delayTime = 0)
  {
    if (isSclAnim)
      return;

    isMove = true;

    float timer = 0;

    this.UpdateAsObservable()
      .TakeWhile(_ => (timer - _delayTime) <= _time)
      .Subscribe(_ =>
      {
        timer += Time.deltaTime;
        float t = (timer-_delayTime) / _time;

        rectTransform.localScale = Vector2.Lerp(_stScl, _edScl, t);
      },
      () =>
      {
        rectTransform.localScale = Vector2.Lerp(_stScl, _edScl, 1);

        isSclAnim = false;

        if (_endAction != null)
          _endAction();

      });
  }

  public void AnimationMove(Vector2 _stPos,Vector2 _edPos,float _time,Action _endAction,float _delayTime = 0)
  {
    if (isMove)
      return;

    isMove = true;

    float timer = 0;

    this.UpdateAsObservable()
      .TakeWhile(_ => (timer-_delayTime) <= _time)
      .Subscribe(_ =>
      {
        timer += Time.deltaTime;
        float t = (timer-_delayTime) / _time;
        rectTransform.anchoredPosition = Vector2.Lerp(_stPos, _edPos, t);
      },
      () =>
      {
        rectTransform.anchoredPosition = Vector2.Lerp(_stPos, _edPos, 1);

        isMove = false;

        if (_endAction != null)
          _endAction();
      });
  }

}
