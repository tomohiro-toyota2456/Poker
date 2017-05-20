using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterItemData : ScriptableObject
{
  [SerializeField]
  ItemData[] itemDataArray;

  public ItemData[] ItemDataArray { get { return itemDataArray; }set { itemDataArray = value; } }
}
