using UnityEngine;

public class DinerEntranceUIController : MonoBehaviour {

	public Animator dinerEntranceAnimator;

	void OnMouseUpAsButton(){
		StartManager.Instance.OnPlayButtonClicked();
	}

	public void ToggleClickable(bool isClickable){
		GetComponent<BoxCollider2D>().enabled = isClickable;
		dinerEntranceAnimator.SetBool("IsClickable", isClickable);
    }
}
