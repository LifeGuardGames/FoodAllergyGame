using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MissionUIController : Singleton<MissionUIController> {
	public TweenToggleDemux demux;
	public List<GameObject> missionSlots;
	public GameObject missionUIPrefab;
	public ParticleSystem coinParticle;
	public Text nextDayMissionText;
	public Animation missionCompleteSymbolAnimation;

	public void Init() {
		// First clear all existing missions
        foreach(GameObject go in missionSlots) {
			foreach(Transform t in go.transform) {
				Destroy(t.gameObject);
			}
		}
		missionCompleteSymbolAnimation.Stop();
        for(int i = 0; i < DataManager.Instance.GameData.Daily.DailyMissions.Count; i++ ){
			GameObject go = GameObjectUtils.AddChildGUI(missionSlots[i], missionUIPrefab);

			// Check to see if any missions are complete and notify
			if(DataManager.Instance.GameData.Daily.DailyMissions[i].progress >= DataManager.Instance.GameData.Daily.DailyMissions[i].amount) {
				missionCompleteSymbolAnimation.Play();
            }
			go.GetComponent<MissionUI>().Init(DataManager.Instance.GameData.Daily.DailyMissions[i]);
		}
		nextDayMissionText.gameObject.SetActive(DataManager.Instance.GameData.Daily.DailyMissions.Count == 0);
	}

	public void CheckMissionComplete(string misKey) {
		Mission mis = DataManager.Instance.GameData.Daily.GetMissionByKey(misKey);
		if(mis.progress >= mis.amount) {
			OnComplete(mis);
		}
	}

	//void OnGUI() {
	//	if(GUI.Button(new Rect(100, 100, 100, 100), "Finish Missions")) {
	//		foreach(Mission mission in DataManager.Instance.GameData.Daily.DailyMissions) {
	//			mission.progress = mission.amount;
	//		}
	//		Init();
	//	}
	//}

	public void OnComplete(Mission mis) {
		//play animation or something
		foreach(GameObject missionUi in missionSlots) {
			if(missionUi.transform.childCount > 0) {
				if(missionUi.GetComponentInChildren<MissionUI>().mission.missionKey == mis.missionKey) {
					CashManager.Instance.OverrideCurrentCash(mis.reward, missionUi.transform.position);

					coinParticle.transform.position = missionUi.transform.position;
					coinParticle.Play();
					AudioManager.Instance.PlayClip("TrophyGoldGet");

					Destroy(missionUi.transform.GetChild(0).gameObject);
				}
				DataManager.Instance.GameData.Daily.DailyMissions.Remove(mis);
			}
		}
		missionCompleteSymbolAnimation.Stop();
        nextDayMissionText.gameObject.SetActive(DataManager.Instance.GameData.Daily.DailyMissions.Count == 0);
	}

	public void OnShowButton() {
		StartManager.Instance.TurnOffEntrances();
		demux.Show();
	}

	public void OnCloseButton() {
		demux.Hide();
		StartManager.Instance.TurnOnEntrances();
	}
}
