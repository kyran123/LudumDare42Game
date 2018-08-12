using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player {

	public static Player _instance;
	public GameObject playerObject;
	public HealthManager healthManagerInstance;
	public OptionsManager optionsManagerInstance;

	private int playerPosX = 0;
	public int posX {
		get {
			return playerPosX;
		}
	}
	private int playerPosZ = 0;
	public int posZ {
		get {
			return playerPosZ;
		}
	}

	public LootItem item1;
	public LootItem item2;
	public LootItem item3;

	public Player() {
		healthManagerInstance = HealthManager._instance;
		updateManagers();
	}

	public void updateManagers() {
		if( _instance == null ) {
			_instance = this;
		}
		if( healthManagerInstance == null ) {
			healthManagerInstance = HealthManager._instance;
		}
		if( optionsManagerInstance == null ) {
			optionsManagerInstance = OptionsManager._instance;
		}
	}

	public void setPlayerObjectToRightLocation(MapTile Tile){
		this.playerPosX = Tile.X;
		this.playerPosZ = Tile.Z;
		updatePlayerObjectPosition();
		updateManagers();
		if( Tile.Type != MapTile.TileType.Spawn ) {
			if( OptionsManager._instance == null ) updateManagers();
			OptionsManager._instance.addOptions( Tile.Monster, Tile.Loot, Tile.ladder );
		}
	}

	public void updatePlayerObjectPosition(){
		playerObject.transform.localScale = new Vector3(0.2f, 0.5f, 0.5f);
		playerObject.transform.localRotation = Quaternion.Euler( 0, 0, 270 );
		playerObject.transform.position = new Vector3(this.playerPosX, 0.3f, this.playerPosZ);
	}


	public void addItem(LootItem newItem){
		item3 = item2;
		item2 = item1;
		item1 = newItem;
		//Check for instant effects
		updateManagers();
		switch( item1.id ) {
			case 0:
				healthManagerInstance.addArmour();
				break;
			case 1:
				healthManagerInstance.addHealth();
				break;
			case 4:
				MapManager._instance.winPanel.SetActive( true );
				return;
		}
		LootManager._instance.updateView(item1, item2, item3);
	}

	public bool hasLuckyRing() {
		if( item1 != null ) 	if( item1.id == 2 ) return true;
		if( item2 != null ) 	if( item2.id == 2 ) return true;
		if( item3 != null ) 	if( item3.id == 2 ) return true;
		return false;
	}

	public bool hasNecklaceOfProtection(){
		if( item1 != null ) 	if( item1.id == 3 ) return true;
		if( item2 != null ) 	if( item2.id == 3 ) return true;
		if( item3 != null ) 	if( item3.id == 3 ) return true;
		return false;
	}

}

public class LootItem {

	public Dictionary<string, int> Names = new Dictionary<string, int>();

	public string[] Descriptions = {
		"Gives you 1 armor when looted",
		"Heals you 1 health when looted",
		"When equipped, gives you 10% chance to automatically win a battle without having to roll a dice.",
		"You have 10% chance to not lose any health!",
		"You found another means of escaping this dungeon!"
	};

	public int id;
	public string name;
	public string description;

	public LootItem(){
		//Add names
		Names.Add( "Shield", 2 );
		Names.Add( "Potion of healing", 0 );
		Names.Add( "Lucky Ring", 5 );
		Names.Add( "Necklace of protection", 15 );
		Names.Add( "Rope", 30 );

		bool index = true;

		while( index == true ) {
			int potentialID = Random.Range( 0, Names.Count );
			if( Names.ElementAt( potentialID ).Value <= Game._instance.level ) {
				id = potentialID;
				name = Names.ElementAt( potentialID ).Key;
				description = Descriptions[ potentialID ];
				index = false;
			}
		}
	}
}