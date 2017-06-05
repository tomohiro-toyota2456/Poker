using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class SkillShopManager : MonoBehaviour
{
  [SerializeField]
  SkillShopListManager listManager;

	// Use this for initialization
	void Start ()
  {

    listManager.AddItem("s00001", "aaaa", "dpff", 1, 200000);
    listManager.AddItem("s00001", "abaaa", "dp3ff", 1, 400);
    listManager.AddItem("s00001", "caaaa", "dp3ff", 1, 200);
    listManager.AddItem("s00001", "daaaa", "2dpff", 3, 200);
    listManager.AddItem("s00001", "eaaaa", "2dpff", 1, 200);
    SceneChanger.Instance.IsInitialize = true;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
