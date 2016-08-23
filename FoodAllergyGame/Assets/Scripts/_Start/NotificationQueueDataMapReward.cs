using UnityEngine.SceneManagement;

/// <summary>
/// Implementation of a NotificationQueueData
/// Launches the reward animation for map progress
/// </summary>
public class NotificationQueueDataMapReward : NotificationQueueData {

	public NotificationQueueDataMapReward(string allowedScene) {
		this.allowedScene = allowedScene;
	}

	public override void Start() {
		if(allowedScene == SceneManager.GetActiveScene().name) {
			StartManager.Instance.MapUIController.InitializeAndShow();
		}
		else {
			Finish();
		}
	}

	public override void Finish() {
		base.Finish();
	}
}
