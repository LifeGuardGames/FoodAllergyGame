using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Localize : MonoBehaviour {
	public string key;
	protected string localizedText;
	protected Text textComponent;

	void Start(){
		if(key != null && key != ""){
			localizedText = GetText(key);
			GetComponent<Text>().text = localizedText;
		}
		_Start();
	}

	public virtual void _Start(){}

	public string GetText(string key){
		return LocalizationText.GetText(key);
	}
}
