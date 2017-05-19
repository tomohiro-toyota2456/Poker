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

      serializedObject.Update();

      numberTex = (Texture2D)EditorGUILayout.ObjectField("NumberTex", numberTex, typeof(Texture2D));

      if(GUILayout.Button("SetSprite"))
      {
        SetSprite();
      }

      serializedObject.ApplyModifiedProperties();
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

      var array = serializedObject.FindProperty("numberSprites");
      array.ClearArray();

      for (int i = 0; i < sprite.Length; i++)
      {
        array.InsertArrayElementAtIndex(i);
        array.GetArrayElementAtIndex(i).objectReferenceValue = sprite[i];
      }

    }

  }
}