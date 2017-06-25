using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//お決まりのろーど をIdのみの引数でとかのサポートするだけ
 public static class ResourcesLoader
{
  //なんというか無駄だけどとりあえずこれで（対した量ないし)
  public static Sprite LoadItemSprite(string _itemId)
  {
    var tex = Resources.LoadAll<Sprite>("ItemTexture");

    for(int i = 0; i < tex.Length; i++)
    {
      if(tex[i].name == _itemId)
      {
        return tex[i];
      }
    }

    return null;
  }
}
