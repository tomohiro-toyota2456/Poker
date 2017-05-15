namespace Title
{
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  using UnityEngine.UI;
  using UniRx;
  using UniRx.Triggers;
  using Common;

  public class TitleManager : MonoBehaviour
  {
    [SerializeField]
    Button startButton;

    // Use this for initialization
    void Start()
    {
      startButton.OnClickAsObservable()
        .Take(1)
        .Subscribe(_ =>
        {
          SceneChanger.Instance.ChangeScene("Game");
        }).AddTo(gameObject);

      SceneChanger.Instance.IsInitialize = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
  }
}
