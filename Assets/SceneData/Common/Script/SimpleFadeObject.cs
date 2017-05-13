namespace Common
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using System;

    public class SimpleFadeObject : FadeObject
    {
        [SerializeField]
        Image fadeObject;
        [SerializeField]
        Color color;
        [SerializeField]
        float fadeTime;

        float moveValue;
        // Use this for initialization
        void Start()
        {
            fadeObject.gameObject.SetActive(false);
            moveValue = 1.0f / fadeTime;
        }

        public override IEnumerator StartFadeOut()
        {
            isFade = true;
            fadeObject.color = color;

            //フェードオブジェクトtrue
            fadeObject.gameObject.SetActive(true);

            float a = 0.0f;
            while (a < 1.0f)
            {
                fadeObject.color = new Color(color.r, color.g, color.b, a);
                a += moveValue * Time.deltaTime;
                yield return null;
            }

        }

        public override IEnumerator StartFadeIn()
        {
            float a = 1.0f;
            while (a >= 0.0f)
            {
                fadeObject.color = new Color(color.r, color.g, color.b, a);
                a -= moveValue * Time.deltaTime;
                yield return null;
            }

            isFade = false;
            fadeObject.gameObject.SetActive(false);
        }


    }

}