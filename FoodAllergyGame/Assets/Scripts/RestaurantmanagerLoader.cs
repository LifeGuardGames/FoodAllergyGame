using UnityEngine;
using System.Collections;

public class RestaurantmanagerLoader : MonoBehaviour {
	public GameObject RestArcade;
	public GameObject RestChallenge;
	// Use this for initialization
	void Start () {
		
		if(DataManager.Instance.GetChallenge() != "") {
			Debug.Log(DataManager.Instance.GetChallenge());
			RestArcade.SetActive(false);
			RestaurantManagerChallenge.Instance.Init();
		}
		else {
			RestChallenge.SetActive(false);
			RestaurantManagerArcade.Instance.Init();
		}
	}
}
