public class NotificationQueueDataAge : NotificationQueueData{

	public override void Start() {
		//shows the ask age panel
		StartManager.Instance.ageAskController.ShowPanel();
	}

	public override void Finish() {
		base.Finish();
	}
}
