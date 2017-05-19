namespace Common
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;

    public class NumberObject : MonoBehaviour
    {
        [SerializeField]
        Image[] numberImages;//数値用イメージ
        [SerializeField]
        Sprite[] numberSprites = new Sprite[10];//数値スプライト

        //数値入れ
        public void SetNumber(int _num)
        {
            for(int i =0; i < numberImages.Length; i++)
            {
                if(numberImages[i] != null)
                {
                    numberImages[i].gameObject.SetActive(false);
                }
            }

            int buf = _num;

            int cnt = 0;
            while(buf > 0)
            {
                int digitNum = buf % 10;
                numberImages[cnt].sprite = numberSprites[digitNum];
                numberImages[cnt].gameObject.SetActive(true);
                buf /= 10;
                cnt++;
            }

            if(cnt == 0)
            {
                numberImages[cnt].sprite = numberSprites[_num];
                numberImages[cnt].gameObject.SetActive(true);
            }

        }

    //数値入れ
    public void SetNumber(long _num,bool _isViewAllDigit = false)
    {
      for (int i = 0; i < numberImages.Length; i++)
      {
        if (numberImages[i] != null)
        {
          numberImages[i].gameObject.SetActive(_isViewAllDigit);
          numberImages[i].sprite = numberSprites[0];
        }
      }

      long buf = _num;

      int cnt = 0;
      while (buf > 0)
      {
        long digitNum = buf % 10;
        numberImages[cnt].sprite = numberSprites[digitNum];
        numberImages[cnt].gameObject.SetActive(true);
        buf /= 10;
        cnt++;
      }

      if (cnt == 0)
      {
        numberImages[cnt].sprite = numberSprites[_num];
        numberImages[cnt].gameObject.SetActive(true);
      }

    }
  }
}