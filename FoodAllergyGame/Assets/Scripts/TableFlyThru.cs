using UnityEngine;
using UnityEngine.SceneManagement;

public class TableFlyThru : Table {
	public TweenToggle FlyThruToggle;

	void Start () {
		Init();
	}

	public override void Init() {
		if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName(SceneUtils.DECO)) {
			base.Init();
			if(DataManager.Instance.GetEvent() == "EventTFlyThru") {
				GameObject.Find("TutFingers").transform.GetChild(9).gameObject.SetActive(true);
			}
			node = Pathfinding.Instance.NodeFlyThru;

			// Set the custom sorting order
			SetBaseSortingOrder(FlyThruLoader.baseSortingOrder);
		}
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

	// Special case for FlyThru
	public override void SetBaseSortingOrder(int _baseSortingOrder) {
		baseSortingOrder = _baseSortingOrder;

		// ....
	}
}
