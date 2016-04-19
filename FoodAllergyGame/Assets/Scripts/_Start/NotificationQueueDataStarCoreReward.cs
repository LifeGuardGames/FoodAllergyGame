using UnityEngine.SceneManagement;

/// <summary>
/// Template of a NotificationQueueData
/// Launches the reward animation for getting a star core
/// </summary>
public class NotificationQueueDataStarCoreReward : NotificationQueueData {

	public NotificationQueueDataStarCoreReward(string allowedScene){
		this.allowedScene = allowedScene;
	}

	public override void Start() {
		if(allowedScene == SceneManager.GetActiveScene().name) {
			StartManager.Instance.StarsUIController.RewardStarCore(this);
		}
		else {
			Finish();
		}
	}

	public override void Finish(){
		base.Finish();
	}
}
