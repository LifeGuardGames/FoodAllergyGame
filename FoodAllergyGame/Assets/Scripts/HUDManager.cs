using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {
	public TweenToggle coinTween;
	public TweenToggle tierTween;

	void Start(){
		ShowHUD();
	}

	public void ShowHUD(){
		StartCoroutine(ToggleHUDInScene(true));
	}

	public void HideHUD(){
		StartCoroutine(ToggleHUDInScene(false));
	}

	private IEnumerator ToggleHUDInScene(bool isShow){
		yield return 0;
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
			coinTween.Show();
		}
		else{
			coinTween.Hide();
		}
	}

	private void ToggleTier(bool isShow){
		if(isShow){
			tierTween.Show();
		}
		else{
			tierTween.Hide();
		}
	}
}
