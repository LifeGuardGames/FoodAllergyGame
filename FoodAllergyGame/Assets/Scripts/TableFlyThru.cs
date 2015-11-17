using UnityEngine;
using System.Collections;

public class TableFlyThru : Table {

	public TweenToggle FlyThruToggle;

	// Use this for initialization
	void Start () {
		Init();
	}

	public override void Init() {
		base.Init();
		if(DataManager.Instance.GetEvent() == "EventTFlyThru") {
			GameObject.Find("TutFingers").transform.GetChild(9).gameObject.SetActive(true);
		}
		node = Pathfinding.Instance.NodeFlyThru;
	}

	public override void TalkToConsumer() {
		base.TalkToConsumer();
		if(DataManager.Instance.GetEvent() == "EventTFlyThru") {
			GameObject.Find("TutFingers").transform.GetChild(9).gameObject.SetActive(false);
		}
	}

	public void FlyThruDropDown() {
		AudioManager.Instance.PlayClip("FlyThruEnter");
		FlyThruToggle.Show();
	}

	public void FlyThruLeave() {
		AudioManager.Instance.PlayClip("FlyThruLeave");
		FlyThruToggle.Hide();
	}
}
