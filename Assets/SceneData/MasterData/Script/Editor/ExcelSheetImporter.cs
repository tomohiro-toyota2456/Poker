using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Excel;
using System;

public class ExcelSheetImporter : EditorWindow
{
  static string excelPath = "Assets/SceneData/MasterData/Editor/";
  static string skillDataExcelPath = "";
  static string itemDataExcelPath = "";
  static string shopDataExcelPath = "";
  static string exchangeShopDataExcelPath = "";
  static string skillShopDataExcelPath = "";

  static string DataPath = "Assets/SceneData/MasterData/Data/";

  [MenuItem("PokerProj/Excel/ImportWindow")]
  public static void Open()
  {
    skillDataExcelPath = excelPath + "SkillData.xlsx";
    itemDataExcelPath = excelPath + "ItemData.xlsx";
    shopDataExcelPath = excelPath + "ShopData.xlsx";
    exchangeShopDataExcelPath = excelPath + "ExchangeShopData.xlsx";
    skillShopDataExcelPath = excelPath + "SkillShopData.xlsx";

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

      EditorGUILayout.LabelField("SkillShopDataExcelPath");
      skillShopDataExcelPath = GUILayout.TextField(skillShopDataExcelPath);

      if (GUILayout.Button("CreateSkillShopData"))
      {
        CreateSkillShopData();
      }

    }
  }

  void CreateSkillData()
  {
    ExcelReader reader = new ExcelReader();
    reader.Open(skillDataExcelPath);
    reader.SetSheet("UserSkillData");
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
      string effect = reader.GetCellData(cnt, 4);
      string coolTime = reader.GetCellData(cnt, 5);
      string hand = reader.GetCellData(cnt, 6);
      
      SkillData skillData = CreateInstance<SkillData>();
      
      skillData.SkillId = skillId;
      skillData.SkillName = skillName;
      skillData.Dist = dist;
      skillData.Type = string.IsNullOrEmpty(skillType) ? SkillData.SkillType.AllBet : (SkillData.SkillType)Enum.Parse(typeof(SkillData.SkillType), skillType); 
      skillData.Effect = string.IsNullOrEmpty(effect) ? 0 : float.Parse(effect);
      skillData.CoolTime = string.IsNullOrEmpty(coolTime) ? 0 : int.Parse(coolTime);
      skillData.Hand = string.IsNullOrEmpty(hand) ? HandChecker.HandType.NoPair : (HandChecker.HandType)Enum.Parse(typeof(HandChecker.HandType), hand);

      AssetDatabase.CreateAsset(skillData, DataPath + "SkillData/UserSkillData/" + skillData.SkillId + ".asset");

      list.Add(skillData);
      cnt++;
    }

    Debug.Log("CreateSkillAssetFinish");
    Debug.Log("StartCreateEnemySkillAsset");

    reader.SetSheet("EnemySkillData");

    List<EnemySkillData> elist = new List<EnemySkillData>();
    cnt = 1;
    while (true)
    {
      string skillId = reader.GetCellData(cnt, 0);

      if (string.IsNullOrEmpty(skillId))
      {
        break;
      }

      string skillName = reader.GetCellData(cnt, 1);
      string dist = reader.GetCellData(cnt, 2);
      string effect = reader.GetCellData(cnt, 3);
      string skillType = reader.GetCellData(cnt, 4);
      string orderType = reader.GetCellData(cnt, 5);
      string colorType = reader.GetCellData(cnt, 6);
      string handType = reader.GetCellData(cnt, 7);
      string targetSkillType = reader.GetCellData(cnt, 8);

      EnemySkillData skillData = CreateInstance<EnemySkillData>();

      skillData.SkillId = skillId;
      skillData.SkillName = skillName;
      skillData.Dist = dist;
      skillData.Effect = string.IsNullOrEmpty(effect) ? 0 : float.Parse(effect);
      skillData.SType = string.IsNullOrEmpty(skillType) ? EnemySkillData.EnemySkillType.ForceAllChange : (EnemySkillData.EnemySkillType)Enum.Parse(typeof(EnemySkillData.EnemySkillType), skillType);
      skillData.OType = string.IsNullOrEmpty(orderType) ? EnemySkillData.OrderType.All : (EnemySkillData.OrderType)Enum.Parse(typeof(EnemySkillData.OrderType), orderType);
      skillData.CType = string.IsNullOrEmpty(colorType) ? EnemySkillData.ColorType.Black : (EnemySkillData.ColorType)Enum.Parse(typeof(EnemySkillData.ColorType), colorType);
      skillData.HType = string.IsNullOrEmpty(handType) ? HandChecker.HandType.NoPair : (HandChecker.HandType)Enum.Parse(typeof(HandChecker.HandType), handType);
      skillData.TargetSkillType = string.IsNullOrEmpty(targetSkillType) ? SkillData.SkillType.AllBet : (SkillData.SkillType)Enum.Parse(typeof(SkillData.SkillType), targetSkillType);

      AssetDatabase.CreateAsset(skillData, DataPath + "SkillData/EnemySkillData/" + skillData.SkillId + ".asset");

      elist.Add(skillData);
      cnt++;
    }

    MasterSkillData master = CreateInstance<MasterSkillData>();
    master.SkillDataArray = list.ToArray();
    master.EnemySkillArray = elist.ToArray();

    AssetDatabase.CreateAsset(master, DataPath + "SkillData/MasterData/SkillMaster.asset");
    reader.Close();
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
    reader.Close();
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
    reader.Close();
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
    reader.Close();
  }

  void CreateSkillShopData()
  {
    ExcelReader reader = new ExcelReader();
    reader.Open(skillShopDataExcelPath);
    reader.SetSheet("シート1");
    List<SkillProductData> list = new List<SkillProductData>();


    int cnt = 1;
    while (true)
    {
      string productId = reader.GetCellData(cnt, 0);

      if (string.IsNullOrEmpty(productId))
      {
        break;
      }

      string skillId = reader.GetCellData(cnt, 1);
      string value = reader.GetCellData(cnt, 2);


      SkillProductData productData = CreateInstance<SkillProductData>();
      productData.SkillId = skillId;
      productData.ProductId = productId;
      productData.Value = int.Parse(value);
      AssetDatabase.CreateAsset(productData, DataPath + "SkillShopData/" + productData.ProductId + ".asset");

      list.Add(productData);

      cnt++;
    }

    MasterSkillShopData master = CreateInstance<MasterSkillShopData>();
    master.SkillProductDataArray = list.ToArray();

    AssetDatabase.CreateAsset(master, DataPath + "SkillShopData/MasterData/SkillShopMaster.asset");

    Debug.Log("CreateAssetFinish");
    reader.Close();

  }

}
