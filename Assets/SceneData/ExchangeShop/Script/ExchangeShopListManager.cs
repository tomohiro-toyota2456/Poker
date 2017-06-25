using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExchangeShopListManager : MonoBehaviour
{
  [SerializeField]
  GameObject root;
  [SerializeField]
  ExchangeShopListItem item;

  public void AddList(string _id,string _name,int _haveNum,Action<ExchangeShopListItem> _sellAction)
  {
    var item = Create();

    item.Init(_id, _name, _haveNum, _sellAction);
  }

  public ExchangeShopListItem Create()
  {
    var item = Instantiate(this.item);
    item.transform.SetParent(root.transform);
    item.transform.localPosition = Vector3.zero;
    item.transform.localScale = Vector3.one;

    return item;
  }
}
