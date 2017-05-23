using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Excel;

public class ExcelSheetImporter : EditorWindow
{
  static string excelPath = "Assets/SceneData/MasterData/Editor/";
  static string skillDataExcelPath = "";
  static string itemDataExcelPath = "";
  static string shopDataExcelPath = "";
  static string exchangeShopDataExcelPath = "";

  static string DataPath = "Assets/SceneData/MasterData/Data/";

  [MenuItem("PokerProj/Excel/ImportWindow")]
  public static void Open()
  {
    skillDataExcelPath = excelPath + "SkillData.xlsx";
    itemDataExcelPath = excelPath + "ItemData.xlsx";
    shopDataExcelPath = excelPath + "ShopData.xlsx";
    exchangeShopDataExcelPath = excelPath + "ExchangeShopData.xlsx";

    var window = GetWindow<ExcelSheetImporter>();
  }

  public void OnGUI()
  {
    using (new EditorGUILayout.VerticalScope())
    {
      EditorGUILayout.LabelField("SkillDataExcelPath");
      skillDataExcelPath = GUILayout.TextField(skillDataExcelPath);

      if(GUILayout.Button("CreateSkillData"))
      {
        CreateSkillData();
      }

      EditorGUILayout.LabelField("ItemDataExcelPath");
      itemDataExcelPath = GUILayout.TextField(itemDataExcelPath);

      if (GUILayout.Button("CreateItemData"))
      {
        CreateItemData();
      }

      EditorGUILayout.LabelField("ShopDataExcelPath");
      shopDataExcelPath = GUILayout.TextField(shopDataExcelPath);

      if (GUILayout.Button("CreateShopData"))
      {
        CreateShopData();
      }

      EditorGUILayout.LabelField("ExchangeShopDataExcelPath");
      exchangeShopDataExcelPath = GUILayout.TextField(exchangeShopDataExcelPath);

      if (GUILayout.Button("CreateExchangeShopData"))
      {
        CreateExchangeShopData();
      }

    }
  }

  void CreateSkillData()
  {
    ExcelReader reader = new ExcelReader();
    reader.Open(skillDataExcelPath);
    reader.SetSheet("シート1");
    List<SkillData> list = new List<SkillData>();
    
    int cnt = 1;
    while(true)
    {
      string skillId = reader.GetCellData(cnt, 0);

      if(string.IsNullOrEmpty(skillId))
      {
        break;
      }

      string skillName = reader.GetCellData(cnt, 1);
      string dist = reader.GetCellData(cnt, 2);
      string skillType = reader.GetCellData(cnt, 3);
      string skillDetail = reader.GetCellData(cnt, 4);
      string value = reader.GetCellData(cnt, 5);
      string mark = reader.GetCellData(cnt, 6);
      
      SkillData skillData = CreateInstance<SkillData>();
      /*
      skillData.SkillId = skillId;
      skillData.SkillName = skillName;
      skillData.Dist = dist;
      skillData.MarkType = ConvertMarkTypeFromStr(mark);
      skillData.Type = ConvertSkillTypeFromStr(skillType);
      skillData.Detail = ConvertSkillDetailFromStr(skillDetail);
      skillData.Number = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
      */
      AssetDatabase.CreateAsset(skillData, DataPath + "SkillData/" + skillData.SkillId + ".asset");

      list.Add(skillData);
      cnt++;
    }

    MasterSkillData master = CreateInstance<MasterSkillData>();
    master.SkillDataArray = list.ToArray();

    AssetDatabase.CreateAsset(master, DataPath + "SkillData/MasterData/SkillMaster.asset");

    Debug.Log("CreateAssetFinish");

  }
  /*
  SkillData.SkillType ConvertSkillTypeFromStr(string _typeStr)
  {
    SkillData.SkillType type = SkillData.SkillType.Draw;
    switch(_typeStr)
    {
      case "Draw":
        type = SkillData.SkillType.Draw;
        break;

      case "Raise":
        type = SkillData.SkillType.Raise;
        break;
    }

    return type;
  }

  SkillData.SkillDetail ConvertSkillDetailFromStr(string _detailStr)
  {
    SkillData.SkillDetail detail = SkillData.SkillDetail.Raise;
    switch(_detailStr)
    {
      case "FixedNumber":
        detail = SkillData.SkillDetail.FixedNumber;
        break;

      case "FixedMark":
        detail = SkillData.SkillDetail.FiexedMark;
        break;
      case "AllChangeOnePair":
        detail = SkillData.SkillDetail.AllChangeOnePair;
        break;

      case "AllChangeTwoPair":
        detail = SkillData.SkillDetail.AllChangeTwoPair;
        break;

      case "AllChangeFlush":
        detail = SkillData.SkillDetail.AllChangeFlush;
        break;

      case "Raise":
        detail = SkillData.SkillDetail.Raise;
        break;

      case "ForceRaise":
        detail = SkillData.SkillDetail.ForceRaise;
        break;
    }

    return detail;
  }
  */
  HandChecker.MarkType ConvertMarkTypeFromStr(string _markStr)
  {
    HandChecker.MarkType markType = HandChecker.MarkType.Clover;

    switch(_markStr)
    {
      case "Heart":
        markType = HandChecker.MarkType.Heart;
        break;

      case "Dia":
        markType = HandChecker.MarkType.Dia;
        break;

      case "Spade":
        markType = HandChecker.MarkType.Spade;
        break;

      case "Clover":
        markType = HandChecker.MarkType.Clover;
        break;
    }

    return markType;
  }

