using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	public static LevelManager _instance;
	public Text textObject;
	public int level;

	// Use this for initialization
	void Start () {
		_instance = this;
		this.level = 0;
		updateText();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void updateText() {
		textObject.text = level.ToString();
	}

	public void nextLevel() {
		this.level++;
		updateText();
	}
}
