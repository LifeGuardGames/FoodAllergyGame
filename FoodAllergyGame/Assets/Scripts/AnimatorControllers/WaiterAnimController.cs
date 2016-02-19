using UnityEngine;
using System.Collections;
using Spine;

/// <summary>
/// Custom animation controller for the waiter
/// Note: Make sure this is attached to the skeleton itself, it does a x-scale for flipping
/// So putting it in the base parent gameobject wont work
/// </summary>
public class WaiterAnimController : MonoBehaviour {
	public SkeletonAnimation skeletonAnim;
	private bool isMoving = false;
	private bool isFacingRight = true;
	private WaiterHands hand1 = WaiterHands.None;	// Aux that is synced with waiter
	private WaiterHands hand2 = WaiterHands.None;
	//private WaiterHands lastHand1 = WaiterHands.None;   // Aux that is synced with waiter
	//private WaiterHands lastHand2 = WaiterHands.None;
	private string currentBodyAnimation = "";

	public void ChangeOrderInLayer(int order) {
		// TODO Finish this!!!
	}

	void Start() {
		skeletonAnim.state.Start += delegate {
			skeletonAnim.skeleton.SetToSetupPose();		// NOTE: Make sure default mix time is 0!!!
		};
	}

	private void Reset() {
		//skeletonAnim.state.ClearTracks();
		//skeletonAnim.state.SetAnimation(0, "Reset", false);
	}

	public void SetMoving(bool isMoving) {
		this.isMoving = isMoving;
		FlipAndSetHand();
    }

	public void SetDirection(bool isFacingRight) {
        transform.localScale = new Vector3(isFacingRight ? 1f : -1f, 1f, 1f);
		this.isFacingRight = isFacingRight;
		FlipAndSetHand();
    }
	
	public void RefreshHand() {
		hand1 = Waiter.Instance.Hand1;
		hand2 = Waiter.Instance.Hand2;
		FlipAndSetHand();
    }
	
	private void FlipAndSetHand() {

        if(isMoving) {
			if(hand1 == WaiterHands.None && hand2 == WaiterHands.None) {
				SetBodyAnimation("Run");
            }
			else if(hand1 != WaiterHands.None && hand2 == WaiterHands.None) {
				SetBodyAnimation(isFacingRight ? "RunCarryFront" : "RunCarryBack");
			}
			else if(hand1 == WaiterHands.None && hand2 != WaiterHands.None) {
				SetBodyAnimation(isFacingRight ? "RunCarryBack" : "RunCarryFront");
			}
			else if(hand1 != WaiterHands.None && hand2 != WaiterHands.None) {
				SetBodyAnimation("RunCarryBoth");
			}
			else {
				Debug.LogError("Bad waiter state");
			}
		}
		else {
			if(hand1 == WaiterHands.None && hand2 == WaiterHands.None) {
				SetBodyAnimation("Idle");
			}
			else if(hand1 != WaiterHands.None && hand2 == WaiterHands.None) {
				SetBodyAnimation(isFacingRight ? "IdleCarryFront" : "IdleCarryBack");
			}
			else if(hand1 == WaiterHands.None && hand2 != WaiterHands.None) {
				SetBodyAnimation(isFacingRight ? "IdleCarryBack" : "IdleCarryFront");
			}
			else if(hand1 != WaiterHands.None && hand2 != WaiterHands.None) {
				SetBodyAnimation("IdleCarryBoth");
			}
			else {
				Debug.LogError("Bad waiter state");
			}
		}
	}

	private void SetBodyAnimation(string bodyAnimation) {
		if(currentBodyAnimation != bodyAnimation) {
			currentBodyAnimation = bodyAnimation;
			skeletonAnim.state.SetAnimation(0, bodyAnimation, true);
		}
	}
}
