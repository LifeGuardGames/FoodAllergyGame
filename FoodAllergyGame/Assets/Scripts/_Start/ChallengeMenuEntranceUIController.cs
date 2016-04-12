using UnityEngine;

public class ChallengeMenuEntranceUIController : MonoBehaviour {

	public Animator challengeMenuEntranceAnimator;

	void Start() {
		// Show the challenge menu entrance
		if(TierManager.Instance.CurrentTier >= 3) {
			bool isFirstTimeChallenge = DataManager.Instance.GameData.Challenge.IsFirstTimeChallengeEntrance;
			Show(isFirstTimeChallenge);
		}
		else {
			Hide();
		}
	}

	public void Show(bool isFirstTime) {
		gameObject.SetActive(true);
		ToggleClickable(true);
		if(isFirstTime) {
			StartManager.Instance.DinerEntranceUIController.ToggleClickable(false);
			StartManager.Instance.ShopEntranceUIController.ToggleClickable(false);
			challengeMenuEntranceAnimator.Play("PirateShipAppear");
        }
		else {
			challengeMenuEntranceAnimator.Play("PirateShipHover");
		}
	}

	public void Hide() {
		gameObject.SetActive(false);
	}

	void OnMouseUpAsButton() {
		StartManager.Instance.ChallengeMenuButtonClicked();
	}

	//	void OnGUI(){
	//		if(GUI.Button(new Rect(100, 100, 100, 100), "play")){
	//			PlayAppearAnimation();
	//		}
	//	}

	public void AppearAnimationDoneEvent() {
		ToggleClickable(true); // NOTE: Don't show diner
	}

	public void ToggleClickable(bool isClickable) {
		GetComponent<BoxCollider2D>().enabled = isClickable;
		challengeMenuEntranceAnimator.SetBool("IsClickable", isClickable);
    }
}
