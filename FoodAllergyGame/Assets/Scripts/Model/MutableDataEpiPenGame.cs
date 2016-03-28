using UnityEngine;
using System.Collections;

public class MutableDataEpiPenGame {

	public bool HasPlayedEpiPenGameThisTier;
	public int ChanceOfEpiGame;
	public int Difficulty;

	public MutableDataEpiPenGame() {
		HasPlayedEpiPenGameThisTier = false;
		ChanceOfEpiGame = 0;
		Difficulty = 3;
	}
}
