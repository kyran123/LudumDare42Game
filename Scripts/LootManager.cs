using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootManager : MonoBehaviour {

	public static LootManager _instance;
	private Player playerInstance;

	public GameObject Icon1Object;
	public Image Icon1;
	public GameObject Icon2Object;
	public Image Icon2;
	public GameObject Icon3Object;
	public Image Icon3;

	public Text LootInfoText;

	public Sprite[] Icons;

	// Use this for initialization
	void Start () {
		_instance = this;
	}

	// Update is called once per frame
	void Update () {
		if( playerInstance == null ) {
			playerInstance = Player._instance;
		}
	}

	public void updateView(LootItem loot1, LootItem loot2, LootItem loot3) {
		if( loot1 != null ) Icon1.sprite = Icons[ loot1.id ];
		if( loot2 != null ) Icon2.sprite = Icons[ loot2.id ];
		if( loot3 != null ) Icon3.sprite = Icons[ loot3.id ];
	}

	public void updateText(int id) {
		if( id == 1 ) {
			if( playerInstance.item1 != null ) {
				LootInfoText.text = playerInstance.item1.description; 
			}
		} else if( id == 2 ) {
			if( playerInstance.item2 != null ) {
				LootInfoText.text = playerInstance.item2.description; 
			}
		} else if( id == 3 ) {
			if( playerInstance.item3 != null ) {
				LootInfoText.text = playerInstance.item3.description; 
			}
		} else {
			LootInfoText.text = "";
		}
	}
}
