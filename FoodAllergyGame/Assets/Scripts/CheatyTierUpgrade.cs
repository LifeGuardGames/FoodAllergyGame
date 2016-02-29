using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CheatyTierUpgrade : MonoBehaviour {
	/// <summary>
	/// checks the current tier and assigns the total cash of the next tier up 
	/// we set the total cash as to not skip any tiers
	/// </summary>
	public void UpgradeTier() {
		switch(TierManager.Instance.CurrentTier) {
			case 0:
				DataManager.Instance.GameData.Cash.TotalCash = 850;
				break;
			case 1:
				DataManager.Instance.GameData.Cash.TotalCash = 2450;
				break;
			case 2:
				DataManager.Instance.GameData.Cash.TotalCash = 4550;
				break;
			case 3:
				DataManager.Instance.GameData.Cash.TotalCash = 7300;
				break;
			case 4:
				DataManager.Instance.GameData.Cash.TotalCash = 10050;
				break;
			case 5:
				DataManager.Instance.GameData.Cash.TotalCash = 12800;
				break;
			case 6:
				DataManager.Instance.GameData.Cash.TotalCash = 15550;
				break;
			case 7:
				DataManager.Instance.GameData.Cash.TotalCash = 18300;
				break;
			case 8:
				DataManager.Instance.GameData.Cash.TotalCash = 21050;
				break;
			case 9:
				DataManager.Instance.GameData.Cash.TotalCash = 23800;
				break;
			case 10:
				DataManager.Instance.GameData.Cash.TotalCash = 26550;
				break;
			case 11:
				DataManager.Instance.GameData.Cash.TotalCash = 29300;
				break;
			case 12:
				DataManager.Instance.GameData.Cash.TotalCash = 32050;
				break;
			case 13:
				DataManager.Instance.GameData.Cash.TotalCash = 35800;
				break;
			case 14:
				DataManager.Instance.GameData.Cash.TotalCash = 37550;
				break;
			case 15:
				DataManager.Instance.GameData.Cash.TotalCash = 40300;
				break;
			case 16:
				DataManager.Instance.GameData.Cash.TotalCash = 43050;
				break;
			case 17:
				DataManager.Instance.GameData.Cash.TotalCash = 45800;
				break;
			default:
				break;
		}
		SceneManager.LoadScene(SceneUtils.START);
			
	}
}
