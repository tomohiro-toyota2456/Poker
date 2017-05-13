namespace Common
{
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  [CreateAssetMenu(fileName ="PagePopupData",menuName ="ScriptableObject/PagePopupData",order = 100)]
  public class PagePopupData : ScriptableObject
  {
    [SerializeField]
    Datas[] dataArray;

    [System.Serializable]
    public struct Datas
    {
      public Sprite sprite;
      public string dist;
    }

    public Datas[] DataArray { get { return dataArray; } }

#if UNITY_EDITOR

    public void ReCreateArray(int _elementNum)
    {
      Datas[] newArray = new Datas[_elementNum];

      int len = 0;

      //データがない場合
      if(dataArray == null)
      {
        dataArray = new Datas[_elementNum];
        return;
      }

      len = _elementNum < dataArray.Length  ? _elementNum : dataArray.Length;

      for(int i = 0; i < len;i++)
      {
        newArray[i].sprite = dataArray[i].sprite;
        newArray[i].dist = dataArray[i].dist;
      }

      dataArray = newArray;
    }

#endif

  }
}
