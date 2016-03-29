using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	public Animator dropPodAnimator;
	public AlphaTweenToggle fadeToggle;
	public TweenToggle doneButtonTween;
	public GameObject rewardItemPrefab;
	public GameObject rewardItemsParent;
	public float distanceBetweenItems = 400f;

	private NotificationQueueDataReward caller;
	private Dictionary<AssetTypes, List<string>> currentTierUnlocks;
	private List<RewardItem> rewardItemList;
	private int internalSpawnIndex;

	// Fade out and spawn the drop pod
	public void Init(NotificationQueueDataReward _caller){
		caller = _caller;
		dropPodAnimator.Play("DropPodAppearUI");
		rewardItemList = new List<RewardItem>();
		internalSpawnIndex = 0;
		isRewardClickable = false;

		currentTierUnlocks = TierManager.Instance.CurrentTierUnlocks;   // Get the tier list
		foreach(KeyValuePair<AssetTypes, List<string>> hashEntry in currentTierUnlocks) {
			if(hashEntry.Key == AssetTypes.Slot) {  // Slot always has an entry, check value
				if(hashEntry.Value[0] != "0") {
					PopulateRewardItemInList(AssetTypes.Slot, null);
                }
			}
			else {
				if(hashEntry.Value.Count > 0) {
					foreach(string s in hashEntry.Value) {
						PopulateRewardItemInList(hashEntry.Key, s);
					}
				}
			}
		}

		StartCoroutine(StartItemsAppear(0));
	}

	private void PopulateRewardItemInList(AssetTypes assetType, string itemID) {
		// Spawn the actual object
		GameObject go = GameObjectUtils.AddChildGUI(rewardItemsParent, rewardItemPrefab);
		go.transform.localPosition = new Vector3(internalSpawnIndex * distanceBetweenItems, 0f);
		RewardItem itemScript = go.GetComponent<RewardItem>();
		itemScript.InitItem(assetType, itemID);

		// Add to internal list
		rewardItemList.Add(itemScript);

		internalSpawnIndex++;
    }

	// Once all the objects have been initialized, start the showing process
	private IEnumerator StartItemsAppear(int itemIndex) {
		yield return new WaitForSeconds(0.2f);

		// Detect if it is the last element
		if(itemIndex >= rewardItemList.Count - 1) {
			isRewardClickable = true;	// Allow clicking from here on
        }
		else {
			itemIndex++;
			StartCoroutine(StartItemsAppear(itemIndex));
		}

	}

	// Open the drop pod and pop out the RewardItems
	public void OnDropPodClicked() {
		dropPodAnimator.Play("DropPodOpenUI");
	}

	// Show the doneButton when all the RewardItems are opened
	public void DoneButtonToggle(bool isShow) {
		if(isShow) {
			doneButtonTween.Show();
		}
		else {
			doneButtonTween.Hide();
		}
	}

	// Exit everything, and close out the UI, start tweening  RewardUI into its respective positions
	public void OnDoneButtonClicked() {

	}

	// Making sure to call finish
	public void OnExitTweeningComplete() {
		caller.Finish();
	}

	public void FadeToggle(bool isFade) {
		if(isFade) {
			fadeToggle.Show();
		}
		else {
			fadeToggle.Hide();
		}
	}
}
