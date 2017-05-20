using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ScriptableObject
{
  [SerializeField]
  string itemId;
  [SerializeField]
  string itemName;
  [SerializeField]
  string dist;

  public string ItemId { get { return itemId; }set { itemId = value; } }
  public string ItemName { get { return itemName; } set { itemName = value; } }
  public string Dist { get { return dist; } set { dist = value; } }
}
