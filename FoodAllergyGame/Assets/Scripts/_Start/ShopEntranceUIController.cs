using UnityEngine;

public class ShopEntranceUIController : MonoBehaviour {

	public GameObject tutorialFinger;
	public Animator decoEntranceAnimator;
	public GameObject newSprite;

	void Start() {
		if(DataManager.Instance.GameData.Tutorial.IsSpeDecoTutDone || TierManager.Instance.CurrentTier >= 3) {
			bool isFirstTimeShop = DataManager.Instance.GameData.Decoration.IsFirstTimeEntrance;
			StartManager.Instance.isShopAppearHideDinerOverride = isFirstTimeShop;
			Show(isFirstTimeShop);
		}
		else {
			Hide();
		}
	}

	public void Show(bool isFirstTime){
		gameObject.SetActive(true);
		if(isFirstTime){
			ToggleClickable(true);
			StartManager.Instance.DinerEntranceUIController.ToggleClickable(false);
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
		decoEntranceAnimator.Play("ShopAppear");
	}

	public void AppearAnimationStartEvent() {
	}

	public void AppearAnimationDoneEvent(){
		Debug.Log("CALL ME");
		// Show the tutorial finger
		tutorialFinger.SetActive(true);
		ToggleClickable(true); // NOTE: Don't show diner
	}

	public void ToggleClickable(bool isClickable){
		GetComponent<BoxCollider2D>().enabled = isClickable;
		decoEntranceAnimator.SetBool("IsClickable", isClickable);
	}

	public void ToggleShowNewItems(bool newItemsExists) {
		StartManager.Instance.shopEntranceUIController.newSprite.SetActive(newItemsExists);
	}
}
