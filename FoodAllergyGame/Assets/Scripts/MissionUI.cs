﻿using UnityEngine;
using UnityEngine.UI;

public class MissionUI : MonoBehaviour {
	public Text titleText;
	public Text missionDescription;
	public Text missionProgress;
	public Text missionAmount;
	public Text reward;
	public Mission mission;
	public Button butComp;

	public void Init(Mission mis) {
		titleText.text = mis.misType.ToString();
		missionDescription.GetComponent<Localize>().key = mis.missionKey + "mdesc";
		missionDescription.GetComponent<Localize>().LocalizeText(mis.amount);
        missionProgress.text = mis.progress.ToString();
		missionDescription.GetComponent<Localize>().key = "";
        missionAmount.text = mis.amount.ToString();
		reward.text = mis.reward.ToString();
		mission = mis;
		butComp.onClick.AddListener(() => MissionUIController.Instance.CheckMissionComplete(mis.missionKey));
	}

}
