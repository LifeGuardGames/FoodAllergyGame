using UnityEngine;
using System.Collections;

public class WaiterAnimController : MonoBehaviour {
	public SkeletonAnimation skeleton;
	private bool isCurrentlyMoving = false;

	public void ChangeOrderInLayer(int order) {
		// TODO Finish this!!!
	}

	void Start() {
		skeleton.state.SetAnimation(0, "Blink", true);
	}

	//private void Reset() {
	//	skeleton.state.ClearTracks();
	//	skeleton.state.SetAnimation(0, "Reset", false);
	//}

	public void SetMoving(bool isMoving) {
		if(isMoving) {
			if(!isCurrentlyMoving) {
				isCurrentlyMoving = true;
				skeleton.state.ClearTrack(1);
				skeleton.state.SetAnimation(1, "Sprint", true);
            }
		}
		else {
			isCurrentlyMoving = false;
			skeleton.state.ClearTrack(1);
			skeleton.state.SetAnimation(1, "Idle", true);
		}
	}

	public void SetDirection(bool isFacingRight) {
		transform.localScale = new Vector3(isFacingRight ? 1f : -1f, 1f, 1f);
	}
}
