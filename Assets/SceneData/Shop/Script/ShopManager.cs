using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Common.DataBase;
using ShopItem;
public class ShopManager : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
    	//適当に
    	SceneChanger.Instance.IsInitialize = true;
		ShopData = DataBaseManager.Instance.GetDataBase<MasterShopDB> ();

		itemData_ = ShopData.GetDataArray ();
		foreach (var item in itemData_) 
		{
			GameObject itemObj = (GameObject)Instantiate(cell);
			itemObj.transform.SetParent (content,false);
			itemObj.GetComponent<Item> ().SetUp (item);
			itemList.Add (itemObj);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[SerializeField]
	private GameObject cell;
	[SerializeField]
	private RectTransform content;

	private List<GameObject> itemList = new List<GameObject> ();
	private MasterShopDB ShopData;
	private ProductData[] itemData_;

}
