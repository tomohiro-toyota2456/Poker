namespace Common
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class SceneChanger : SingletonUnityObject<SceneChanger>
    {
        [SerializeField]
        GameObject touchCollision;
        [SerializeField]
        FadeObject[] fadeObject;
        bool isLoading = false;//
        bool isInitialize = false;

        string nowScene;//今シーン

        public bool IsInitialize { get {return isInitialize; } set { isInitialize = value; } }
        public bool IsLoading { get { return isLoading; } set { isLoading = value; } }

        

        // Use this for initialization
        void Start()
        {
        }


        //ChangeScene
        //引数：移動するシーン
        //遷移先でIsInitializeをtrueにしなければならない
        public void ChangeScene(string _sceneName,int _fadeObjectNum = 0)
        {
            StartCoroutine(_ChangeScene(_sceneName,_fadeObjectNum));
        }

        IEnumerator _ChangeScene(string _sceneName,int _fadeObjectNum)
        {
            //ロード中は弾く
            if (isLoading)
            {
                yield break;
            }

            isLoading = true;
            isInitialize = false;
            touchCollision.SetActive(true);

            yield return fadeObject[_fadeObjectNum].StartFadeOut();

            //前シーン削除
            if (!string.IsNullOrEmpty(nowScene))
            {
			          	var uop = SceneManager.UnloadSceneAsync (nowScene);
              while(!uop.isDone)
              {
                yield return null;
              }
            }

            //非同期読み込み
            var op = SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);

            while(!op.isDone || !isInitialize)
            {
                yield return null;      
            }

            nowScene = _sceneName;

            yield return fadeObject[_fadeObjectNum].StartFadeIn();

            touchCollision.SetActive(false);
            isLoading = false;
        }
    }
}
