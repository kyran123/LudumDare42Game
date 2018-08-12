using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	public static Game _instance;
	public Player playerInstance;

	public int level;

	// Use this for initialization
	void Start () {
		_instance = this;
		this.level = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if( _instance == null ) {
			_instance = this;
			if( playerInstance == null ) {
				playerInstance = Player._instance;
			}
		}
	}

	public void setupPlayer(){
		playerInstance = new Player();
		playerInstance.playerObject = Instantiate( Resources.Load( "PlayerObject", typeof(GameObject) ) ) as GameObject;
		playerInstance.setPlayerObjectToRightLocation(MapManager._instance.getSpawnTile());
		playerInstance.playerObject.gameObject.transform.SetParent( this.transform );
		if( OptionsManager._instance == null ) {
			GameObject.Find( "TileOptionsPanel" ).GetComponentInChildren<OptionsManager>().addOptions( false, false, false );
		} else {
			OptionsManager._instance.addOptions( false, false, false );
		}
	}
}
