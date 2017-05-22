namespace Commmon
{
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class Twitter : MonoBehaviour
  {
    public void Tweet(string _text)
    {
      string escape = WWW.EscapeURL(_text);
      Application.OpenURL("https://twitter.com/intent/tweet?text=" + escape);
    }
  }
}
