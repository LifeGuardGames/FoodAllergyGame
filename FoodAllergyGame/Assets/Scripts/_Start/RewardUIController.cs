using UnityEngine;
using UnityEngine.UI;
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

	public TweenToggleDemux UIDemux;
	public Animator dropPodAnimator;
	public AlphaTweenToggle fadeToggle;
	public TweenToggle doneButtonTween;
	public GameObject rewardItemPrefab;
	public GameObject rewardItemsParent;
	public float distanceBetweenItems = 400f;
	public GameObject leftArrow;
	public GameObject rightArrow;
	public Text textTitle;
	public Text textDescription;

	private NotificationQueueDataReward caller;
	private Dictionary<AssetTypes, List<string>> currentTierUnlocks;
	private List<RewardItem> rewardItemList;
	private int internalSpawnIndex;		// Used for spawning in sequence
	private int showingItemIndex;		// Used for tracking item display index

	// Fade out and spawn the drop pod
	public void Init(NotificationQueueDataReward _caller){
		caller = _caller;
		dropPodAnimator.Play("DropPodHide");
		rewardItemList = new List<RewardItem>();
		internalSpawnIndex = 0;
		showingItemIndex = 0;
		isRewardClickable = false;
		leftArrow.SetActive(false);
		rightArrow.SetActive(false);

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

		UIDemux.Show();
    }

	private void PopulateRewardItemInList(AssetTypes assetType, string itemID) {
		// Spawn the actual object
		GameObject go = GameObjectUtils.AddChildGUI(rewardItemsParent, rewardItemPrefab);
		go.transform.localPosition = new Vector3(internalSpawnIndex * distanceBetweenItems, 0f);
		RewardItem itemScript = go.GetComponent<RewardItem>();
		itemScript.InitItem(assetType, itemID, this);

		// Add to internal list
		rewardItemList.Add(itemScript);

		internalSpawnIndex++;
    }

	public void OnUITweenComplete() {
		dropPodAnimator.Play("DropPodAppearUI");
	}

	// Open the drop pod and pop out the RewardItems
	public void OnDropPodClicked() {
		dropPodAnimator.Play("DropPodOpenUI");
	}

	public void OnDropPodOpenAnimationDone() {
		StartCoroutine(StartItemsAppear(0));
	}

	// Once all the objects have been initialized, start the showing process
	private IEnumerator StartItemsAppear(int itemIndex) {
		yield return new WaitForSeconds(0.2f);
		ChangeSize(rewardItemList[itemIndex].gameObject, itemIndex == 0 ? true : false);
		rewardItemList[itemIndex].AnimationAppear();

		// Detect if it is the last element
		if(itemIndex >= rewardItemList.Count - 1) {
			isRewardClickable = true;   // Allow clicking from here on
			RefreshArrowStatus();
        }
		else {
			itemIndex++;
			StartCoroutine(StartItemsAppear(itemIndex));
		}
	}

	public bool IsCapsuleClickable(RewardItem rewardItemScript) {
		if(rewardItemScript == rewardItemList[showingItemIndex]) {
			return true;
		}
		else {
			return false;
		}
	}

	// Does a check to see if it needs to show the done button
	public void DoneButtonToggleCheck() {
		bool isShow = true;
		foreach(RewardItem item in rewardItemList) {
			if(!item.IsOpened) {
				isShow = false;
				break;
            }
		}
		if(isShow) {
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

	public void OnNextArrowClicked(bool isRight) {
		ChangeSize(rewardItemList[showingItemIndex].gameObject, false);     // Minimize the previous item

		if(isRight) {
			showingItemIndex++;
        }
		else {
			showingItemIndex--;
        }
		LeanTween.moveLocalX(rewardItemsParent, -distanceBetweenItems * showingItemIndex, 0.2f)
			.setEase(LeanTweenType.easeInOutQuad);

		ChangeSize(rewardItemList[showingItemIndex].gameObject, true);     // Maximize the current item
		rewardItemList[showingItemIndex].ShowDescription();
        RefreshArrowStatus();
    }

	// Info pumped through the rewardItem, show none if null
	public void ShowInfo(string titleKey, string descriptionKey) {
		textTitle.text = !string.IsNullOrEmpty(titleKey) ? LocalizationText.GetText(titleKey) : "";
		textDescription.text = !string.IsNullOrEmpty(descriptionKey) ? LocalizationText.GetText(descriptionKey) : "";
	}

	// Used to define which object is currently in focus
	public void ChangeSize(GameObject go, bool isLarge) {
		if(isLarge) {
			LeanTween.scale(go, Vector3.one, 0.2f).setEase(LeanTweenType.easeOutBack);
		}
		else {
			LeanTween.scale(go, new Vector3(0.6f, 0.6f, 1f), 0.2f);
		}
	}

	// Refresh to see if the arrows needs to be shown
	private void RefreshArrowStatus() {
		leftArrow.SetActive(showingItemIndex <= 0 ? false : true);
		rightArrow.SetActive(showingItemIndex >= rewardItemList.Count - 1 ? false : true);
	}
}
