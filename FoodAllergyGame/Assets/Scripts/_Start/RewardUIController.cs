using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// UI spawning new item controller, called by RewardAnimationMeta for rewarding visually
/// Finish callback needs to be connected
/// 
/// Spawns a series of RewardItems, which take care of themselves
/// 
/// Reward Types: Customer, DecoSpecial, DecoBasic, Food, Slot, Challenge
/// </summary>
public class RewardUIController : MonoBehaviour {
	private bool isRewardClickable = false;
	public bool IsRewardClickable {
		get { return isRewardClickable; }
	}

	public TweenToggleDemux UIDemux;
	public Button dropPodButton;
	public Animator dropPodAnimator;
	public Animator capsuleAnimator;
	public TweenToggle doneButtonTween;

	public RewardInfoController rewardInfoController;

	private NotificationQueueDataReward caller;
	private List<LgTuple<AssetTypes, string>> rewardItemList;
	private int showingItemIndex;		// Used for tracking item display index

	// Fade out and spawn the drop pod
	public void Init(NotificationQueueDataReward _caller){
		caller = _caller;
		dropPodAnimator.Play("DropPodOff");
		rewardItemList = new List<LgTuple<AssetTypes, string>>();
		showingItemIndex = 0;
		isRewardClickable = false;

		Dictionary<AssetTypes, List<string>>  currentTierUnlocks = TierManager.Instance.CurrentTierUnlocks;   // Get the tier list
		foreach(KeyValuePair<AssetTypes, List<string>> hashEntry in currentTierUnlocks) {
			if(hashEntry.Key == AssetTypes.Slot) {  // Slot always has an entry, check value
				if(hashEntry.Value[0] != "0") {
					rewardItemList.Add(new LgTuple<AssetTypes, string>(hashEntry.Key, "Slot"));
				}
			}
			else {
				if(hashEntry.Value.Count > 0) {
					foreach(string itemId in hashEntry.Value) {
						rewardItemList.Add(new LgTuple<AssetTypes, string>(hashEntry.Key, itemId));
					}
				}
			}
		}
		
		// Add the starpiece custom to the reward crate
		rewardItemList.Add(new LgTuple<AssetTypes, string>(AssetTypes.StarPiece, "StarPiece"));

		UIDemux.Show();
		dropPodButton.gameObject.SetActive(false);
	}

	public void OnUITweenComplete() {
		dropPodButton.gameObject.SetActive(true);
		StartCoroutine(ShowDropPod());
	}

	private IEnumerator ShowDropPod() {
		yield return 0;
		dropPodAnimator.Play("DropPodAppearUI");
	}

	// Spawn another capsule
	public void OnDropPodClicked() {
		rewardInfoController.HideInfo();
		dropPodAnimator.SetTrigger("Open");
	}

	// When the capsule finishes tweening, play this
	public void ShowNextItemInfo() {
		rewardInfoController.ShowInfo(rewardItemList[showingItemIndex].Item1, rewardItemList[showingItemIndex].Item2);
		showingItemIndex++;
		DoneButtonToggleCheck();
    }

	// Does a check to see if it needs to show the done button
	public void DoneButtonToggleCheck() {
		if(showingItemIndex == rewardItemList.Count) {
			dropPodButton.interactable = false;
			dropPodAnimator.Play("DropPodHide");
			doneButtonTween.Show();
        }
		else {
			doneButtonTween.Hide();
		}
	}

	// Exit everything, and close out the UI, start tweening RewardItem into its respective positions
	public void OnDoneButtonClicked() {
		UIDemux.Hide();
		doneButtonTween.Hide();
		rewardInfoController.HideInfo();
	}
	
	public void OnExitTweeningComplete() {
		caller.Finish();
    }
}
