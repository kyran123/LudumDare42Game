using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapTile {

	//The type of the game tile. 
	//This is only for the type of ground
	public enum TileType { 
		Wall,
		Tile,
		Spawn,
		None
	};

	//The actual Enum variable that's accessable and changable from outside this class.
	private TileType type;
	public TileType Type {
		get {
			return type;
		}
		set {
			//Store the old type before changing to the new one
			TileType oldType = type;
			//Change type
			type = value;
		}
	}

	//Variable that keeps track of the X coord.
	//You are not allowed to change them as soon as the map has been built.
	//When game ends, all tile references will be destroyed/removed.
	private int x;
	public int X {
		get {
			return x;
		}
	}

	//Variable that keeps track of the Y coord.
	//You are not allowed to change them as soon as the map has been built.
	//When game ends, all tile references will be destroyed/removed.
	private int z;
	public int Z {
		get {
			return z;
		}
	}

	//GameObject
	private GameObject thisTile;
	public GameObject ThisTile {
		get {
			return thisTile;
		}
	}

	public bool Monster;
	public Monster MonsterClass;
	public bool Loot;
	public LootItem lootItem;
	public bool ladder;
	public bool nothing;

	//Initializes a new instance of the class
	public MapTile(int x, int z, string tileType) {
		this.x = x;
		this.z = z;
		setTileType( tileType );

		int currentLevel = Game._instance.level + 1;
		int difficultyRoll = UnityEngine.Random.Range(1, 100);
		int totalRoll = difficultyRoll / 3 + currentLevel;

		if( totalRoll < 15 ) {
			Loot = true;
			lootItem = new LootItem();
		}
		if( totalRoll < 20 && totalRoll > 8 ) {
			Monster = true;
			MonsterClass = new Monster(UnityEngine.Random.Range((1 + currentLevel), (10 * currentLevel)));
		}
		if( Monster != true && Loot != true && ladder != true ) nothing = true;
	}

	public void setTileType(string type){
		if( this != null ) {
			switch( type ) {
				case "Tile":
					this.Type = TileType.Tile;
					break;
				case "Wall":
					this.Type = TileType.Wall;
					break;
				case "Spawn":
					this.Type = TileType.Spawn;
					break;
				case "None":
				default:
					this.Type = TileType.None;
					break;
			}
		}
	}

	public void setTileGameObject(GameObject ob) {
		this.thisTile = ob;
	}
}
