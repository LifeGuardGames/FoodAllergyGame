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
		if(DataManager.Instance.GameData.Tutorial.IsDecoTuTDone){
			decoTut.SetActive(false);
		}
	}

	public void OnToggleButtonClicked(){
		if(decoTweenToggle.IsShowing){
			decoTweenToggle.Hide();
			imageSymbol.sprite = upSprite;
			DecoManager.Instance.ResetCamera();
		}
		else{
			if(!DataManager.Instance.GameData.Tutorial.IsDecoTuTDone){
				decoTut.SetActive(false);
				DataManager.Instance.GameData.Tutorial.IsDecoTuTDone = true;
			}
			decoTweenToggle.Show();
			imageSymbol.sprite = downSprite;
		}
	}
}
