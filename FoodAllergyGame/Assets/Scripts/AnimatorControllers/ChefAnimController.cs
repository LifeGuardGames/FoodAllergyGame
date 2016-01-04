using UnityEngine;
using System.Collections;

public class ChefAnimController : MonoBehaviour {
	public SkeletonAnimation skeleton;

	private void Reset() {
		Debug.Log("RESET");
		skeleton.state.ClearTracks();
		skeleton.state.SetAnimation(0, "Reset", false);
	}

	void Start() {
		SetIdle();
    }

	public void SetIdle() {
		Reset();
		int randomIndex = UnityEngine.Random.Range(0, 3);
		skeleton.state.AddAnimation(0, "Idle", true, 0f).Complete += delegate {
			Reset();
			switch(randomIndex) {
				case 0:
					skeleton.state.AddAnimation(0, "Idle1", false, 0f);
					skeleton.state.AddAnimation(0, "Idle", true, 0f);
					break;
				case 1:
					skeleton.state.AddAnimation(0, "IdleJuggle", false, 0f);
					skeleton.state.AddAnimation(0, "Idle", true, 0f);
					break;
				case 2:
					skeleton.state.AddAnimation(0, "IdleJuggle2", false, 0f);
					skeleton.state.AddAnimation(0, "Idle", true, 0f);
					break;
			}
		};
	}

	/// <summary>
	/// Order in, chef starts cooking
	/// </summary>
	public void SetStartCooking() {
		Reset();
		skeleton.state.AddAnimation(0, "Cooking", false, 0f);
	}

	/// <summary>
	/// Chef completely finishes cooking and brings up a dish
	/// </summary>
	public void SetFinishCooking() {
		Reset();
		skeleton.state.AddAnimation(0, "FinishedCooking", false, 0f).Complete += delegate {
			SetIdle();
        };
	}

	/// <summary>
	/// Chef cancels an order and goes back to idle
	/// </summary>
	public void SetCancelCooking() {
		Reset();
		skeleton.state.AddAnimation(0, "Idle", true, 0f).Complete += delegate {
			SetIdle();
		};
	}
}
