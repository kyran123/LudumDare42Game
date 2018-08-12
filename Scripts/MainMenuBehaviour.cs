using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBehaviour : MonoBehaviour {

	public static MainMenuBehaviour _instance;
	public GameObject leaveGamePanel;
	public GameObject GameIntro;

	public AudioClip click;
	private bool setup = false;

	// Use this for initialization
	void Start () {
		setup = false;
	}
	
	// Update is called once per frame
	void Update () {
		if( _instance == null ) {
			_instance = this;
		}
		if( Input.GetKeyDown( KeyCode.Escape ) ) {
			if( leaveGamePanel.activeSelf == true ) {
				leaveGamePanel.SetActive( false );
			} else {
				leaveGamePanel.SetActive( true );
			}
		}

		if( GameIntro != null ) {
			if( GameIntro.activeSelf == true ) {
				if( Input.GetMouseButtonDown( 0 ) ) {
					GameIntro.SetActive( false );
					ToGame();
				}
			}
		}
	}

	public void showIntro() {
		SoundManager.PlayClip( click );
		GameIntro.SetActive( true );
		setup = true;
	}

	public void returnToGame() {
		SoundManager.PlayClip( click );
		leaveGamePanel.SetActive( false );
	}

	public void ToGame(){
		SoundManager.PlayClip( click );
		SceneManager.LoadScene( "_Game_Scene" );
	}

	public void toMenu(){
		SoundManager.PlayClip( click );
		SceneManager.LoadScene( "_MainMenu_Scene" );
	}

	public void toCredits() {
		SoundManager.PlayClip( click );
		SceneManager.LoadScene( "_End_Scene" );
	}
}
