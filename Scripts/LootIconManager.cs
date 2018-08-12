using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;

public class LootIconManager : MonoBehaviour {

	private LootManager lootManager;

	// Use this for initialization
	void Start () {
		getLootManager();
	}

	public void getLootManager() {
		if( lootManager == null ) {
			lootManager = LootManager._instance;
		}
	}
	
	// Update is called once per frame
	public void updateLootText() {
		getLootManager();
		int id = Int32.Parse(Regex.Replace(this.name, @"[^0-9]", "").ToString());
		lootManager.updateText(id);
	}
	public void removeLastLootText() {
		getLootManager();
		lootManager.updateText(0);
	}
}
