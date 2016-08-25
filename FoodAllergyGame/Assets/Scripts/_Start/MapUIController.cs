using UnityEngine;
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
	public GameObject nodeMidPrefab;
	public GameObject nodeBasePrefab;
	public GameObject trailSegmentPrefab;
	public GameObject cometPrefab;
	private GameObject comet;
	public GameObject cometParticle;

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
	private int oldTotalCash;
	private int newTotalCash;
	private ImmutableDataTiers startTier;
	private ImmutableDataTiers endTier;
	private float tierProgressPercentage;

	public void InitializeAndShow(int _oldTotalCash, int _newTotalCash, NotificationQueueData _notif) {
		// Initialize variables
		nodePositionList = new List<Vector2>();
		auxNodesList = new List<Transform>();
		segmentList = new List<MapTrailSegment>();
		segmentHeight = canvasScaler.referenceResolution.y / (numberNodesBetweenStartEnd + 1);
		notif = _notif;
		oldTotalCash = _oldTotalCash;
		newTotalCash = _newTotalCash;

		// Initialize all the node positions
		Random.seed = 42;                           // Repeatable random numbers
		for(int i = 0; i < DataLoaderTiers.GetTotalTierCount(); i++) {
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
        GameObject nodeBaseStart = GameObjectUtils.AddChildGUI(mapParent, nodeBasePrefab);
		nodeBaseStart.GetComponent<MapNodeBase>().Init(startTier, true, canvasScaler);
		auxNodesList.Add(nodeBaseStart.transform);

		for(int i = 1; i <= numberNodesBetweenStartEnd; i++) {
			GameObject nodeMid = GameObjectUtils.AddChildGUI(mapParent, nodeMidPrefab);
			nodeMid.GetComponent<MapNodeMid>().Init(startTier, i, nodePositionList);
			auxNodesList.Add(nodeMid.transform);
		}

		endTier = DataLoaderTiers.GetNextTier(startTier);
		GameObject nodeBaseEnd = GameObjectUtils.AddChildGUI(mapParent, nodeBasePrefab);
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
		UpdateTrailPercentage(tierProgressPercentage);

		// Place the comet and initialize all the rewards
	//	if(!TempoGoalManager.Instance.IsGoalActive()) {
			PlaceComet(TempoGoalManager.Instance.GetPercentageOfTier(TierManager.Instance.CurrentTier));
	//	}

        demux.Show();
    }

	public void OnShowComplete() {
		StartAnimation();
	}

	public void UpdateTrailPercentage(float trailPercentage) {
		foreach(MapTrailSegment segment in segmentList) {
			segment.UpdateSegmentPercentage(trailPercentage);
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
			Vector3? cometPosition = segment.GetPositionOfSegmentPercentage(percentage);
            if(cometPosition != null) {
				comet = GameObjectUtils.AddChildGUI(mapParent, cometPrefab);
				comet.transform.localPosition = cometPosition.GetValueOrDefault();
				cometParticle.transform.position = comet.transform.position;
				break;
			}
		}
	}

	#region Animation functions
	public void StartAnimation() {
		Debug.Log("Starting animation");
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
		LeanTween.value(gameObject, TweenValuePercentage, tierProgressPercentage, finishPercentage, 2f)
			.setEase(LeanTweenType.easeInOutQuad)
			.setDelay(1f)
			.setOnComplete(TweenCompleted);
	}

	private void TweenValuePercentage(float val) {
		// The trails themselves will update the node animations
		if(val >= TempoGoalManager.Instance.GetPercentageOfTier(TierManager.Instance.CurrentTier)&& !DataManager.Instance.GameData.DayTracker.HasCompletedGoalThisTier) {
			SmashComet();
		}
		else {
			UpdateTrailPercentage(val);
		}
    }

	private void SmashComet() {
		LeanTween.pause(gameObject);
		cometParticle.SetActive(true);
		DataManager.Instance.GameData.DayTracker.HasCompletedGoalThisTier = true;
		StartCoroutine("PlayCometParticle");
    }

	private IEnumerator PlayCometParticle() {
		yield return new WaitForSeconds(2.0f);
		Destroy(comet.gameObject);
		cometParticle.SetActive(false);
		LeanTween.resume(gameObject);
	}

	private void TweenCompleted() {
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
