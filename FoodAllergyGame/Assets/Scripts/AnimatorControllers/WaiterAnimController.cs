using UnityEngine;
using System.Collections;

public class WaiterAnimController : MonoBehaviour {
	public SkeletonAnimation skeleton;
	private bool isCurrentlyMoving = false;
	private bool isFacingRight = true;
	private WaiterHands hand1 = WaiterHands.None;	// Aux that is synced with waiter
	private WaiterHands hand2 = WaiterHands.None;
	private WaiterHands lastHand1 = WaiterHands.None;   // Aux that is synced with waiter
	private WaiterHands lastHand2 = WaiterHands.None;

	public void ChangeOrderInLayer(int order) {
		// TODO Finish this!!!
	}

	// TODO Rip out separate animations
	void Start() {
		Reset();
		 skeleton.state.SetAnimation(0, "Blink", true);
		//skeleton.state.SetAnimation(0, "ArmRunBack", true);
		//skeleton.state.SetAnimation(1, "Sprint", true);
		//FlipAndSetHand();
	}

	private void Reset() {
		skeleton.state.ClearTracks();
	//	skeleton.state.SetAnimation(0, "Reset", false);
	}

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
		this.isFacingRight = isFacingRight;
		//FlipAndSetHand();
    }
	
	public void RefreshHand() {
		hand1 = Waiter.Instance.Hand1;
		hand2 = Waiter.Instance.Hand2;
		FlipAndSetHand();
    }
	
	private void FlipAndSetHand() {
		/*
		// hand1 has track 2 and hand2 has track 3
		if(isCurrentlyMoving) {
			if(isFacingRight) {
				//skeleton.state.ClearTrack(2);
				skeleton.state.AddAnimation(2, hand1 == WaiterHands.None ? "ArmRunBack" : "ArmRunCarryBack", true, 0f);
				//skeleton.state.ClearTrack(3);
				skeleton.state.AddAnimation(3, hand2 == WaiterHands.None ? "ArmRunFront" : "ArmRunCarryFront", true, 0f);
			}
			else {	// Switch the front and back arm animations
				//skeleton.state.ClearTrack(2);
				skeleton.state.SetAnimation(2, hand1 == WaiterHands.None ? "ArmRunFront" : "ArmRunCarryFront", true);
				//skeleton.state.ClearTrack(3);
				skeleton.state.SetAnimation(3, hand2 == WaiterHands.None ? "ArmRunBack" : "ArmRunCarryBack", true);
			}
		}
		else {
			if(isFacingRight) {
				//skeleton.state.ClearTrack(2);
				skeleton.state.AddAnimation(2, hand1 == WaiterHands.None ? "ArmIdleBack" : "ArmIdleCarryBack", true, 0f);
				//skeleton.state.ClearTrack(3);
				skeleton.state.AddAnimation(3, hand2 == WaiterHands.None ? "ArmIdleFront" : "ArmIdleCarryFront", true, 0f);
			}
			else {  // Switch the front and back arm animations
				//skeleton.state.ClearTrack(2);
				skeleton.state.SetAnimation(2, hand1 == WaiterHands.None ? "ArmIdleFront" : "ArmIdleCarryFront", true);
				//skeleton.state.ClearTrack(3);
				skeleton.state.SetAnimation(3, hand2 == WaiterHands.None ? "ArmIdleBack" : "ArmIdleCarryBack", true);
			}
		}
		*/
	}
}
