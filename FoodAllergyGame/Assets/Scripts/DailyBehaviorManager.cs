using System.Collections.Generic;
using System;
using System.Linq;

public class DailyBehaviorManager : Singleton<DailyBehaviorManager> {
	public void Init() {
		if(DateTime.Today.Day != DataManager.Instance.GameData.Daily.CurrDay) {
			if(DataManager.Instance.GameData.Daily.DailyMissions.Count < 3) {
				GetNewMission();
				DataManager.Instance.GameData.Daily.CurrDay = DateTime.Today.Day;
			}
			else {
				DataManager.Instance.GameData.Daily.DailyMissions.RemoveAt(0);
				GetNewMission();
			}
			ObtainRandomDecos();
		}
		MissionUIController.Instance.Init();
	}

	public void GetNewMission() {
		List<MissionType> possibleTypes = new List<MissionType>();
		possibleTypes = Enum.GetValues(typeof(MissionType)).Cast<MissionType>().ToList<MissionType>();
		int rand;
		do {
			rand = UnityEngine.Random.Range(0, possibleTypes.Count);
		}
		while(DataManager.Instance.GameData.Daily.GetMissionByKey(possibleTypes[rand].ToString()) != null);
		Mission newMission = new Mission();
		newMission.misType = possibleTypes[rand];
		newMission.missionKey = GetMissionKey(newMission.misType);
		newMission.amount = GetMissionAmount(newMission.misType);
		newMission.reward = GenerateReward(newMission.amount);
		DataManager.Instance.GameData.Daily.DailyMissions.Add(newMission);

	}

	public string GetMissionKey(MissionType misType) {
		if(misType != MissionType.Customer && misType != MissionType.Food) {
			return misType.ToString();
		}
		else if(misType == MissionType.Customer) {
			int rand = UnityEngine.Random.Range(0, DataManager.Instance.GameData.RestaurantEvent.CustomerList.Count);
			return DataManager.Instance.GameData.RestaurantEvent.CustomerList[rand];
		}
		else {
			int rand = UnityEngine.Random.Range(0, FoodManager.Instance.FoodStockList.Count);
			return FoodManager.Instance.FoodStockList[rand].ID;
		}
	}

	public int GetMissionAmount(MissionType misType) {
		switch(misType) {
			case MissionType.Allergy:
				return UnityEngine.Random.Range(2, 5);

			case MissionType.Customer:
				return UnityEngine.Random.Range(4, 9);

			case MissionType.Days:
				return UnityEngine.Random.Range(2, 6);

			case MissionType.Food:
				return UnityEngine.Random.Range(5, 14);

			case MissionType.Walk:
				return UnityEngine.Random.Range(20, 100);

			default:
				return UnityEngine.Random.Range(2, 6);
		}
	}

	public int GenerateReward(int amount) {
		if(amount < 10) {
			return amount * 70;
		}
		else {
			return amount * 10;
		}
	}

	public void ObtainRandomDecos() {
		GetRandomTablesForStore(DataLoaderDecoItem.GetUnlockedDecoListByTabType(TierManager.Instance.CurrentTier, DecoTabTypes.Table));
		GetRandomFloorsForStore(DataLoaderDecoItem.GetUnlockedDecoListByTabType(TierManager.Instance.CurrentTier, DecoTabTypes.Floor));
		GetRandomKitchensForStore(DataLoaderDecoItem.GetUnlockedDecoListByTabType(TierManager.Instance.CurrentTier, DecoTabTypes.Kitchen));
		GetRandomSpecialItem(DataLoaderDecoItem.GetUnlockedDecoList(TierManager.Instance.CurrentTier));
	}

	public void GetRandomTablesForStore(List<string> decoList) {
		while(DataManager.Instance.GameData.Daily.RotTables.Count < 3) {
			int rand = UnityEngine.Random.Range(0, decoList.Count);
			if(!DataManager.Instance.GameData.Daily.RotTables.Contains(decoList[rand])) {
				DataManager.Instance.GameData.Daily.RotTables.Add(decoList[rand]);
			}
		}
	}

	public void GetRandomFloorsForStore(List<string> decoList) {
		while(DataManager.Instance.GameData.Daily.RotFloors.Count < 3) {
			int rand = UnityEngine.Random.Range(0, decoList.Count);
			if(!DataManager.Instance.GameData.Daily.RotFloors.Contains(decoList[rand])) {
				DataManager.Instance.GameData.Daily.RotFloors.Add(decoList[rand]);
			}
		}
	}

	public void GetRandomKitchensForStore(List<string> decoList) {
		while(DataManager.Instance.GameData.Daily.RotKitchen.Count < 3) {
			int rand = UnityEngine.Random.Range(0, decoList.Count);
			if(!DataManager.Instance.GameData.Daily.RotKitchen.Contains(decoList[rand])) {
				DataManager.Instance.GameData.Daily.RotKitchen.Add(decoList[rand]);
			}
		}
	}

	public void GetRandomSpecialItem(List<string> decoList) {
		while(DataManager.Instance.GameData.Daily.DailyRandomDeco == "") {
			int rand = UnityEngine.Random.Range(0, decoList.Count);
			if(!DataManager.Instance.GameData.Decoration.BoughtDeco.ContainsKey(decoList[rand])) {
				DataManager.Instance.GameData.Daily.DailyRandomDeco = decoList[rand];
			}
		}
	}
}
