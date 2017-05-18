using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;

//トランプ画像を移動させるスクリプト
//また選択状態も対応
public class TrumpObj : MonoBehaviour
{
  [SerializeField]
  Button button;
  [SerializeField]
  Vector2 stPos;//配布される位置
  [SerializeField]
  Vector2 targetPos;//配置される位置
  [SerializeField]
  float selectOffsetY;//選択時にどれだけ上に上がるか

  bool isMove;
  public bool IsMove { get { return isMove; } }

  bool isSelect = false;
  public bool IsSelect { get { return isSelect; } }

  RectTransform rectTransform;
	// Use this for initialization
	void Start ()
  {
    rectTransform = GetComponent<RectTransform>();

    //クリックで選択・非選択状態
    //選択時は少し上にオブジェクトがずれる処理
    button.OnClickAsObservable()
      .Where(_ => !isMove)
      .Subscribe(_ =>
      {
        isSelect = !isSelect;

        var pos = targetPos;
        if(isSelect)
        {
          pos.y += selectOffsetY;
        }

        rectTransform.anchoredPosition = pos;

      }).AddTo(gameObject);

	}

  //移動用
  public void Move(bool isDistribute,float _time,Action _endAction)
  {
    button.gameObject.SetActive(true);
    Vector2 sPos;
    Vector2 tPos;

    isMove = true;

    //配りモードの場合は手元にくるように、それ以外は逆の動きを
    if (isDistribute)
    {
      sPos = stPos;
      tPos = targetPos;
    }
    else
    {
      sPos = targetPos;
      tPos = stPos;
    }

    float timer = 0;

    this.UpdateAsObservable()
      .TakeWhile(_ => timer <= _time)
      .Subscribe(_ =>
      {
        timer += Time.deltaTime;
        float t = timer / _time;

        rectTransform.anchoredPosition = Vector2.Lerp(sPos, tPos, t);
      },
      () =>
      {
        rectTransform.anchoredPosition = Vector2.Lerp(sPos, tPos, 1);

        if (_endAction != null)
          _endAction();

        if(!isDistribute)
        {
          button.gameObject.SetActive(false);
        }

        isMove = false;
      });

  }

  public void SetSprite(Sprite _sprite)
  {
    button.image.sprite = _sprite;
  }

  public void SetSelect(bool _isSelect)
  {
    isSelect = _isSelect;

    var pos = targetPos;
    if (isSelect)
    {
      pos.y += selectOffsetY;
    }

    rectTransform.anchoredPosition = pos;
  }


	

}
