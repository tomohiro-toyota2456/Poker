namespace Common
{
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  using System;
  using UniRx;
  using UniRx.Triggers;

  //ポップアップの規定クラス
  public class PopupBase : MonoBehaviour
  {

    //クローズ時の呼び出す関数リスト
    List<Action> closeEndActionList = new List<Action>();

    virtual public void Start()
    {
      //終了時にオブジェクト破棄
      Action act = () =>
      {
        GameObject.Destroy(this.gameObject);
      };

      closeEndActionList.Add(act);
    }

    //Open
    //裏のタッチ防止を出すには継承先で作るか　PopupManagerのOpen関数から呼び出す
    public virtual void Open(Action _openEndAction = null)
    {
      AnimationScl(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(1, 1, 1),0.25f, _openEndAction);
    }

    //Close
    public virtual void Close()
    { 
      AnimationScl(new Vector3(1,1, 1), new Vector3(0,0, 0), 0.25f, closeEndActionList);
    }

    //アニメーション用
    protected void AnimationScl(Vector3 _st,Vector3 _ed,float _time,List<Action> _endAnimationActionList = null)
    {
      float t = 0;

      this.UpdateAsObservable()
        .TakeWhile(_ => t < _time)
        .Subscribe
        (
        _ =>
        {
          t += Time.deltaTime;

          float comp = t / _time;

          gameObject.transform.localScale = Vector3.Slerp(_st, _ed, comp);
        },
        () =>
        {
          if(_endAnimationActionList != null)
          {
            for (int i = 0; i < _endAnimationActionList.Count; i++)
            {
              _endAnimationActionList[i]();
            }
          }
        }
        ).AddTo(gameObject);
    }

    protected void AnimationScl(Vector3 _st, Vector3 _ed, float _time, Action _endAnimationAction = null)
    {
      float t = 0;

      this.UpdateAsObservable()
        .TakeWhile(_ => t < _time)
        .Subscribe
        (
        _ =>
        {
          t += Time.deltaTime;

          float comp = t / _time;

          gameObject.transform.localScale = Vector3.Slerp(_st, _ed, comp);
        },
        () =>
        {
          if (_endAnimationAction != null)
          {
            _endAnimationAction();
          }
        }
        ).AddTo(gameObject);
    }

    //終了時アクション追加
    public void AddCloseEndAction(Action _action)
    {
      closeEndActionList.Add(_action);
    }

  }
}