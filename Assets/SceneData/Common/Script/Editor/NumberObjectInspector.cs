namespace Common
{
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  using UnityEditor;

  [CustomEditor(typeof(NumberObject))]
  public class NumberObjectInspector : Editor
  {
    Texture2D numberTex = null;

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      numberTex = (Texture2D)EditorGUILayout.ObjectField("NumberTex", numberTex, typeof(Texture2D));

      if(GUILayout.Button("SetSprite"))
      {
        SetSprite();
      }
    }

    void SetSprite()
    {
      if (numberTex == null)
        return;

      string numberPath = AssetDatabase.GetAssetPath(numberTex);

      int idx = numberPath.IndexOf("Resources");
      numberPath = numberPath.Substring(idx + 10);

      string fileNameExtension = System.IO.Path.GetFileName(numberPath);
      string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(numberPath);

      numberPath = numberPath.Replace(fileNameExtension, fileNameWithoutExtension);

      Sprite[] sprite = Resources.LoadAll<Sprite>(numberPath);

      var numberObject = (NumberObject)target;
      numberObject.SetSpriteArray(sprite);
    }

  }
}