using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DecoUIToggleController : MonoBehaviour {

	public TweenToggle decoTweenToggle;
	public Image imageSymbol;
	public Sprite upSprite;
	public Sprite downSprite;

	public void OnToggleButtonClicked(){
		if(decoTweenToggle.IsShown){
			decoTweenToggle.Hide();
			imageSymbol.sprite = upSprite;
		}
		else{
			decoTweenToggle.Show();
			imageSymbol.sprite = downSprite;
		}
	}
}
