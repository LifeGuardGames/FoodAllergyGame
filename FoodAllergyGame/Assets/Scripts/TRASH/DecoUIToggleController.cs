using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DecoUIToggleController : MonoBehaviour {

	public TweenToggleDemux decoTweenToggle;
	public Image imageSymbol;
	public Sprite upSprite;
	public Sprite downSprite;
	public GameObject decoTut;

	void Start(){
		if(DataManager.Instance.GameData.Tutorial.IsDecoTutDone){
			decoTut.SetActive(false);
		}
	}

	public void OnToggleButtonClicked(){
		if(decoTweenToggle.IsShowing){
			decoTweenToggle.Hide();
			imageSymbol.sprite = upSprite;
		}
		else{
			if(!DataManager.Instance.GameData.Tutorial.IsDecoTutDone){
				decoTut.SetActive(false);
				DataManager.Instance.GameData.Tutorial.IsDecoTutDone = true;
			}
			decoTweenToggle.Show();
			imageSymbol.sprite = downSprite;
		}
	}
}
