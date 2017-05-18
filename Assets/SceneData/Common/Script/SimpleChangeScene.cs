namespace Common
{
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class SimpleChangeScene : MonoBehaviour
  {
    [SerializeField]
    string changeSceneName;
    
    public void ChangeScene()
    {
      if(!string.IsNullOrEmpty(changeSceneName))
      SceneChanger.Instance.ChangeScene(changeSceneName);
    }

  }
}