using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MissionUiController : Singleton<MissionUiController> {

	public List<GameObject> missionSlots;
	public GameObject missionUIPrefab;

	public void Init() {
		for(int i = 0; i < DataManager.Instance.GameData.Daily.DailyMissions.Count; i++ ){
			GameObject go = GameObjectUtils.AddChildGUI(missionSlots[i], missionUIPrefab);
			go.GetComponent<MissionUI>().Init(DataManager.Instance.GameData.Daily.DailyMissions[i]);
		}
	}

	public void CheckMissionComplete(string misKey) {
		Mission mis = DataManager.Instance.GameData.Daily.GetMissionByKey(misKey);
		if(mis.progress >= mis.amount) {
			OnComplete(mis);
		}
	}

	public void OnComplete(Mission mis) {
		CashManager.Instance.OverrideCurrentCash(mis.reward);
		//play animation or something
		foreach(GameObject missionUi in missionSlots) {
			if(missionUi.GetComponent<MissionUI>().mission.missionKey == mis.missionKey) {
				Destroy(missionUi);
			}
			DataManager.Instance.GameData.Daily.DailyMissions.Remove(mis);
		}
	}

	public void OnShowButton() {
		this.GetComponent<PositionTweenToggle>().Show();
	}

	public void OnCloseButton() {
		this.GetComponent<PositionTweenToggle>().Hide();
	}
}
