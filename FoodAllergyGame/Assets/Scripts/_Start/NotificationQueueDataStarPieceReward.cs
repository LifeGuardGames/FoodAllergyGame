using UnityEngine.SceneManagement;

/// <summary>
/// Template of a NotificationQueueData
/// Launches the reward animation for getting a star piece
/// </summary>
public class NotificationQueueDataStarPieceReward : NotificationQueueData {
	private int oldTier;
	private int newTier;

	public NotificationQueueDataStarPieceReward(string allowedScene, int _oldTier, int _newTier){
		this.allowedScene = allowedScene;
		oldTier = _oldTier;
		newTier = _newTier;
	}

	public override void Start(){
		if(allowedScene == SceneManager.GetActiveScene().name){
			StartManager.Instance.StarsUIController.RewardStarPiece(this, oldTier, newTier);
		}
		else{
			Finish();
		}
	}

	public override void Finish(){
		base.Finish();
	}
}
