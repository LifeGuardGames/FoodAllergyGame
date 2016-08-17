using UnityEngine;
using UnityEngine.UI;

public class MapNodeBase : MonoBehaviour {

	public void Init(ImmutableDataTiers tier, bool isStartTier, CanvasScaler canvasScaler) {
		if(isStartTier) {
			transform.localPosition = new Vector3(0f, 0f, 0f);
		}
		else {
			transform.localPosition = new Vector3(0f, canvasScaler.referenceResolution.y, 0f);
			if(tier != null) {

			}
			else {
				// TODO Last tier here
			}
		}
	}
}
