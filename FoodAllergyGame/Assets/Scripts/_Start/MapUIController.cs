using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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

	private float segmentHeight;					// Y component from one node to the other
	private int numberNodesBetweenStartEnd = 3;
	private List<Vector2> nodePositionList;         // Positions of the nodes, some random, some static
	private List<Transform> auxNodesList;           // Keep track of all nodes spawned, used for line
	private List<MapTrailSegment> segmentList;

	private float percentPerSegment;
	public float PercentPerSegment {
		get { return percentPerSegment; }
	}

	public void InitializeAndShow() {
		// Initialize variables
		nodePositionList = new List<Vector2>();
		auxNodesList = new List<Transform>();
		segmentList = new List<MapTrailSegment>();
		segmentHeight = canvasScaler.referenceResolution.y / (numberNodesBetweenStartEnd + 1);

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

		ImmutableDataTiers startTier = DataLoaderTiers.GetDataFromTier(DataLoaderTiers.GetTierFromCash(CashManager.Instance.TotalCash));
        GameObject nodeBaseStart = GameObjectUtils.AddChildGUI(mapParent, nodeBasePrefab);
		nodeBaseStart.GetComponent<MapNodeBase>().Init(startTier, true, canvasScaler);
		auxNodesList.Add(nodeBaseStart.transform);

		for(int i = 1; i <= numberNodesBetweenStartEnd; i++) {
			GameObject nodeMid = GameObjectUtils.AddChildGUI(mapParent, nodeMidPrefab);
			nodeMid.GetComponent<MapNodeMid>().Init(startTier, i, nodePositionList);
			auxNodesList.Add(nodeMid.transform);
		}

		ImmutableDataTiers endTier = DataLoaderTiers.GetNextTier(startTier);
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
		float tierProgressPercentage;
		if(endTier != null) {
			int totalCashNeededForTier = endTier.CashCutoffFloor - startTier.CashCutoffFloor;
			tierProgressPercentage = (float)(CashManager.Instance.LastSeenTotalCash - startTier.CashCutoffFloor) / totalCashNeededForTier;
			Debug.Log("tier progress percentage start " + tierProgressPercentage);
		}
		else {  // Reached last tier
			tierProgressPercentage = 1f;
			Debug.Log("tier progress percentage full");
        }
		UpdateTrailPercentage(tierProgressPercentage);

		// Place the comet and initialize all the rewards
		if(!TempoGoalManager.Instance.IsGoalActive()) {
			PlaceComet(TempoGoalManager.Instance.GetPercentageInCurrentTier());
		}

        demux.Show();
	}

	public void UpdateTrailPercentage(float trailPercentage) {
		foreach(MapTrailSegment segment in segmentList) {
			segment.UpdateSegmentPercentage(trailPercentage);
		}
	}

	private void HidePanel() {
		demux.Hide();
	}

	public void OnExitButton() {
		//StartManager.Instance.ChallengeMenuEntranceUIController.ToggleClickable(true);
		//StartManager.Instance.DinerEntranceUIController.ToggleClickable(true);
		//StartManager.Instance.ShopEntranceUIController.ToggleClickable(true);
		HidePanel();
	}

	public void PlaceComet(float percentage) {
		// Determine which segment this goes into
		foreach(MapTrailSegment segment in segmentList) {
			Vector3? cometPosition = segment.GetPositionOfSegmentPercentage(percentage);
            if(cometPosition != null) {
				GameObject comet = GameObjectUtils.AddChildGUI(mapParent, cometPrefab);
				comet.transform.localPosition = cometPosition.GetValueOrDefault();
				break;
			}
		}
	}
}
