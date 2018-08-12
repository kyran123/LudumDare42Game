using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;
using System.IO;

public class MapManager : MonoBehaviour {

	public static MapManager _instance;

	public Material ladderMaterial;
	public GameObject winPanel;

	//Array with 2 keys to store references to the tile classes
	// - 1st key is the X coordinate
	// - 2nd key is the z coordinate
	private MapTile[,] tiles;

	//The variable that keeps track of the width of the map
	//You are not allowed to change it as soon as the map has been built.
	//When game ends, all tile references will be destroyed/removed.
	private int width;
	public int Width {
		get {
			return width;
		}
	}

	//The variable that keeps track of the height of the map
	//You are not allowed to change it as soon as the map has been built.
	//When game ends, all tile references will be destroyed/removed.
	private int height;
	public int Height {
		get {
			return height;
		}
	}

	public MapTile queuedTile;
	public int minChanceValue = 30;
	public int userLuck = 5;
	public GameObject warning;


	// Use this for initialization
	void Start () {
		Debug.Log( this );
		_instance = this;

		//Code to load map from XML
		//creates new XmlDocument instance
		XmlDocument rawXML = new XmlDocument();

		//Loads file into the instance of XmlDocument
		rawXML.Load ("Assets/Data/Maps/map1.xml");

		//Get all elements within the XML with the tag 'Tile'
		XmlNodeList TileList = rawXML.GetElementsByTagName ("Tile");

		//Get the required map dimensions from the XML file.
		//NOTE: These have to be placed in the top of the document!
		width = Int32.Parse(rawXML.GetElementsByTagName ("Width")[0].ChildNodes[0].Value);
		height = Int32.Parse(rawXML.GetElementsByTagName ("Height")[0].ChildNodes[0].Value);

		createNewMap(TileList, width, height);
		Game._instance.setupPlayer();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void createNewMap(XmlNodeList TileList, int width, int height){

		tiles = new MapTile[width, height];

		//Loop through XML nodes in the TileList
		foreach( XmlNode tileInfo in TileList ) {
			//reset xPos, yPos and TileType to default for the new Tile
			int xPos = 0;
			int yPos = 0;
			string tileType = "Empty";

			//Get the child nodes from the Tile element
			XmlNodeList tileContent = tileInfo.ChildNodes;
			//Loop through child nodes
			foreach( XmlNode contentNode in tileContent ) {

				//Check the Tags within the XML file
				switch( contentNode.Name ) {
					case "X":
						//Set the xPos with the data from the XML file
						xPos = Int32.Parse( contentNode.InnerXml );
						break;
					case "Y":
						//Set the yPos with the data from the XML file
						yPos = Int32.Parse( contentNode.InnerXml );
						break;
					case "Type":
						//Set the tile type with the data from the XML file
						tileType = contentNode.InnerXml;
						break;
				}


			}

			//Stores the new tile Instance in the tiles array with the X and z coordinates as key
			tiles [xPos, yPos] = new MapTile (xPos, yPos, tileType);

			MapTile tile_data = getTileAt(xPos, yPos);

			switch( tile_data.Type ) {
				case MapTile.TileType.Tile:
				case MapTile.TileType.Spawn:
					tile_data.setTileGameObject(Instantiate(Resources.Load("Tile", typeof(GameObject))) as GameObject);
					tile_data.ThisTile.transform.localScale = new Vector3( 1f, 0.2f, 1f );
					tile_data.ThisTile.transform.position = new Vector3(tile_data.X, 0, tile_data.Z);
					break;
				case MapTile.TileType.Wall:
					tile_data.setTileGameObject( Instantiate( Resources.Load( "Wall", typeof(GameObject) ) ) as GameObject );
					tile_data.ThisTile.transform.localScale = new Vector3( 0.5f, 0.5f, 1f );
					tile_data.ThisTile.transform.localRotation = Quaternion.Euler( 90, 0, 0 );
					tile_data.ThisTile.transform.position = new Vector3(tile_data.X, 1, tile_data.Z);
					break;
				case MapTile.TileType.None:
					tile_data.setTileGameObject(Instantiate(Resources.Load("None", typeof(GameObject))) as GameObject);
					break;
			}

			//Set position of tile
			//It's position is based of the X and Z axes because this is supposed to be a flat board in a 3D build!
			//This means the camera is creating the Isometric view by angling it 50 degrees on the X and 45 on the Y axes.
			tile_data.ThisTile.transform.SetParent(this.transform, true);
			//Give object name
			tile_data.ThisTile.name = "Tile_" + tile_data.X + "_" + tile_data.Z;

		}


		setLadderInLevel();

	}

	//returns the tile instance found on X and Z coordinates
	public MapTile getTileAt(int x, int z){
		//Checks if the X and Y coordinate exceeds map limits
		if (x > width || x < 0 || z > height || z < 0) {
			//Log that the tile is out of range
			Debug.LogError ("Tile (" + x + ", " + z + ") is out of range.");
			//Return null instead of a tile
			return null;
		}
		//Return the tile based on the X and Z coordinate
		return tiles [x, z];
	}

	public MapTile getTileWherePlayerIsAt() {
		return tiles[ Player._instance.posX, Player._instance.posZ ];
	}

	//returns the tile instance found on X and Z coordinates
	public MapTile getSpawnTile(){
		for( int x = 0; x < (this.Width - 1); x++ ) {
			for( int z = 0; z < (this.height - 1); z++ ) {
				if( tiles[ x, z ].Type == MapTile.TileType.Spawn ) {
					return tiles[ x, z ];
				}
			}
		}
		return null;
	}

	public void setLadderInLevel() {
		bool index = true;

		while( index == true ) {
			int rand1 = UnityEngine.Random.Range( 0, this.width );
			int rand2 = UnityEngine.Random.Range( 0, this.height );
			MapTile potentialTile = getTileAt( rand1, rand2 );
			if( potentialTile.Type == MapTile.TileType.Tile ) {
				potentialTile.ladder = true;
				potentialTile.ThisTile.GetComponent<MeshRenderer>().material = ladderMaterial;
				//Set spawn tile things to false
				potentialTile.Monster = false;
				potentialTile.Loot = false;
				Debug.Log( "Ladder at: " + rand1 + "_" + rand2 );
				index = false;
			}
		}
	}

	public void showNewMap(){
		if( Game._instance.level == 98 ) {
			winPanel.SetActive( true );
			return;
		}

		foreach( Transform child in transform ) {
			GameObject.Destroy( child.gameObject );
		}
		DirectoryInfo dir = new DirectoryInfo("Assets/Data/Maps/" );
		FileInfo[] files = dir.GetFiles( "*.*" );
		bool index = true;
		int count = 0;
		int rand = 0;
		string file = "";

		while( index == true ) {
			count++;
			rand = UnityEngine.Random.Range( 0, ( files.Length - 1 ) );
			if( !files[ rand ].ToString().Contains("meta") ) {
				Debug.Log( files[ rand ].ToString() );
				file = files[ rand ].ToString();
				index = false;
			}
			if( count >= 100 ) {
				file = "Assets/Data/Maps/map1.xml";
				index = false;
			}
		}

		//Code to load map from XML
		//creates new XmlDocument instance
		XmlDocument rawXML = new XmlDocument();

		//Loads file into the instance of XmlDocument
		rawXML.Load( file );

		//Get all elements within the XML with the tag 'Tile'
		XmlNodeList TileList = rawXML.GetElementsByTagName ("Tile");

		//Get the required map dimensions from the XML file.
		//NOTE: These have to be placed in the top of the document!
		int mapWidth = Int32.Parse(rawXML.GetElementsByTagName ("Width")[0].ChildNodes[0].Value);
		int mapHeight = Int32.Parse(rawXML.GetElementsByTagName ("Height")[0].ChildNodes[0].Value);

		createNewMap(TileList, mapWidth, mapHeight);
	}


	/// <summary>
	/// Updates the map with new walls as the player does more moves
	/// </summary>
	public void updateMap() {
		if( queuedTile != null ) {
			if( queuedTile.ladder == true ) {
				//Can't continue game, since ladder has been lost!
				Debug.Log("Can't continue game, no ladder left");
				MainMenuBehaviour._instance.toCredits();
			}
			queuedTile.setTileType( "Wall" );
			GameObject.Destroy( queuedTile.ThisTile );
			queuedTile.setTileGameObject(Instantiate(Resources.Load("Wall", typeof(GameObject))) as GameObject);
			queuedTile.ThisTile.transform.SetParent(this.transform, true);
			queuedTile.ThisTile.transform.localScale = new Vector3( 0.5f, 0.5f, 1f );
			queuedTile.ThisTile.transform.localRotation = Quaternion.Euler( 90, 0, 0 );
			queuedTile.ThisTile.transform.position = new Vector3(queuedTile.X, 1, queuedTile.Z);
		}


		//Randomize the chance of a tile changing
		int chance = UnityEngine.Random.Range(0, 100);
		if( chance > (minChanceValue - Game._instance.level + userLuck) ) {

			bool index = true;

			//Loop until tile is found
			while( index == true ) {
				//Get random X coord
				//Get random Z coord
				int rand1 = UnityEngine.Random.Range( 0, this.width );
				int rand2 = UnityEngine.Random.Range( 0, this.height );
				//Get the tile with the new coords
				MapTile potentialTile = this.getTileAt( rand1, rand2 );
				//Check if tile meets criteria
				if( potentialTile.Type == MapTile.TileType.Tile ) {
					//Check if user is in early game
					if( Game._instance.level < 4 ) {
						MapTile[] neighbouringTiles = new MapTile[4];
						neighbouringTiles[ 0 ] = getTileAt( rand1 + 1, rand2 );
						neighbouringTiles[ 1 ] = getTileAt( rand1 - 1, rand2 );
						neighbouringTiles[ 2 ] = getTileAt( rand1, rand2 + 1 );
						neighbouringTiles[ 3 ] = getTileAt( rand1, rand2 - 1 );

						for( int i = 0; i < neighbouringTiles.Length; i++ ) {
							if( neighbouringTiles[ i ] == null ) {
								Debug.Log( "No neighbouring tile" );

							} else if( neighbouringTiles[ i ].Type == MapTile.TileType.Wall ) {
								queuedTile = potentialTile;
								queuedTile.ThisTile.GetComponent<MeshRenderer>().material.color = new Color( 0.4f, 0.4f, 0.4f );
								if( queuedTile.ladder ) {
									warning.SetActive( true );
								}
							}
						}
					}
					//Set spawn tile things to false
					potentialTile.Monster = false;
					potentialTile.Loot = false;
					index = false;
				}
			}

		}
	}
}
