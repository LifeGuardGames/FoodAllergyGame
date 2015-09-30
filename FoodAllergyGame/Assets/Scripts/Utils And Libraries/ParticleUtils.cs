﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class ParticleUtils{

	static public void  PlayMoneyFloaty(Vector3 pos, int amount){
		GameObject go = Resources.Load("CoinFloaty") as GameObject;
		GameObject Instance = GameObject.Instantiate(go, pos, go.transform.rotation) as GameObject;
	
		if(amount > 0){
			Instance.GetComponentInChildren<Text>().text = "+" + amount.ToString();
		}
		else{
			Instance.GetComponentInChildren<Text>().text = amount.ToString();
		}
	}

	static public void  PlayDecoChangePoof(Vector3 pos){
		GameObject go = Resources.Load("DecoPoof") as GameObject;
		GameObject.Instantiate(go, pos, go.transform.rotation);
	}

	static public void  PlayTeleportParticle(Vector3 pos){
		GameObject go = Resources.Load("ParticleTeleport") as GameObject;
		GameObject.Instantiate(go, pos, go.transform.rotation);
	}

	static public void  PlayHandsFullFloaty(Vector3 pos){
		GameObject go = Resources.Load("HandsFull") as GameObject;
		GameObject.Instantiate(go, pos, go.transform.rotation);
		AudioManager.Instance.PlayClip("hFull");
	}
}
