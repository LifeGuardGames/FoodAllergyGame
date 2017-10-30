using UnityEngine.SceneManagement;

/// <summary>
/// Implementation of a NotificationQueueData
/// Launches the reward animation for map progress
/// </summary>
public class NotificationQueueDataMapReward : NotificationQueueData {
	private int oldTotalCash;
	private int newTotalCash;

	public NotificationQueueDataMapReward(string allowedScene, int _oldTotalCash, int _newTotalCash) {
		this.allowedScene = allowedScene;
		oldTotalCash = _oldTotalCash;
		newTotalCash = _newTotalCash;
	}

	public override void Start() {
		if(allowedScene == SceneManager.GetActiveScene().name) {
			//StartManager.Instance.MapUIController.InitializeAndShow(oldTotalCash, newTotalCash, this);
		}
		else {
			Finish();
		}
	}

	public override void Finish() {
		base.Finish();
	}
}
