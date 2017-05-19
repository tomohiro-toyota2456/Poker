using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(TrumpSpriteContainer))]
public class TrumpSpriteContainerInspector : Editor
{
  string heartPath = "";
  string diaPath = "";
  string cloverPath = "";
  string spadePath = "";

  Texture2D heartTex = null;
  Texture2D diaTex = null;
  Texture2D cloverTex = null;
  Texture2D spadeTex = null;

  public override void OnInspectorGUI()
  {
    base.OnInspectorGUI();

    serializedObject.Update();

    heartTex = (Texture2D)EditorGUILayout.ObjectField("HeartTex", heartTex, typeof(Texture2D));
    spadeTex = (Texture2D)EditorGUILayout.ObjectField("SpadeTex", spadeTex, typeof(Texture2D));
    diaTex = (Texture2D)EditorGUILayout.ObjectField("DiaTex", diaTex, typeof(Texture2D));
    cloverTex = (Texture2D)EditorGUILayout.ObjectField("CloverTex", cloverTex, typeof(Texture2D));

    if(GUILayout.Button("SetSprite"))
    {
      SetSprite();
    }

    serializedObject.ApplyModifiedProperties();
    

  }

  void SetSprite()
  {
    if (heartTex != null)
    {
      heartPath = AssetDatabase.GetAssetPath(heartTex);

      int idx = heartPath.IndexOf("Resources");
      heartPath = heartPath.Substring(idx + 10);

      string fileNameExtension = System.IO.Path.GetFileName(heartPath);
      string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(heartPath);

      heartPath = heartPath.Replace(fileNameExtension, fileNameWithoutExtension);

      Sprite[] sprite = Resources.LoadAll<Sprite>(heartPath);

      var array = serializedObject.FindProperty("heartSpriteArray");
      array.ClearArray();

      for (int i = 0; i < sprite.Length; i++)
      {
        array.InsertArrayElementAtIndex(i);
        array.GetArrayElementAtIndex(i).objectReferenceValue = sprite[i];
      }

    }

    if (spadeTex != null)
    {
      spadePath = AssetDatabase.GetAssetPath(spadeTex);

      int idx = spadePath.IndexOf("Resources");
      spadePath = spadePath.Substring(idx + 10);

      string fileNameExtension = System.IO.Path.GetFileName(spadePath);
      string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(spadePath);

      spadePath = spadePath.Replace(fileNameExtension, fileNameWithoutExtension);

      Sprite[] sprite = Resources.LoadAll<Sprite>(spadePath);

      var array = serializedObject.FindProperty("spadeSpriteArray");
      array.ClearArray();

      for (int i = 0; i < sprite.Length; i++)
      {
        array.InsertArrayElementAtIndex(i);
        array.GetArrayElementAtIndex(i).objectReferenceValue = sprite[i];
      }
    }

    if (diaTex != null)
    {
      diaPath = AssetDatabase.GetAssetPath(diaTex);

      int idx = diaPath.IndexOf("Resources");
      diaPath = diaPath.Substring(idx + 10);

      string fileNameExtension = System.IO.Path.GetFileName(diaPath);
      string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(diaPath);

      diaPath = diaPath.Replace(fileNameExtension, fileNameWithoutExtension);

      Sprite[] sprite = Resources.LoadAll<Sprite>(diaPath);

      var array = serializedObject.FindProperty("diaSpriteArray");
      array.ClearArray();

      for (int i = 0; i < sprite.Length; i++)
      {
        array.InsertArrayElementAtIndex(i);
        array.GetArrayElementAtIndex(i).objectReferenceValue = sprite[i];
      }
    }

    if (cloverTex != null)
    {
      cloverPath = AssetDatabase.GetAssetPath(cloverTex);

      int idx = cloverPath.IndexOf("Resources");
      cloverPath = cloverPath.Substring(idx + 10);

      string fileNameExtension = System.IO.Path.GetFileName(cloverPath);
      string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(cloverPath);

      cloverPath = cloverPath.Replace(fileNameExtension, fileNameWithoutExtension);

      Sprite[] sprite = Resources.LoadAll<Sprite>(cloverPath);

      var array = serializedObject.FindProperty("cloverSpriteArray");
      array.ClearArray();

      for (int i = 0; i < sprite.Length; i++)
      {
        array.InsertArrayElementAtIndex(i);
        array.GetArrayElementAtIndex(i).objectReferenceValue = sprite[i];
      }
    }

  }
}
