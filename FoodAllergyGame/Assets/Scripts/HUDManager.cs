using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {
	public TweenToggleDemux coinDemux;
	public TweenToggleDemux tierDemux;

	void Start(){
		ShowHUD();
	}

	public void ShowHUD(){
		ToggleHUDInScene(true);
	}

	public void HideHUD(){
		ToggleHUDInScene(false);
	}

	private void ToggleHUDInScene(bool isShow){
		// Show different panels based on what scene it is
		switch(Application.loadedLevelName){
		case SceneUtils.START:
			ToggleCoin(isShow);
			ToggleTier(isShow);
			break;
		case SceneUtils.MENUPLANNING:
			ToggleCoin(isShow);
			ToggleTier(false);
			break;
		case SceneUtils.RESTAURANT:
			// Nothing to do here, doesnt exist
			break;
		case SceneUtils.DECO:
			ToggleCoin(isShow);
			ToggleTier(false);
			break;
		default:
			Debug.LogWarning("Bad scene string detected");
			break;
		}
	}

	private void ToggleCoin(bool isShow){
		if(isShow){
			coinDemux.Show();
		}
		else{
			coinDemux.Hide();
		}
	}

	private void ToggleTier(bool isShow){
		if(isShow){
			tierDemux.Show();
		}
		else{
			tierDemux.Hide();
		}
	}
}
