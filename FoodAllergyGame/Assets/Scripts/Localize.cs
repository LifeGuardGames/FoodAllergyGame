using UnityEngine;
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

	public virtual void _Start(){}
}
