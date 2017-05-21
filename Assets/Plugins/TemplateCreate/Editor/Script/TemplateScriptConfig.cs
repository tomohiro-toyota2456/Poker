using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//簡易的なスクリプトを組む際のコンフィグデータ
[CreateAssetMenu(fileName ="TemplateScriptConfig",menuName ="ScriptableObject/TemplateScriptConfig",order =100)]
public class TemplateScriptConfig : ScriptableObject
{
  [SerializeField]
  string className="";
  [SerializeField]
  string derivationName;
  [SerializeField]
  string nameSpaceName="";
  [SerializeField]
  int spaceNum;
  [SerializeField]
  List<string> enumList = new List<string>();
  [SerializeField]
  List<VariableData> variableDataList = new List<VariableData>();
  [SerializeField]
  List<string> usingList = new List<string>();

  public string ClassName { get { return className; } set { className = value; } }
  public string DerivationName { get { return derivationName; }set { derivationName = value; } }
  public string NameSpaceName { get { return nameSpaceName; }set { nameSpaceName = value; } }
  public int SpaceNum { get { return spaceNum; } set { spaceNum = value; } }
  public List<VariableData> VariableDataList { get { return variableDataList; }set { variableDataList = value; } }
  public List<string> UsingList { get { return usingList; }set { usingList = value; } }
  public List<string> EnumList { get { return enumList; } set { enumList = value; } }

  [System.Serializable]
  public struct VariableData
  {
    public MoldType moldType;
    public string moldName;
    public string variableName;
    public bool isArray;
  }

  public enum MoldType
  {
    Int,
    Float,
    String,
    Long,
    Double,
    Enum,

    Vector2 = 20,
    Vector3,
    GameObject,
    RectTransform,
    Transform,
    Button,
    Image,
  }

}
