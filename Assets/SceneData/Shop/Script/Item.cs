using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ShopItem
{

	public class Item : MonoBehaviour {

		public Text itemName { get { return itemName_;} set{ itemName_ = value;}}
		public int itemValue{ get { return itemValue_; } }

		public void SetUp(ProductData item)
		{
			itemName_.text = item.name;
			itemId_ = item.ItemId;
			itemValue_ = item.Value;

		}

		[SerializeField]
		private Text itemName_;

		[SerializeField]
		private Image icon_;

		[SerializeField]
		private Text money_;

		private int itemValue_;

		private bool isSoldOut_;

		private string itemId_;

	}
}