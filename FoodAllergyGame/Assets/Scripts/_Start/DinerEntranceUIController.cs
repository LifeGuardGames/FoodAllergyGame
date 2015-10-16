using UnityEngine;
using System.Collections;

public class DinerEntranceUIController : MonoBehaviour {

	public GameObject glowSprite;

	void OnMouseUpAsButton(){
		StartManager.Instance.OnPlayButtonClicked();
	}

	public void ToggleClickable(bool isClickable){
		GetComponent<BoxCollider2D>().enabled = isClickable;
		glowSprite.SetActive(isClickable);
	}
}
