using UnityEngine;
using UnityEngine.UI;

public static class ParticleUtils{

	static public void PlayMoneyFloaty(Vector3 pos, int amount){
		GameObject go = Resources.Load("CoinFloaty") as GameObject;
		GameObject Instance = GameObject.Instantiate(go, pos, go.transform.rotation) as GameObject;

		Text textScript = Instance.GetComponentInChildren<Text>();
		Image imageScript = Instance.GetComponentInChildren<Image>();

		if(amount > 0){
			textScript.text = "+" + amount.ToString();
		}
		else{
			textScript.text = amount.ToString();
			textScript.color = new Color(1f, 0.242f, 0.242f, 1f);
			imageScript.color = new Color(1f, 0.586f, 0.586f, 1f);
        }
	}

	static public void PlayDecoChangePoof(Vector3 pos){
		GameObject go = Resources.Load("DecoPoof") as GameObject;
		GameObject.Instantiate(go, pos, go.transform.rotation);
	}

	static public void PlayTeleportParticle(Vector3 pos){
		GameObject go = Resources.Load("ParticleTeleport") as GameObject;
		GameObject.Instantiate(go, pos, go.transform.rotation);
	}

	static public void PlayHandsFullFloaty(Vector3 pos){
		GameObject go = Resources.Load("HandsFullFloaty") as GameObject;
		GameObject.Instantiate(go, pos, go.transform.rotation);
		AudioManager.Instance.PlayClip("HandsFull");
	}

	// When table smasher smashes a table
	static public void PlayTableSmashedParticle(Vector3 pos) {
		GameObject go = Resources.Load("TableSmashPoof") as GameObject;
		GameObject.Instantiate(go, pos, go.transform.rotation);
	}
}
