using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MissionUI : MonoBehaviour {

	public Text titleText;
	public Text missionDescription;
	public Text missionProgress;
	public Text missionAmount;
	public Text reward;
	public Mission mission;

	public void Init(Mission mis) {
		titleText.text = mis.misType.ToString();
		missionDescription.GetComponent<Localize>().key = mis.missionKey;
		missionDescription.GetComponent<Localize>().LocalizeText();
		missionProgress.text = mis.progress.ToString();
		missionAmount.text = mis.amount.ToString();
		reward.text = mis.reward.ToString();
		mission = mis;
	}
}
