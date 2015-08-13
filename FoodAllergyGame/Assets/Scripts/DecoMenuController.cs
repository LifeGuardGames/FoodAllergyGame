using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DecoMenuController : MonoBehaviour {

	public TweenToggle decoTweenToggle;
	public bool isOpen;

	void Start(){
		this.GetComponent<Button>().onClick.AddListener(this.CheckUI);
	}

	public void ShowUi(){
		decoTweenToggle.Show();
	}

	public void HideUi(){
		decoTweenToggle.Hide();
	}

	public void CheckUI(){
		if(isOpen){
			isOpen = false;
			HideUi();
		}
		else{
			isOpen = true;
			ShowUi();
		}
	}
}
