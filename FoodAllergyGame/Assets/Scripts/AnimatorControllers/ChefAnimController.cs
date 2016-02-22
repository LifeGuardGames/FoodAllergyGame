using UnityEngine;
using System.Collections;

public class ChefAnimController : MonoBehaviour {
	public SkeletonAnimation skeletonAnim;

	void Start() {
		skeletonAnim.state.Start += delegate {
			skeletonAnim.skeleton.SetToSetupPose();     // NOTE: Make sure default mix time is 0!!!
		};
		SetIdle();
    }

	public void SetIdle() {
		skeletonAnim.state.SetAnimation(0, "Idle", false);
		int randomIndex = UnityEngine.Random.Range(0, 3);
		switch(randomIndex) {
			case 0:
				skeletonAnim.state.AddAnimation(0, "Idle1", false, 0f);
				skeletonAnim.state.AddAnimation(0, "Idle", true, 0f);
				break;
			case 1:
				skeletonAnim.state.AddAnimation(0, "IdleJuggle", false, 0f);
				skeletonAnim.state.AddAnimation(0, "Idle", true, 0f);
				break;
			case 2:
				skeletonAnim.state.AddAnimation(0, "IdleJuggle2", false, 0f);
				skeletonAnim.state.AddAnimation(0, "Idle", true, 0f);
				break;
			default:
				Debug.Log("Bad index");
				break;
		}
	}

	/// <summary>
	/// Order in, chef starts cooking
	/// </summary>
	public void SetStartCooking() {
		skeletonAnim.state.SetAnimation(0, "Cooking", false);
	}

	/// <summary>
	/// Chef completely finishes cooking and brings up a dish
	/// </summary>
	public void SetFinishCooking() {
		skeletonAnim.state.SetAnimation(0, "FinishedCooking", false);
		skeletonAnim.state.AddAnimation(0, "Idle", true, 0f);
	}

	/// <summary>
	/// Chef cancels an order and goes back to idle
	/// </summary>
	public void SetCancelCooking() {
		skeletonAnim.state.SetAnimation(0, "Idle", true);
		skeletonAnim.state.AddAnimation(0, "Idle", true, 0f);
	}
}
