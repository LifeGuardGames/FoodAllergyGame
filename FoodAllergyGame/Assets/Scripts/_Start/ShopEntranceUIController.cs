using UnityEngine;
using System.Collections;

public class ShopEntranceUIController : MonoBehaviour {

	public GameObject tutorialFinger;
	public Animator animator;
	public GameObject glowSprite;

	public void Show(bool isFirstTime){
		gameObject.SetActive(true);
		if(isFirstTime){
			PlayAppearAnimation();

		}
		else{
			tutorialFinger.SetActive(false);
		}
	}

	public void Hide(){
		gameObject.SetActive(false);
	}

	void OnMouseUpAsButton(){
		StartManager.Instance.DecoButtonClicked();
	}
	
//	void OnGUI(){
//		if(GUI.Button(new Rect(100, 100, 100, 100), "play")){
//			PlayAppearAnimation();
//		}
//	}

	private void PlayAppearAnimation(){
		animator.Play("ShopAppear");
	}

	public void AppearAnimationDoneEvent(){
		// Show the tutorial finger
		tutorialFinger.SetActive(true);
	}

	public void ToggleClickable(bool isClickable){
		GetComponent<BoxCollider2D>().enabled = isClickable;
		glowSprite.SetActive(isClickable);
	}
}
