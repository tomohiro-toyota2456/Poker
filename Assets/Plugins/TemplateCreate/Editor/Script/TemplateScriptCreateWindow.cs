using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Text;
using System.Globalization;
using System.IO;

public class TemplateScriptCreateWindow : EditorWindow
{
  string outPath = "Assets/";
  TemplateScriptConfig config;

  SerializedObject serializedObject;
  ReorderableList usingList;
  ReorderableList enumList;
  ReorderableList variableDataList;

  [MenuItem("HYTools/OpenWindow/CreateTemplateScript")]
  public static void Open()
  {
    GetWindow<TemplateScriptCreateWindow>();
  }

  public void OnGUI()
  {
    if(config == null)
    {
      config = Resources.Load<TemplateScriptConfig>("ConfigData/TemplateScriptConfig");
      serializedObject = new SerializedObject(config);
    }

    if(serializedObject == null)
    {
      config = Resources.Load<TemplateScriptConfig>("ConfigData/TemplateScriptConfig");
      serializedObject = new SerializedObject(config);
    }

    if (serializedObject != null)
      serializedObject.Update();

    using (new EditorGUILayout.VerticalScope())
    {
      EditorGUILayout.LabelField("保存パス(ファイル名はいらない)");
      outPath = EditorGUILayout.TextField(outPath);

      EditorGUILayout.LabelField("クラス名");
      config.ClassName = EditorGUILayout.TextField(config.ClassName);

      EditorGUILayout.LabelField("派生クラス名");
      config.DerivationName = EditorGUILayout.TextField(config.DerivationName);

      EditorGUILayout.LabelField("NameSpace(使わない場合は空)");
      config.NameSpaceName = EditorGUILayout.TextField(config.NameSpaceName);

      EditorGUILayout.LabelField("改行スペース数");
      config.SpaceNum = EditorGUILayout.IntField(config.SpaceNum);

      EditorGUILayout.LabelField("Usingリスト");
      CreateUsingList();

      EditorGUILayout.LabelField("作成Enumリスト(書き方:Enum名,要素1,要素2,....要素n)");
      CreateEnumList();

      EditorGUILayout.LabelField("作成変数リスト(プロパティはアッパーキャメルで変数名と同じ名前になります。");
      CreateVariableDataList();

      if(serializedObject!=null)
      serializedObject.ApplyModifiedProperties();

      if (GUILayout.Button("CreateScript"))
      {
        CreateScript();
      }
    }
  }

  void CreateUsingList()
  {
    if(usingList == null)
    {
      usingList = new ReorderableList(serializedObject, serializedObject.FindProperty("usingList"));
      usingList.drawHeaderCallback = (rect) =>
      {
        GUI.Label(rect, "UsingList");
      };

      usingList.drawElementCallback = (rect, idx, isActive, isForcus) =>
      {
        var list = serializedObject.FindProperty("usingList");
        var element = list.GetArrayElementAtIndex(idx);
        element.stringValue = EditorGUI.TextField(rect,element.stringValue);
      };
    }
    usingList.DoLayoutList();
  }

  void CreateEnumList()
  {
    if (enumList == null)
    {
      enumList = new ReorderableList(serializedObject, serializedObject.FindProperty("enumList"));
      enumList.drawHeaderCallback = (rect) =>
      {
        GUI.Label(rect, "enumList");
      };

      enumList.drawElementCallback = (rect, idx, isActive, isForcus) =>
      {
        var list = serializedObject.FindProperty("enumList");
        var element = list.GetArrayElementAtIndex(idx);
        element.stringValue = EditorGUI.TextField(rect, element.stringValue);
      };
    }
    enumList.DoLayoutList();
  }

  void CreateVariableDataList()
  {
    if (variableDataList == null)
    {
      variableDataList = new ReorderableList(serializedObject, serializedObject.FindProperty("variableDataList"));
      variableDataList.drawHeaderCallback = (rect) =>
      {
        GUI.Label(rect, "variableDataList");
      };

      variableDataList.drawElementCallback = (rect, idx, isActive, isForcus) =>
      {
        Rect localRect = rect;

        float width = localRect.width / 5.0f;
        var list = serializedObject.FindProperty("variableDataList");
        var element = list.GetArrayElementAtIndex(idx);

        var enumType = element.FindPropertyRelative("moldType");
        var MoldName = element.FindPropertyRelative("moldName");
        var name = element.FindPropertyRelative("variableName");
        var isArray = element.FindPropertyRelative("isArray");

        localRect.width = width /2;
        GUI.Label(localRect, "型");
        localRect.x += localRect.width + 2f;
        enumType.enumValueIndex = (int)((TemplateScriptConfig.MoldType)EditorGUI.EnumPopup(localRect, (TemplateScriptConfig.MoldType)enumType.enumValueIndex));
        localRect.x += localRect.width + 2f;

        if (enumType.enumValueIndex == TemplateScriptConfig.MoldType.Enum.GetHashCode())
        {
          localRect.width = width / 2;
          GUI.Label(localRect, "Enum名");
          localRect.x += localRect.width + 2f;
          localRect.width = width;
          MoldName.stringValue = EditorGUI.TextField(localRect, MoldName.stringValue);
          localRect.x += localRect.width + 2f;
        }

        localRect.width = width / 2;
        GUI.Label(localRect, "変数名");
        localRect.x += localRect.width + 2f;
        localRect.width = width;
        name.stringValue = GUI.TextField(localRect,name.stringValue);

        localRect.x += localRect.width + 2f;
        localRect.width = width / 2;
        GUI.Label(localRect, "配列フラグ");

        localRect.x += localRect.width + 2f;
        localRect.width = width;
        isArray.boolValue = EditorGUI.Toggle(localRect, isArray.boolValue);
      };
    }
    variableDataList.DoLayoutList();
  }

