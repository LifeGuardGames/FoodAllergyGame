﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Terminology:
///		MapComet - The comet object
///		MapNodeMid - Constellation stars, the middle one being the mini-reward
///		MapNodeBaseStart - Node at bottom which is the start position
///		MapNodeBaseEnd - Node at the top which is the end position
///		MapTrailSegment - The trail between two nodes
///		MapTrailTotal - All the trails added together, length = sum of all trailSegments
/// </summary>
public class MapUIController : MonoBehaviour {
	public TweenToggleDemux demux;
	public GameObject mapParent;
	public CanvasScaler canvasScaler;
	public GameObject nodeStartPrefab;
	public GameObject nodeMidPrefab;
	public GameObject nodeEndPrefab;
	public GameObject trailSegmentPrefab;
	public GameObject cometPrefab;
	private GameObject comet;
	private bool move;

	private float segmentHeight;					// Y component from one node to the other
	private int numberNodesBetweenStartEnd = 3;
	private List<Vector2> nodePositionList;         // Positions of the nodes, some random, some static
	private List<Transform> auxNodesList;           // Keep track of all nodes spawned, used for line
	private List<MapTrailSegment> segmentList;

	private float percentPerSegment;
	public float PercentPerSegment {
		get { return percentPerSegment; }
	}

	private NotificationQueueData notif;				// Used for tracking end notification
	private int newTotalCash;
	private ImmutableDataTiers startTier;
	private ImmutableDataTiers endTier;
	private float tierProgressPercentage;

	private MapCometController cometController;
	private Dictionary<AssetTypes, List<string>> unlocksHash;

	public void InitializeAndShow(int _oldTotalCash, int _newTotalCash, NotificationQueueData _notif) {
		// Initialize variables
		nodePositionList = new List<Vector2>();
		auxNodesList = new List<Transform>();
		segmentList = new List<MapTrailSegment>();
		segmentHeight = (canvasScaler.referenceResolution.y - 100) / (numberNodesBetweenStartEnd + 1);
		notif = _notif;
		newTotalCash = _newTotalCash;

		// Initialize all the node positions
		//TODO Fix seed here
		//Random.seed = 42;                           // Repeatable random numbers
		for(int i = 0; i < (DataLoaderTiers.GetTotalTierCount() * (numberNodesBetweenStartEnd + 1)) + 1; i++) {
			// Determine if we are dealing with MapNodeMid or MapNodeBase
			int modulo = i % (numberNodesBetweenStartEnd + 1);
			if(modulo == 0) {						// MapNodeBase
				nodePositionList.Add(new Vector2(0f, 0f));
			}
			else {                                  // MapNodeMid
				// Determine if Node is on left or right side of screen center
				float xDisplacement = UnityEngine.Random.Range(100, 401);
				if(i % 2 == 0) {
                    xDisplacement *= -1;
				}
                nodePositionList.Add(new Vector2(xDisplacement, segmentHeight * modulo));
			}
		}
		
		startTier = DataLoaderTiers.GetDataFromTier(DataLoaderTiers.GetTierFromCash(_oldTotalCash));
        GameObject nodeBaseStart = GameObjectUtils.AddChildGUI(mapParent, nodeStartPrefab);
		nodeBaseStart.GetComponent<MapNodeBase>().Init(startTier, true, canvasScaler);
		auxNodesList.Add(nodeBaseStart.transform);

		// See how many capsules we will collect this tier
		unlocksHash = TierManager.Instance.GetAllUnlocksAtTier(startTier.TierNumber);
		int rewardCount = 0;
		rewardCount += unlocksHash[AssetTypes.Challenge].Count;
		rewardCount += unlocksHash[AssetTypes.Customer].Count;
		rewardCount += unlocksHash[AssetTypes.DecoBasic].Count;
		rewardCount += unlocksHash[AssetTypes.DecoSpecial].Count;
		rewardCount += unlocksHash[AssetTypes.Food].Count;
		rewardCount += unlocksHash[AssetTypes.Slot][0] == "0" ? 0 : 1;
		int moduloReward = rewardCount % numberNodesBetweenStartEnd;	// Used for adding extra capsule if needed

		for(int i = 1; i <= numberNodesBetweenStartEnd; i++) {
			int nodeRewardCount = (rewardCount / numberNodesBetweenStartEnd);
			if(moduloReward > 0) {
				nodeRewardCount++;
				moduloReward--;
			}
            GameObject nodeMid = GameObjectUtils.AddChildGUI(mapParent, nodeMidPrefab);
			nodeMid.GetComponent<MapNodeMid>().Init(startTier, i, nodePositionList, nodeRewardCount);
			auxNodesList.Add(nodeMid.transform);
		}

		endTier = DataLoaderTiers.GetNextTier(startTier);
		GameObject nodeBaseEnd = GameObjectUtils.AddChildGUI(mapParent, nodeEndPrefab);
		nodeBaseEnd.GetComponent<MapNodeBase>().Init(endTier, false, canvasScaler);
		auxNodesList.Add(nodeBaseEnd.transform);

		// Connect all the nodes using TrailSegments
		percentPerSegment = 1f / (numberNodesBetweenStartEnd + 1);
        for(int i = 0; i < auxNodesList.Count - 1; i++) {
			GameObject segment = GameObjectUtils.AddChildGUI(mapParent, trailSegmentPrefab);
			segment.transform.SetSiblingIndex(0);
			MapTrailSegment segmentScript = segment.GetComponent<MapTrailSegment>();
			segmentScript.Init(auxNodesList[i], auxNodesList[i + 1], i + 1, this);
			segmentList.Add(segmentScript);
        }

		//Calculate current percentage and update trails
		if(endTier != null) {
			tierProgressPercentage = GetPercentageAtTierRange(startTier, endTier, _oldTotalCash);
		}
		else {  // Reached last tier
			tierProgressPercentage = 1f;
        }
		UpdateTrailPercentage(tierProgressPercentage, true);
		if(DataManager.Instance.GameData.DayTracker.CurrentTempoGoal != "") {
			// Place the comet and initialize all the rewards
			if(startTier.TierNumber == DataLoaderTempoGoals.GetData(DataManager.Instance.GameData.DayTracker.CurrentTempoGoal).Tier) {
				PlaceComet(TempoGoalManager.Instance.GetPercentageComet());
			}
		}
        demux.Show();
    }

