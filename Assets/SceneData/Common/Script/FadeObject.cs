namespace Common
{
    using UnityEngine;
    using System.Collections;

    public abstract class FadeObject : MonoBehaviour
    {
        protected bool isFade = false;//Fade全体のフラグ
        public bool IsFade
        {
            get { return isFade; }
        }

        //シーンきりかえ　シーンをロード画面等にする
        public abstract IEnumerator StartFadeOut();

        //シーン切り替え シーンをゲームシーンにしていく
        public abstract IEnumerator StartFadeIn();

    }
}
