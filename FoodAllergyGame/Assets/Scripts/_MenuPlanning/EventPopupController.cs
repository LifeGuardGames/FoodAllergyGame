using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventPopupController : MonoBehaviour {

	public TweenToggle eventPanelTween;
	public Text eventTitle;
	public Text eventDescription;

	public void Init(ImmutableDataEvents eventData){
		if(!string.IsNullOrEmpty(eventData.EventDescription)){
			eventTitle.text = LocalizationText.GetText(eventData.ID);
			Debug.Log(eventData.EventDescription);
			eventDescription.text = LocalizationText.GetText(eventData.EventDescription);
			eventPanelTween.Show();
		}
	}
	
	public void OnOkayButton(){
		eventPanelTween.Hide();
	}
}
