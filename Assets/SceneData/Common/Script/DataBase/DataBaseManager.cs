namespace Common.DataBase
{
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class DataBaseManager : SingletonUnityObject<DataBaseManager>
  {
    [SerializeField]
    DataBase[] dataBaseArray;

    // Use this for initialization
    void Start()
    {
      foreach(var db in dataBaseArray)
      {
        db.Init();
      }
    }

    public T GetDataBase<T>() where T :class
    {
      foreach( var db in dataBaseArray)
      {
        T ret = db as T;

        if(ret != null)
        {
          return ret;
        }
      }

      return null;
    }
  }
}
