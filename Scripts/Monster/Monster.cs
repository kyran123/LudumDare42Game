using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster {

	private int difficulty;

	public enum Effects {
		LoseHealth,
		WinFight,
		Flee,
		Stagnate
	}

	public string[] Names = {
		"Zombie",
		"Skeleton",
		"Vampire",
		"Werewolf"
	};

	public string infoText;
	public string name;

	public Effects[] effects = new Effects[6];

	public Monster(int diff) {
		this.difficulty = diff;
		this.name = Names[ Random.Range( 0, Names.Length - 1 ) ];
		setDiceRolls();
	}

	public void setDiceRolls() {
		if( difficulty > 0 && difficulty < 8 ) {
			effects[ 0 ] = Effects.Flee;
			effects[ 1 ] = Effects.LoseHealth;
			effects[ 2 ] = Effects.WinFight;
			effects[ 3 ] = Effects.Stagnate;
			effects[ 4 ] = Effects.WinFight;
			effects[ 5 ] = Effects.Flee;
			infoText = "[1] Flee \n" +
						"[2] Lose 1 heart \n" +
						"[3] Win fight \n" +
						"[4] Stagnate battle \n" +
						"[5] Win fight \n" +
						"[6] Flee \n";
		}
		if( difficulty > 8 && difficulty < 18 ) {
			effects[ 0 ] = Effects.LoseHealth;
			effects[ 1 ] = Effects.LoseHealth;
			effects[ 2 ] = Effects.WinFight;
			effects[ 3 ] = Effects.WinFight;
			effects[ 4 ] = Effects.Flee;
			effects[ 5 ] = Effects.Stagnate;
			infoText = "[1] Lose 1 heart \n" +
						"[2] Lose 1 heart \n" +
						"[3] Win fight \n" +
						"[4] Win fight \n" +
						"[5] Flee \n" +
						"[6] Stagnate \n";
		}
		if( difficulty > 18 && difficulty < 32 ) {
			effects[ 0 ] = Effects.LoseHealth;
			effects[ 1 ] = Effects.LoseHealth;
			effects[ 2 ] = Effects.WinFight;
			effects[ 3 ] = Effects.WinFight;
			effects[ 4 ] = Effects.Flee;
			effects[ 5 ] = Effects.Stagnate;
			infoText = "[1] Lose 1 heart \n" +
						"[2] Lose 1 heart \n" +
						"[3] Win fight \n" +
						"[4] Stagnate \n" +
						"[5] Win fight \n" +
						"[6] Lose 1 heart \n";
		}
		if( difficulty > 32 && difficulty < 56 ) {
			effects[ 0 ] = Effects.LoseHealth;
			effects[ 1 ] = Effects.LoseHealth;
			effects[ 2 ] = Effects.WinFight;
			effects[ 3 ] = Effects.WinFight;
			effects[ 4 ] = Effects.Flee;
			effects[ 5 ] = Effects.Stagnate;
			infoText = "[1] Lose 1 heart \n" +
				"[2] Lose 1 heart \n" +
				"[3] Win fight \n" +
				"[4] Lose 1 heart \n" +
				"[5] Win fight \n" +
				"[6] Lose 1 heart \n";
		}
		if( difficulty > 56 && difficulty < 78 ) {
			effects[ 0 ] = Effects.LoseHealth;
			effects[ 1 ] = Effects.LoseHealth;
			effects[ 2 ] = Effects.WinFight;
			effects[ 3 ] = Effects.WinFight;
			effects[ 4 ] = Effects.Flee;
			effects[ 5 ] = Effects.Stagnate;
			infoText = "[1] Lose 1 heart \n" +
				"[2] stagnate \n" +
				"[3] Win fight \n" +
				"[4] Stagnate \n" +
				"[5] lose 1 heart \n" +
				"[6] Lose 1 heart \n";
		}
		if( difficulty > 78 && difficulty < 11600 ) {
			effects[ 0 ] = Effects.LoseHealth;
			effects[ 1 ] = Effects.LoseHealth;
			effects[ 2 ] = Effects.WinFight;
			effects[ 3 ] = Effects.WinFight;
			effects[ 4 ] = Effects.Flee;
			effects[ 5 ] = Effects.Stagnate;
			infoText = "[1] Lose 1 heart \n" +
				"[2] Lose 1 heart \n" +
				"[3] Lose 1 heart \n" +
				"[4] Lose 1 heart \n" +
				"[5] Win fight \n" +
				"[6] Lose 1 heart \n";
		}

	}
}
