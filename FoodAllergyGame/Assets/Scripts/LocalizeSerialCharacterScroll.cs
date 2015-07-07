using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Localized text with character showing one by one, much like an rpg
public class LocalizeSerialCharacterScroll : Localize{

	public float delayBetweenChars = 0.1f;
	private string textToShow;
	private int currentIndex = 0;

	public override void _Start(){
		textToShow = textComponent.text;	// Save the text, clear it, and start showing it
		textComponent.text = "";
		StartCoroutine(CharacterDelay());
	}

	private IEnumerator CharacterDelay(){
		if(textToShow.Length > currentIndex){
			textComponent.text = textComponent.text += textToShow[currentIndex];
			currentIndex++;
			yield return new WaitForSeconds(delayBetweenChars);
			StartCoroutine(CharacterDelay());
		}
		else{
			yield return null;
		}
	}
}
