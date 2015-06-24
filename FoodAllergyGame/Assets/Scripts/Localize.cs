using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Localize : MonoBehaviour {

	public string key;

	// Use this for initialization
	void Start () {
		if(key != "" && key != null){
			//Debug.Log ( LocalizationText.GetText(key));
			GetComponent<Text>().text = LocalizationText.GetText(key);
		}
	}

	public string setText(string _key){
		return LocalizationText.GetText(_key);
	}

	// Update is called once per frame
	void Update () {
	}
}