  void CreateItemData()
  {
    ExcelReader reader = new ExcelReader();
    reader.Open(itemDataExcelPath);
    reader.SetSheet("シート1");
    List<ItemData> list = new List<ItemData>();

    int cnt = 1;
    while (true)
    {
      string itemId = reader.GetCellData(cnt, 0);

      if (string.IsNullOrEmpty(itemId))
      {
        break;
      }

      string itemName = reader.GetCellData(cnt, 1);
      string dist = reader.GetCellData(cnt, 2);

      ItemData itemData = CreateInstance<ItemData>();
      itemData.ItemId = itemId;
      itemData.ItemName = itemName;
      itemData.Dist = dist;

      AssetDatabase.CreateAsset(itemData, DataPath + "ItemData/" + itemData.ItemId + ".asset");

      list.Add(itemData);
      cnt++;
    }

    MasterItemData master = CreateInstance<MasterItemData>();
    master.ItemDataArray = list.ToArray();

    AssetDatabase.CreateAsset(master, DataPath + "ItemData/MasterData/ItemMaster.asset");

    Debug.Log("CreateAssetFinish");

  }

  void CreateShopData()
  {
    ExcelReader reader = new ExcelReader();
    reader.Open(shopDataExcelPath);
    reader.SetSheet("シート1");
    List<ProductData> list = new List<ProductData>();

    int cnt = 1;
    while (true)
    {
      string productId = reader.GetCellData(cnt, 0);

      if (string.IsNullOrEmpty(productId))
      {
        break;
      }

      string itemId = reader.GetCellData(cnt, 1);
      string value = reader.GetCellData(cnt, 2);

      ProductData productData = CreateInstance<ProductData>();
      productData.ItemId = itemId;
      productData.ProductId = productId;
      productData.Value = int.Parse(value);

      AssetDatabase.CreateAsset(productData, DataPath + "ShopData/" + productData.ProductId + ".asset");

      list.Add(productData);
      cnt++;
    }

    MasterShopData master = CreateInstance<MasterShopData>();
    master.ProductDataArray = list.ToArray();

    AssetDatabase.CreateAsset(master, DataPath + "ShopData/MasterData/ShopMaster.asset");

    Debug.Log("CreateAssetFinish");
  }

  void CreateExchangeShopData()
  {
    ExcelReader reader = new ExcelReader();
    reader.Open(exchangeShopDataExcelPath);
    reader.SetSheet("シート1");
    List<ExchangeProductData> list = new List<ExchangeProductData>();

    int cnt = 1;
    while (true)
    {
      string productId = reader.GetCellData(cnt, 0);

      if (string.IsNullOrEmpty(productId))
      {
        break;
      }

      string itemId = reader.GetCellData(cnt, 1);
      string minValue = reader.GetCellData(cnt, 2);
      string maxValue = reader.GetCellData(cnt, 3);

      ExchangeProductData productData = CreateInstance<ExchangeProductData>();
      productData.ItemId = itemId;
      productData.ProductId = productId;
      productData.MinVal = int.Parse(minValue);
      productData.MaxVal = int.Parse(maxValue);

      AssetDatabase.CreateAsset(productData, DataPath + "ExchangeShopData/" + productData.ProductId + ".asset");

      list.Add(productData);

      cnt++;
    }

    MasterExchangeShopData master = CreateInstance<MasterExchangeShopData>();
    master.ExchangeProductDataArray = list.ToArray();

    AssetDatabase.CreateAsset(master, DataPath + "ExchangeShopData/MasterData/ExchangeShopMaster.asset");

    Debug.Log("CreateAssetFinish");
  }
}
