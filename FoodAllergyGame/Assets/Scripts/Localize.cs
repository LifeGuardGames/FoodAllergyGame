using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Localize : MonoBehaviour {
	public string key;

	void Start(){
		if(key != "" && key != null){
			GetComponent<Text>().text = LocalizationText.GetText(key);
		}
	}

	public string SetText(string _key){
		return LocalizationText.GetText(_key);
	}
}
