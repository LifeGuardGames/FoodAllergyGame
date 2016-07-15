using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventPopupController : MonoBehaviour {

	public TweenToggleDemux tweenDemux;
	public Text eventTitle;
	public Text eventDescription;
	public Text bonusDescription;

	public void Init(ImmutableDataEvents eventData){
		if(!string.IsNullOrEmpty(DataManager.Instance.GetBonus())) {
			bonusDescription.text = LocalizationText.GetText(DataManager.Instance.GetBonus());
		}
		if(!string.IsNullOrEmpty(eventData.EventDescription)){
			eventTitle.text = LocalizationText.GetText(eventData.EventTitle);
			eventDescription.text = LocalizationText.GetText(eventData.EventDescription);
			StartCoroutine(ShowAfterFrame());
		}
	}

	// Just making sure everything is initialized before tween call
	private IEnumerator ShowAfterFrame() {
		yield return 0;
		tweenDemux.Show();
    }
	
	public void OnOkayButton(){
		tweenDemux.Hide();
	}
}
