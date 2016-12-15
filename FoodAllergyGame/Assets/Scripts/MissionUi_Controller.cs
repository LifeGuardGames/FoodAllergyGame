using UnityEngine;
using System.Collections.Generic;

public class MissionUIController : Singleton<MissionUIController> {

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
		
		//play animation or something
		foreach(GameObject missionUi in missionSlots) {
			if(missionUi.transform.childCount > 0) {
				Debug.Log(missionUi.GetComponentInChildren<MissionUI>().mission);
				if(missionUi.GetComponentInChildren<MissionUI>().mission.missionKey == mis.missionKey) {
					CashManager.Instance.OverrideCurrentCash(mis.reward, missionUi.transform.position);
					Destroy(missionUi.transform.GetChild(0).gameObject);
				}
				DataManager.Instance.GameData.Daily.DailyMissions.Remove(mis);
			}
		}
	}

	public void OnShowButton() {
		StartManager.Instance.TurnOffEntrances();
		this.GetComponent<PositionTweenToggle>().Show();
	}

	public void OnCloseButton() {
		this.GetComponent<PositionTweenToggle>().Hide();
		StartManager.Instance.TurnOnEntrances();
	}
}
