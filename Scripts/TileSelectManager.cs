using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileSelectManager : MonoBehaviour {

	public static TileSelectManager _instance;
	private MapManager mapManagerInstance;
	private OptionsManager optionManager;
	public Renderer rend;
	private Player player;
	public AudioClip walk;

	// Use this for initialization
	void Start () {
		_instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void addInstances() {
		if( this.player == null ) {
			this.player = Game._instance.playerInstance;
		}
		if( this.mapManagerInstance == null ) {
			this.mapManagerInstance = MapManager._instance;
		}
		if( this.optionManager == null ) {
			this.optionManager = OptionsManager._instance;
		}
	}

	// The mesh goes red when the mouse is over it...
	void OnMouseEnter()
	{		

		int tileX = Int32.Parse( this.name.Split( '_' )[ 1 ] );
		int tileZ = Int32.Parse( this.name.Split( '_' )[ 2 ] );
		addInstances();
		MapTile highlightedTile = mapManagerInstance.getTileAt( tileX, tileZ );

		if( tileX == player.posX && tileZ == player.posZ ) return;
		if( mapManagerInstance.queuedTile != null ) {
			if( mapManagerInstance.queuedTile.X == tileX && mapManagerInstance.queuedTile.Z == tileZ )
				return;
		}

		MapTile[] neighbouringTiles = new MapTile[4];
		neighbouringTiles[ 0 ] = MapManager._instance.getTileAt(tileX + 1, tileZ);
		neighbouringTiles[ 1 ] = MapManager._instance.getTileAt(tileX - 1, tileZ);
		neighbouringTiles[ 2 ] = MapManager._instance.getTileAt(tileX, tileZ + 1);
		neighbouringTiles[ 3 ] = MapManager._instance.getTileAt(tileX, tileZ - 1);

		bool accessableTile = false;

		for( int tiles = 0; tiles < neighbouringTiles.Length; tiles++ ) {
			if( neighbouringTiles[ tiles ].X == player.posX && neighbouringTiles[ tiles ].Z == player.posZ ) {
				accessableTile = true;
			}
		}

		if( accessableTile ) {
			rend.material.color = new Color( 0.1f, 0.4f, 0.1f );
		} else {
			rend.material.color = new Color( 0.4f, 0.3f, 0.2f );
		}
	}

	// ...and the mesh finally turns white when the mouse moves away.
	void OnMouseExit()
	{
		int tileX = Int32.Parse( this.name.Split( '_' )[ 1 ] );
		int tileZ = Int32.Parse( this.name.Split( '_' )[ 2 ] );
		addInstances();
		MapTile highlightedTile = mapManagerInstance.getTileAt( tileX, tileZ );

		if( mapManagerInstance.queuedTile != null ) {
			if( mapManagerInstance.queuedTile.X == tileX && mapManagerInstance.queuedTile.Z == tileZ )
				return;
		}
		rend.material.color = new Color( 0.34f, 0.13f,0.053f );

	}

	void OnMouseUp() {
		SoundManager.PlayClip( walk );
		if(OptionsManager._instance.FightPanel.activeSelf == false){
			int tileX = Int32.Parse( this.name.Split( '_' )[ 1 ] );
			int tileZ = Int32.Parse( this.name.Split( '_' )[ 2 ] );
			MapTile highlightedTile = MapManager._instance.getTileAt( tileX, tileZ );
			addInstances();

			MapTile[] neighbouringTiles = new MapTile[4];
			neighbouringTiles[ 0 ] = MapManager._instance.getTileAt(tileX + 1, tileZ);
			neighbouringTiles[ 1 ] = MapManager._instance.getTileAt(tileX - 1, tileZ);
			neighbouringTiles[ 2 ] = MapManager._instance.getTileAt(tileX, tileZ + 1);
			neighbouringTiles[ 3 ] = MapManager._instance.getTileAt(tileX, tileZ - 1);

			for( int tiles = 0; tiles < neighbouringTiles.Length; tiles++ ) {
				if( neighbouringTiles[ tiles ].X == player.posX && neighbouringTiles[ tiles ].Z == player.posZ ) {
					player.setPlayerObjectToRightLocation( highlightedTile );
					if( highlightedTile.Monster ) {
						if( highlightedTile.MonsterClass.infoText.Length < 5 ) {
							highlightedTile.MonsterClass.setDiceRolls();
						}
						optionManager.fightHeader.text = "A " + highlightedTile.MonsterClass.name + " has approached you. Prepare to fight!";
						optionManager.fightInfo.text = highlightedTile.MonsterClass.infoText;
					}
					mapManagerInstance.updateMap();
				}
			}
		}
	}
}
