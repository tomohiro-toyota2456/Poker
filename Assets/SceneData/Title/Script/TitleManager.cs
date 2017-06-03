namespace Title
{
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  using UnityEngine.UI;
  using UniRx;
  using UniRx.Triggers;
  using Common;
  using Common.DataBase;
  public class TitleManager : MonoBehaviour
  {
    [SerializeField]
    Button startButton;
    [SerializeField]
    Button deleteUserDataButton;

    UserDB userDB;
    HaveItemDB haveItemDB;
    HaveSkillDB haveSkillDB;
    // Use this for initialization
    void Start()
    {
      userDB = DataBaseManager.Instance.GetDataBase<UserDB>();
      haveItemDB = DataBaseManager.Instance.GetDataBase<HaveItemDB>();
      haveSkillDB = DataBaseManager.Instance.GetDataBase<HaveSkillDB>();

      startButton.OnClickAsObservable()
        .Take(1)
        .Subscribe(_ =>
        {
          ChangeScene();
        }).AddTo(gameObject);

      deleteUserDataButton.OnClickAsObservable()
        .Take(1)
        .Subscribe(_ =>
        {
          userDB.DeleteUserData();
          Debug.Log("DeleteUserData!!!!");
        }).AddTo(gameObject);

      SceneChanger.Instance.IsInitialize = true;
    }

    void ChangeScene()
    {
      if (userDB.IsExistData())
      {
        userDB.LoadUserData();
        haveSkillDB.Load();
        haveItemDB.LoadData();
        SceneChanger.Instance.ChangeScene("Home");
      }
      else
      {
        userDB.LoadUserData();
        haveSkillDB.Load();
        haveItemDB.LoadData();
        //ほんとは導入シーンへ
        SceneChanger.Instance.ChangeScene("Home");
      }
    }

  }
}
