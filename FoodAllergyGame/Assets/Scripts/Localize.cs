using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class Localize : MonoBehaviour {
	public string key;
	protected string localizedText;
	protected Text textComponent;

	void Start(){
		LocalizeText();
	}


	public void LocalizeText() {
		if(key != null && key != "") {
			localizedText = LocalizationText.GetText(key);
			textComponent = GetComponent<Text>();
			textComponent.text = localizedText;
		}
	}

	public void LocalizeText(object j) {
		if(key != null && key != "") {
			localizedText = LocalizationText.GetText(key);
			textComponent = GetComponent<Text>();
			textComponent.text = String.Format(localizedText, j.ToString());
		}
	}

	public virtual void _Start(){}
}
