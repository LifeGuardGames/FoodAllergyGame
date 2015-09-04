using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DecoUIToggleController : MonoBehaviour {

	public TweenToggle decoTweenToggle;
	public Image imageSymbol;
	public Sprite upSprite;
	public Sprite downSprite;
	public GameObject decoTuT;

	void Start(){
		if(DataManager.Instance.GameData.Tutorial.IsDecoTuTDone){
			decoTuT.SetActive(false);
		}
	}

	public void OnToggleButtonClicked(){
		if(decoTweenToggle.IsShown){
			decoTweenToggle.Hide();
			imageSymbol.sprite = upSprite;
		}
		else{
			if(!DataManager.Instance.GameData.Tutorial.IsDecoTuTDone){
				decoTuT.SetActive(false);
				DataManager.Instance.GameData.Tutorial.IsDecoTuTDone = true;
			}
			decoTweenToggle.Show();
			imageSymbol.sprite = downSprite;
		}
	}
}
