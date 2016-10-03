using UnityEngine;
using UnityEngine.UI;

public class MapNodeBase : MapNode {
	public Text starNumberText;
	public Animation reachedAnim;
	public Image nodeImage;
	
	private int currentRewardIndex = 0;
	private bool isStartTier;

	public void Init(ImmutableDataTiers tier, bool _isStartTier, CanvasScaler canvasScaler) {
		isStartTier = _isStartTier;
		if(_isStartTier) {
			transform.localPosition = new Vector3(0f, 0f, 0f);
        }
		else {
			transform.localPosition = new Vector3(0f, canvasScaler.referenceResolution.y - 100, 0f);
			starNumberText.enabled = true;
			if(tier != null) {
				starNumberText.text = "Star Piece " + tier.TierNumber.ToString();
			}
			else {
				starNumberText.text = "Coming Soon!";
			}
		}
	}

	public override void ToggleReached(bool isSetup) {
		if(!isStartTier) {
			AudioManager.Instance.PlayClip("MapEndReach");
		}
		if(!isSetup) {
			AudioManager.Instance.PlayClip("MapNodeReach");
			reachedAnim.Play();
		}
    }
}
