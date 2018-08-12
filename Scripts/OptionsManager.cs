using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour {

	public static OptionsManager _instance;
	public GameObject lootOption;
	public GameObject ladderOption;
	public GameObject DoNothingOption;

	public GameObject FightPanel;
	public Image diceFace;
	public Button fightButton;
	public Button leaveFightButton;
	public GameObject leaveButtonObject;
	public Text fightInfo;
	public Text fightHeader;

	public GameObject lootOptionPanel;
	public Text lootHeaderText;
	public Text lootEffectText;
	public Button discardButton;
	public Button lootButton;

	public GameObject lootButtonObject;


	public AudioClip click;
	public AudioClip hurt;
	public AudioClip bag;
	public AudioClip[] Dice;

	public Sprite[] diceSides;
	int finalSide = 0;

	// Use this for initialization
	void Start () {
		_instance = this;
	}

	public void addOptions(bool monster, bool loot, bool ladder) {
		foreach( Transform child in transform ) {
			GameObject.Destroy( child.gameObject );
		}


		if( monster ) {
			fightButton.enabled = true;
			leaveFightButton.enabled = true;
			leaveButtonObject.SetActive( false );
			FightPanel.SetActive(true);
		}
		if( loot ) {
			lootOption = Instantiate( Resources.Load( "TileOption", typeof(GameObject) ) ) as GameObject;
			lootOption.transform.SetParent( transform, false );
			lootOption.name = "LootOption";
			lootOption.GetComponentInChildren<Text>().text = "Loot the tile!";
		}
		if( ladder ) {
			ladderOption = Instantiate( Resources.Load( "TileOption", typeof(GameObject) ) ) as GameObject;
			ladderOption.transform.SetParent( transform, false );
			ladderOption.name = "LadderOption";
			ladderOption.GetComponentInChildren<Text>().text = "Leave the room.";
		}
	}


	public void showLootPanel(GameObject lb) {
		SoundManager.PlayClip( click );
		lootButtonObject = lb;
		MapTile playerTile = MapManager._instance.getTileWherePlayerIsAt();
		if(playerTile.lootItem == null) playerTile.lootItem = new LootItem();
		lootHeaderText.text = "You found a " + playerTile.lootItem.name + "!";
		lootEffectText.text = playerTile.lootItem.description;
		lootOptionPanel.SetActive( true );
	}

	public void acceptLoot() {
		
		MapTile playerTile = MapManager._instance.getTileWherePlayerIsAt();
		if( playerTile.lootItem == null ) Debug.LogError( "No loot found!" );
		if( playerTile.lootItem.id == 0 ) {
			SoundManager.PlayClip( bag );
		} else {
			SoundManager.PlayClip( click );
		}
		Player._instance.addItem( playerTile.lootItem );
		MapManager._instance.getTileWherePlayerIsAt().Loot = false;
		GameObject.Destroy( lootButtonObject );
		lootButtonObject = null;
		lootOptionPanel.SetActive( false );
	}

	public void declineLoot() {
		SoundManager.PlayClip( click );
		lootOptionPanel.SetActive( false );
		lootHeaderText.text = "";
		lootEffectText.text = "";
	}

	public void closeFightWindow() {
		SoundManager.PlayClip( click );
		leaveFightButton.enabled = false;
		FightPanel.SetActive( false );
	}

	public void rollDice() {
		//disable button
		if( Player._instance.hasLuckyRing() ) {
			int num = Random.Range( 0, 100 );
			if( num < 10 ) {
				fightInfo.text = "You have automatically won the battle thanks to your lucky ring!";
				MapManager._instance.getTileWherePlayerIsAt().Monster = false;
				leaveButtonObject.SetActive(true);
			}
		}
		SoundManager.PlayRandomClip( Dice );
		fightButton.enabled = false;
		StartCoroutine( "RollTheDice" );
	}

	public void fightResult() {
		Player player = Game._instance.playerInstance;
		MapTile tile = MapManager._instance.getTileAt(player.posX, player.posZ);
		Monster monsterValues = tile.MonsterClass;

		if( monsterValues.effects[ finalSide ] == Monster.Effects.Flee ) {
			//Allow the user to close the window, but not roll dice again
			fightInfo.text = "You have fled!";
			leaveButtonObject.SetActive(true);
		}
		if( monsterValues.effects[ finalSide ] == Monster.Effects.LoseHealth ) {
			//Player loses health, reset button to allow user to roll dice again
			HealthManager._instance.loseHealth();
			SoundManager.PlayClip( hurt );
			fightButton.enabled = true;
		}
		if( monsterValues.effects[ finalSide ] == Monster.Effects.Stagnate ) {
			//Only reset button
			fightButton.enabled = true;
		}
		if( monsterValues.effects[ finalSide ] == Monster.Effects.WinFight ) {
			//Give XP / Gold. Allow the user to close the window, but not roll dice again
			fightInfo.text = "You have won the battle!";
			tile.Monster = false;
			leaveButtonObject.SetActive(true);
		}

	}

	private IEnumerator RollTheDice() {
		int randomDiceSide = 0;
		int limit = Random.Range( 16, 28 );
		finalSide = 0;

		for( int i = 0; i <= limit; i++ ) {
			randomDiceSide = Random.Range( 0, 5 );
			diceFace.sprite = diceSides[ randomDiceSide ];
			yield return new WaitForSeconds( 0.1f );
		}
		finalSide = randomDiceSide;

		fightResult();
	}

}
