using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionManager : MonoBehaviour {

	public AudioClip click;

	void Start() {
		this.GetComponent<Button>().onClick.AddListener( this.executeAction );;
	}

	public void executeAction() {
		SoundManager.PlayClip( click );
		if( this.name.Contains( "Nothing" ) ) {
			//Do nothing
			return;
		}
		if( this.name.Contains( "Loot" ) ) {
			OptionsManager._instance.showLootPanel(this.gameObject);
		}
		if( this.name.Contains( "Ladder" ) ) {
			LevelManager._instance.nextLevel();
			MapManager._instance.showNewMap();
			Game._instance.playerInstance.setPlayerObjectToRightLocation(MapManager._instance.getSpawnTile());
			if( OptionsManager._instance == null ) {
				GameObject.Find( "TileOptionsPanel" ).GetComponentInChildren<OptionsManager>().addOptions( MapManager._instance.getSpawnTile().Monster, MapManager._instance.getSpawnTile().Loot, MapManager._instance.getSpawnTile().ladder );
			} else {
				OptionsManager._instance.addOptions( MapManager._instance.getSpawnTile().Monster, MapManager._instance.getSpawnTile().Loot, MapManager._instance.getSpawnTile().ladder );
			}
		}
	}

}
