namespace Common
{
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  using UniRx;
  using UnityEngine.UI;
  using TMPro;

  public class PagePopup : PopupBase
  {
    [SerializeField]
    Button nextButton;
    [SerializeField]
    Button closeButton;
    [SerializeField]
    TextMeshProUGUI pageText;
    [SerializeField]
    TextMeshProUGUI dist;
    [SerializeField]
    Image image;

    PagePopupData data;
    int curPage = 0;
    int maxPage = 0;

    // Use this for initialization
    public override void Start()
    {
      base.Start();

      pageText.gameObject.SetActive(true);
      closeButton.gameObject.SetActive(false);

      //閉じるボタン
      closeButton.OnClickAsObservable()
        .Take(1)
        .Subscribe(_ =>
        {
          Close();
        }).AddTo(gameObject);

      //次へボタン
      nextButton.OnClickAsObservable()
        .Subscribe(_ =>
        {
          curPage++;

          if(curPage >= maxPage )
          {
            closeButton.gameObject.SetActive(true);
            pageText.gameObject.SetActive(false);
            curPage = maxPage;
            return;
          }
          else
          {
            UpdateData(curPage);
          }
        }).AddTo(gameObject);
    }

    public void SetData(PagePopupData _data)
    {
      data = _data;
      curPage = 0;
      maxPage = data.DataArray.Length;
      UpdateData(curPage);
    }

    void UpdateData(int _idx)
    {
      image.sprite = data.DataArray[_idx].sprite;
      dist.text = data.DataArray[_idx].dist;

      pageText.text = (_idx+1).ToString() + "/" + maxPage.ToString();
    }

  }
}
