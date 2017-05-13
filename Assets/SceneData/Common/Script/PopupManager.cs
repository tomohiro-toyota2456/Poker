namespace Common
{
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  using System;

  public class PopupManager : SingletonUnityObject<PopupManager>
  {
    [SerializeField]
    GameObject root;
    [SerializeField]
    GameObject cover;

    public T Create<T>(T _obj) where T : MonoBehaviour
    {
      T obj = null;

      obj = Instantiate(_obj);
      obj.gameObject.transform.SetParent(root.transform);
      obj.gameObject.transform.localScale = new Vector3(0, 0, 0);
      obj.gameObject.transform.localPosition = new Vector3(0, 0, 0);

      return obj;
    }

    public GameObject Create(GameObject _obj)
    {
      GameObject obj = Instantiate(_obj);
      obj.transform.SetParent(root.transform);
      obj.transform.localScale = new Vector3(1, 1, 1);
      obj.transform.localPosition = new Vector3(0, 0, 0);

      return obj;
    }

    public void OpenPopup(PopupBase _ppBase,Action _endAction)
    {
      cover.SetActive(true);
      Action coverDisable = () => { cover.SetActive(false); };
      _ppBase.AddCloseEndAction(coverDisable);
      _ppBase.Open(_endAction);
    }

  }

}