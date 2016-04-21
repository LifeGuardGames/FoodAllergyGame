using UnityEngine;
using UnityEngine.UI;

public class StarsUIController : MonoBehaviour {
	public Image starBase;
	public Image starCore;
	public Animator starsAnimator;
	public TweenToggleDemux starsDemux;

	private Sprite nextStarSpriteAux;   // Aux for populating the next star piece
	private NotificationQueueData internalNotifData;

	void Start() {
		starsAnimator.Play("StarDisabled");
	}

	#region Star Piece Rewarding
	public void RewardStarPiece(NotificationQueueData notifData, int oldTier, int newTier) {
		if(oldTier % 6 == 0) {
			starBase.enabled = false;
		}
		else {
			starBase.sprite = SpriteCacheManager.GetStarFillHelper(oldTier % 6, 6);
		}
		nextStarSpriteAux = SpriteCacheManager.GetStarFillHelper(newTier % 6, 6);
		Debug.Log(nextStarSpriteAux.name);

		internalNotifData = notifData;
        starsDemux.Show();
        starsAnimator.Play("StarPieceReward");
	}

	public void OnNewStarSpriteEvent() {
		starBase.enabled = true;
		starBase.sprite = nextStarSpriteAux;
	}
	#endregion

	#region Star Core Rewarding
	public void RewardStarCore(NotificationQueueData notifData) {
		internalNotifData = notifData;
		starsDemux.Show();
		starsAnimator.Play("StarCoreReward");
    }

	// NOTE: Animator overrides all instances of sprite change if it is changed somewhere
	public void OnCoreRewardStart() {
		starBase.sprite = SpriteCacheManager.GetStarFillHelper(6, 6);
	}
	#endregion
	
	// Called from StarAnimHelper
	public void OnRewardFinish() {
		starsDemux.Hide();
    }

	// Call hide on notification
	public void OnFinishHideDone() {
		internalNotifData.Finish();
    }
}