  void CreateScript()
  {
    StringBuilder baseBuilder = new StringBuilder();

    //using
    foreach(var item in config.UsingList)
    {
      baseBuilder.Append("using " + item + ";");
      baseBuilder.AppendLine();
    }

    //usingがあれば1行開けておく
    if(config.UsingList.Count != 0)
    {
      baseBuilder.AppendLine();
    }

    int deep = 0;

    //namespace
    if (!string.IsNullOrEmpty(config.NameSpaceName))
    {
      baseBuilder.Append("namespace " + config.NameSpaceName);
      baseBuilder.AppendLine();
      baseBuilder.Append("{");
      deep++;
    }


    //ClassName
    string space = GetSpace(deep);

    baseBuilder.AppendLine();
    baseBuilder.Append(space + "public class "+config.ClassName);
    
    if(!string.IsNullOrEmpty(config.DerivationName))
    {
      baseBuilder.Append(" : " + config.DerivationName);
    }

    baseBuilder.AppendLine();
    baseBuilder.Append(space + "{");

    int classDeep = deep;//クラス設定時の深さを保存

    deep++;

    space = GetSpace(deep);

    //Variable
    StringBuilder variableBuilder = new StringBuilder();
    StringBuilder propertyBuilder = new StringBuilder();
    foreach(var item in config.VariableDataList)
    {
      string moldName;
      switch(item.moldType)
      {
        case TemplateScriptConfig.MoldType.Enum:

          moldName = item.moldName;

          break;

        default:

          if (item.moldType.GetHashCode() < 20)
          {
            TextInfo info = CultureInfo.CurrentCulture.TextInfo;
            moldName = info.ToLower(item.moldType.ToString());
          }
          else
          {
            moldName = item.moldType.ToString();
          }

          break;
      }

      string name = item.variableName;
      bool isArray = item.isArray;

      string arrayStr = "";

      if(isArray)
      {
        arrayStr = "[]";
      }

      variableBuilder.Append(space + "[SerializeField]");
      variableBuilder.AppendLine();
      variableBuilder.Append(space + moldName + arrayStr + " " + name + ";");
      variableBuilder.AppendLine();

      TextInfo info2 = CultureInfo.CurrentCulture.TextInfo;
      string propertyName = info2.ToTitleCase(name);
      propertyBuilder.Append(space + "public "+moldName + arrayStr + " " + propertyName + "{get{ return " + name + "; }set{ " + name + " = value; }}");
      propertyBuilder.AppendLine();
    }

    baseBuilder.AppendLine();
    baseBuilder.Append(variableBuilder);
    baseBuilder.AppendLine();
    baseBuilder.Append(propertyBuilder);
    baseBuilder.AppendLine();

    StringBuilder enumBuilder = new StringBuilder();
    foreach (var item in config.EnumList)
    {
      int idx = item.IndexOf(",");
      string enumName = item.Substring(0, idx);

      enumBuilder.Append(space + "public enum " + enumName);
      enumBuilder.AppendLine();
      enumBuilder.Append(space + "{");
      enumBuilder.AppendLine();

      deep++;
      space = GetSpace(deep);

      string elementStr = item.Substring(idx + 1);
      int idx2 = 0;

      while (true)
      {
        idx2 = elementStr.IndexOf(",");

        if (idx2 == -1)
          break;

        string elementName = elementStr.Substring(0, idx2 + 1);
        elementStr = elementStr.Substring(idx2 + 1);

        enumBuilder.Append(space + elementName);
        enumBuilder.AppendLine();
      }

      enumBuilder.Append(space + elementStr);
      enumBuilder.AppendLine();

      deep--;
      space = GetSpace(deep);
      enumBuilder.AppendLine(space + "}");
      enumBuilder.AppendLine();
      enumBuilder.AppendLine();
    }

    baseBuilder.AppendLine();
    baseBuilder.Append(enumBuilder);

    space = GetSpace(classDeep);
    baseBuilder.Append(space + "}");

    //namespaceEnd
    if (!string.IsNullOrEmpty(config.NameSpaceName))
    {
      baseBuilder.AppendLine();
      baseBuilder.Append("}");
    }

    File.WriteAllText(outPath + config.ClassName+".cs", baseBuilder.ToString());
    AssetDatabase.SaveAssets();
    AssetDatabase.Refresh();

    //参照がおかしくなるから一旦nullで新規取り直す
    config = null;
    serializedObject = null;
    enumList = null;
    usingList = null;
    variableDataList = null;
  }

  string GetSpace(int _deep)
  {
    string spaceBase = "";
    string space = "";

    for (int i = 0; i < config.SpaceNum; i++)
    {
      spaceBase += " ";
    }

    for (int i = 0; i < _deep; i++)
    {
      space += spaceBase;
    }

    return space;
  }
}