	public void OnShowComplete() {
		if(endTier != null) {
			StartAnimation();
		}
		else {	// Max tier, just close after a while
			StartCoroutine(MaxTierClose());
		}
	}

	private IEnumerator MaxTierClose() {
		yield return new WaitForSeconds(1f);
		demux.Hide();
		notif.Finish();
	}

	public void UpdateTrailPercentage(float trailPercentage, bool isSetup) {
		foreach(MapTrailSegment segment in segmentList) {
			segment.UpdateSegmentPercentage(trailPercentage, isSetup);
		}
	}

	/// <summary>
	/// Given start and end tiers, and a cash value in between, what percentage am I at?
	/// </summary>
	private float GetPercentageAtTierRange(ImmutableDataTiers _startTier, ImmutableDataTiers _endTier, int totalCash) {
		int totalCashNeededForTier = _endTier.CashCutoffFloor - _startTier.CashCutoffFloor;
		float aux = (float)(totalCash - _startTier.CashCutoffFloor) / totalCashNeededForTier;
		return aux;
	}

	public void PlaceComet(float percentage) {
		// Determine which segment this goes into
		foreach(MapTrailSegment segment in segmentList) {
			float? cometSegmentXPosition = segment.GetXPositionOfSegmentPercentage(percentage);
            if(cometSegmentXPosition != null) {
				comet = GameObjectUtils.AddChildGUI(segment.gameObject, cometPrefab);
				cometController = comet.GetComponent<MapCometController>();
                comet.transform.localPosition = new Vector2(cometSegmentXPosition.GetValueOrDefault(), 0f);
				break;
			}
		}
	}

	#region Animation functions
	public void StartAnimation() {
		float finishPercentage;

		// First check if we are passed into a new tier
		int currentTierNum = DataLoaderTiers.GetTierFromCash(newTotalCash);
        if(currentTierNum == startTier.TierNumber) {		// Same tier, do regular tweening
			finishPercentage = GetPercentageAtTierRange(startTier, endTier, newTotalCash);
		}
		else {                                              // Tiered up, just show progress 100%
			finishPercentage = 1f;
		}
		//	Debug.Log("finishpercentage " + tierProgressPercentage + " " + finishPercentage);
		AudioManager.Instance.PlayClip("MapShipTravel");
		LeanTween.value(gameObject, TweenValuePercentage, tierProgressPercentage, finishPercentage, 2f)
			.setEase(LeanTweenType.easeInOutQuad)
			.setDelay(1f)
			.setOnComplete(TweenCompleted);
	}

	private void TweenValuePercentage(float val) {
		// The trails themselves will update the node animations
		if(DataManager.Instance.GameData.DayTracker.CurrentTempoGoal != "") {
			if(val >= TempoGoalManager.Instance.GetPercentageComet() &&
				!DataManager.Instance.GameData.DayTracker.HasCompletedGoalThisTier &&
				startTier.TierNumber != 0 &&
				!move) {

				SmashComet();
			}
			else {
				UpdateTrailPercentage(val, false);
			}
		}
		else {
			UpdateTrailPercentage(val, false);
		}
	}

	private void SmashComet() {
	//	Debug.Log(DataManager.Instance.GameData.DayTracker.HasCompletedGoalThisTier);
		if(startTier.TierNumber == TierManager.Instance.CurrentTier) {
			LeanTween.pause(gameObject);
			DataManager.Instance.GameData.DayTracker.HasCompletedGoalThisTier = true;
			StartCoroutine("PlayCometParticle");
			if(DataManager.Instance.GameData.DayTracker.CurrentTempoGoal != "") {
				ParticleAndFloatyUtils.PlayMoneyFloaty(comet.transform.position, DataLoaderTempoGoals.GetData(DataManager.Instance.GameData.DayTracker.CurrentTempoGoal).Reward);
				CashManager.Instance.OverrideCurrentCash(DataLoaderTempoGoals.GetData(DataManager.Instance.GameData.DayTracker.CurrentTempoGoal).Reward, comet.transform.position);
			}
			else {
				ParticleAndFloatyUtils.PlayMoneyFloaty(comet.transform.position, 100);
				CashManager.Instance.OverrideCurrentCash(100, comet.transform.position);
			}
		}
		else {
			move = true;
			LeanTween.resume(gameObject);
		}
	}

	private IEnumerator PlayCometParticle() {
		cometController.SmashComet();
		yield return new WaitForSeconds(2.0f);
		LeanTween.resume(gameObject);
	}

	private void TweenCompleted() {
		move = false;
		StartCoroutine(WaitAndClose());
	}

	private IEnumerator WaitAndClose() {
		yield return new WaitForSeconds(2f);
		//StartManager.Instance.ChallengeMenuEntranceUIController.ToggleClickable(true);
		//StartManager.Instance.DinerEntranceUIController.ToggleClickable(true);
		//StartManager.Instance.ShopEntranceUIController.ToggleClickable(true);
		demux.Hide();
		notif.Finish();
    }
	#endregion
}
