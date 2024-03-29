﻿using UnityEngine;
using UnityEngine.UI;

public static class ParticleAndFloatyUtils{

	static public void PlayMoneyFloaty(Vector3 pos, int amount){
		GameObject go = Resources.Load("CoinFloaty") as GameObject;
		GameObject instance = GameObjectUtils.AddChildGUI(GameObject.Find("Canvas"), go);
		pos.z = 0f;
		instance.transform.position = pos;

		Text textScript = instance.GetComponentInChildren<Text>();
		Image imageScript = instance.GetComponentInChildren<Image>();

		if(amount > 0){
			textScript.text = "+" + amount.ToString();
		}
		else{
			textScript.text = amount.ToString();
			textScript.color = new Color(1f, 0.242f, 0.242f, 1f);
			imageScript.color = new Color(1f, 0.586f, 0.586f, 1f);
        }
    }

	static public void PlayHandsFullFloaty(Vector3 pos) {
		GameObject go = Resources.Load("HandsFullFloaty") as GameObject;
		GameObject instance = GameObjectUtils.AddChildGUI(GameObject.Find("Canvas"), go);
		instance.transform.position = pos;
		AudioManager.Instance.PlayClip("HandsFull");
	}

	static public void PlaySlotsFullFloaty(GameObject _parent) {
		GameObject parent;
        if(_parent == null) {
			parent = GameObject.Find("Canvas");
        }
		else {
			parent = _parent;
		}
		GameObject go = Resources.Load("SlotsFullFloaty") as GameObject;
		GameObject instance = GameObjectUtils.AddChildGUI(parent, go);
		instance.transform.localPosition = Vector3.zero;
		AudioManager.Instance.PlayClip("HandsFull");
	}

	static public void PlayDecoChangePoof(Vector3 pos){
		AudioManager.Instance.PlayClip("DecoPoofSound");
		GameObject go = Resources.Load("DecoPoof") as GameObject;
		GameObject.Instantiate(go, pos, go.transform.rotation);
	}

	static public void PlayTeleportParticle(Vector3 pos) {
		GameObject go = Resources.Load("ParticleTeleport") as GameObject;
		GameObject.Instantiate(go, pos, go.transform.rotation);
	}

	// When table smasher smashes a table
	static public void PlayTableSmashedParticle(Vector3 pos) {
		GameObject go = Resources.Load("TableSmashPoof") as GameObject;
		GameObject.Instantiate(go, pos, go.transform.rotation);
	}
}
