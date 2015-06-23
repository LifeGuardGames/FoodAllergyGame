using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Localize : MonoBehaviour {

	public string key;

	// Use this for initialization
	void Start () {
		//Debug.Log ( LocalizationText.GetText(key));
		GetComponent<Text>().text = LocalizationText.GetText(key);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
