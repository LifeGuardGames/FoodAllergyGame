using UnityEngine;
using UnityEngine.UI;

public class MapTrailSegment : MonoBehaviour {
	public Image fillImage;
	public Image spaceShipImage;

	private bool isInitialized = false;
	private MapUIController mapScript;
	private float totalFillWidth;
	private int segmentIndex;					// Start index at 1 instead of 0
	private float percentPerSegment;
	private Vector3 startPosition;
	private Vector3 endPosition;
	private Transform endNode;
	private bool isFilled;						// Use for toggling end nodes and optimization

	public void Init(Transform startBase, Transform endBase, int _segmentIndex, MapUIController _mapScript) {
		mapScript = _mapScript;
		segmentIndex = _segmentIndex;
		percentPerSegment = _mapScript.PercentPerSegment;
		endNode = endBase;
		spaceShipImage.gameObject.SetActive(false);

		transform.localPosition = startBase.localPosition;
		startPosition = startBase.localPosition;
		endPosition = endBase.localPosition;
		totalFillWidth = Vector3.Distance(startBase.localPosition, endBase.localPosition);
		GetComponent<RectTransform>().sizeDelta = new Vector2(totalFillWidth, 2f);

		Vector3 direction = endBase.localPosition - startBase.localPosition;
		transform.localEulerAngles = new Vector3(0f, 0f, Vector3.Angle(Vector3.right, direction));
		isInitialized = true;
    }

	public void UpdateSegmentPercentage(float tierProgressPercentage) {
		if(isInitialized) {
			if(!isFilled) {				// Shortcut to see if need processing at all
				if(tierProgressPercentage >= (segmentIndex) * percentPerSegment) {  // Fill 100%
					fillImage.rectTransform.sizeDelta = new Vector2(totalFillWidth, 10f);

					// Toggle shortcut skip
					isFilled = true;
					spaceShipImage.gameObject.SetActive(false);

					// Ping its end node to animate
					MapNode mapNode = endNode.GetComponent<MapNode>();
					mapNode.ToggleReached();
                }
				else {                                                              // Partial fill
					float difference = ((segmentIndex * percentPerSegment) - tierProgressPercentage);
					if(difference >= percentPerSegment) {
						fillImage.rectTransform.sizeDelta = new Vector2(0f, 10f);   // Blank
						spaceShipImage.gameObject.SetActive(false);
					}
					else {
						float fillPercentage = 1f - (((segmentIndex * percentPerSegment) - tierProgressPercentage) / percentPerSegment);
						fillImage.rectTransform.sizeDelta = new Vector2(totalFillWidth * fillPercentage, 10f);
						spaceShipImage.gameObject.SetActive(true);
					}
				}
			}
		}
		else {
			Debug.LogError("segment not initialized");
		}
    }

	// Used to get the comet position
	public float? GetXPositionOfSegmentPercentage(float totalPercentage) {
		if(isInitialized) {
			if(totalPercentage > (segmentIndex - 1) * percentPerSegment && totalPercentage <= segmentIndex * percentPerSegment) {
				float segmentPercentage = (totalPercentage - (segmentIndex - 1) * percentPerSegment) / percentPerSegment;
				return totalFillWidth * segmentPercentage;
			}
		}
		else {
			Debug.LogError("segment not initialized");
		}
		return null;
	}
}
