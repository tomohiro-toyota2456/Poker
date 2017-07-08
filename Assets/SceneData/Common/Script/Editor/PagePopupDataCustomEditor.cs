namespace Common
{
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  using UnityEditor;

  [CustomEditor(typeof(PagePopupData))]
  public class PagePopupDataCustomEditor : Editor
  {
    Vector2 scrollPos;
    int dataSize;
    public override void OnInspectorGUI()
    {
      serializedObject.Update();
      var array = base.serializedObject.FindProperty("dataArray");
      using (new EditorGUILayout.HorizontalScope())
      {
        dataSize = array.arraySize;

        EditorGUILayout.LabelField("ページ数");
        dataSize = EditorGUILayout.IntField(dataSize);

        //データ数が変わった場合
        if(array.arraySize != dataSize)
        {
          var buf = (PagePopupData)target;
          buf.ReCreateArray(dataSize);
        }
      }

      int len = array.arraySize;

      GUILayoutOption[] options = new GUILayoutOption[2];

      options[0] = GUILayout.Width(300);
      options[1] = GUILayout.Height(200);

      using (new EditorGUILayout.VerticalScope())
      {
        using (var scroll = new EditorGUILayout.ScrollViewScope(scrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
        {
          scrollPos = scroll.scrollPosition;
          for (int i = 0; i < len; i++)
          {
            var elem = array.GetArrayElementAtIndex(i);
            var sprite = elem.FindPropertyRelative("sprite");
            var dist = elem.FindPropertyRelative("dist");

            EditorGUILayout.LabelField("Page" + (i + 1));
            EditorGUILayout.PropertyField(sprite);
            using (new EditorGUILayout.HorizontalScope())
            {
              EditorGUILayout.LabelField("文章");
              dist.stringValue = EditorGUILayout.TextArea(dist.stringValue,options);
            }
          }
        }
      }

      serializedObject.ApplyModifiedProperties();

    }
  }
}