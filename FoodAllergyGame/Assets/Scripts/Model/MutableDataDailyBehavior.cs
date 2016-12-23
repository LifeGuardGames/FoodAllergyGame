using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MutableDataDailyBehavior{

	public List<Mission> DailyMissions;
	public List<string> RotTables;
	public List<string> RotFloors;
	public List<string> RotKitchen;
	public int CurrDay;
	public string SpeciDeco;

	public MutableDataDailyBehavior() {
		DailyMissions = new List<Mission>();
		RotTables = new List<string>();
		RotKitchen = new List<string>();
		RotFloors = new List<string>();
		CurrDay = 0;
		SpeciDeco = "";
	}
	public Mission GetMissionByKey(string key) {
		foreach(Mission mis in DailyMissions) {
			if(mis.missionKey == key) {
				return mis;
			}
		}
		// should be unreachable
		return null;
	}
}
