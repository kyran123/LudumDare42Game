using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

	public static HealthManager _instance;

	int totalHealth;
	int totalArmour;

	public GameObject healthPanelObject;
	public GameObject health1Object;
	public GameObject health2Object;
	public GameObject health3Object;
	public GameObject ArmorObject;

	// Use this for initialization
	void Start () {
		_instance = this;
		totalHealth = 3;
		totalArmour = 0;
		health1Object.SetActive( false );
		health2Object.SetActive( false );
		health2Object.SetActive( false );
		ArmorObject.SetActive( false );
		updateHealth();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void addArmour() {
		if( totalArmour <= 0 ) {
			totalArmour++;
			updateHealth();
		} else {
			Debug.LogError( "Unable to add armour" );
		}
	}

	public void removeArmour() {
		if( totalArmour > 0 ) {
			totalArmour -= 1;
		}
	}

	public void updateHealth() {
		if( totalHealth >= 1 ) {
			health1Object.SetActive( true );
			health2Object.SetActive( false );
			health3Object.SetActive( false );
		}
		if( totalHealth >= 2 ) {
			health1Object.SetActive( true );
			health2Object.SetActive( true );
			health3Object.SetActive( false );
		}
		if( totalHealth >= 3 ) {
			health1Object.SetActive( true );
			health2Object.SetActive( true );
			health3Object.SetActive( true );
		}
		if( totalArmour > 0 ) ArmorObject.SetActive( true );
		if( totalArmour == 0 ) ArmorObject.SetActive( false );
	}

	public void loseHealth() {
		if( Player._instance.hasNecklaceOfProtection() ) {
			int num = Random.Range( 0, 100 );
			if( num < 10 )
				return;
		}

		if( totalArmour > 0 ) {
			totalArmour = 0;
		} else if( totalHealth > 1 ) {
			totalHealth -= 1;
		} else if( totalHealth <= 1 ) {
			totalHealth = 0;
			updateHealth();
			MainMenuBehaviour._instance.toCredits();
		}
		updateHealth();
	}

	public void addHealth() {
		if( totalHealth < 3 ) {
			totalHealth++;
			updateHealth();
		} else {
			Debug.LogError( "Unable to add health" );
		}
	}


}
