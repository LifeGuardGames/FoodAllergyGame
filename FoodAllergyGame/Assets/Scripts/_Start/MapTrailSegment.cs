using UnityEngine;
using UnityEngine.UI;

public class MapTrailSegment : MonoBehaviour {
	public Image fillImage;

	private MapUIController mapScript;
	private float totalFillWidth;
	private int segmentIndex;					// Start index at 1 instead of 0
	private float percentPerSegment;

	public void Init(Transform startBase, Transform endBase, int _segmentIndex, MapUIController _mapScript) {
		mapScript = _mapScript;
		segmentIndex = _segmentIndex;
		percentPerSegment = _mapScript.PercentPerSegment;

		transform.localPosition = startBase.localPosition;
		totalFillWidth = Vector3.Distance(startBase.localPosition, endBase.localPosition);
		GetComponent<RectTransform>().sizeDelta = new Vector2(totalFillWidth, 2f);

		Vector3 direction = endBase.localPosition - startBase.localPosition;
		transform.localEulerAngles = new Vector3(0f, 0f, Vector3.Angle(Vector3.right, direction));
    }

	public void UpdateSegmentPercentage(float tierProgressPercentage) {
		if(tierProgressPercentage >= (segmentIndex) * percentPerSegment) {	// Fill 100%
			fillImage.rectTransform.sizeDelta = new Vector2(totalFillWidth, 10f);
		}
		else {																// Partial fill
			float difference = ((segmentIndex * percentPerSegment) - tierProgressPercentage);
			if(difference >= percentPerSegment) {
				fillImage.rectTransform.sizeDelta = new Vector2(0f, 10f);   // Blank
			}
			else {
				float fillPercentage = 1f - (((segmentIndex * percentPerSegment) - tierProgressPercentage) / percentPerSegment);
				fillImage.rectTransform.sizeDelta = new Vector2(totalFillWidth * fillPercentage, 10f);
			}
		}
    }
}
