namespace Common
{
    //************************************************
    //SingletonUnityObject
    //Author HaradaYuto
    //************************************************
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    //************************************************
    //SingletonUnityObject.cs
    //Author HaradaYuto
    //************************************************
    public class SingletonUnityObject<T> : MonoBehaviour where T : class
    {
        static T obj = null;

        public static T Instance
        {
            get
            {
                if (obj == null)
                {
                    obj = FindObjectOfType(typeof(T)) as T;
                }

                return obj;
            }
        }
    }
}
