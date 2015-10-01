using UnityEngine;
using System.Collections;

public class CustomerAnimController : MonoBehaviour {
	public SkeletonAnimation skeleton;
	private string currentWaitingStateString;	// For use after losing heart, revert to corrosponding waiting animation

	public void SetWaitingInLine(){
		skeleton.state.SetAnimation(0, "WaitingInLine", true);
		currentWaitingStateString = "WaitingInLine";
	}

//	void OnGUI(){
//		if(GUI.Button(new Rect(100, 100, 100, 100), "1")){
//			skeleton.skeleton.SetToSetupPose();
//		}
//		if(GUI.Button(new Rect(200, 100, 100, 100), "2")){
//			skeleton.state.SetAnimation(0, "LosingHeart", false);
//		}
//		if(GUI.Button(new Rect(300, 100, 100, 100), "3")){
//			skeleton.state.SetAnimation(1, "Reset", false);
//		}
//	}

	public void ResetListener(Spine.AnimationState state, int trackIndex){
		Debug.Log("Resetting");
//		skeleton.skeleton.SetToSetupPose();
		skeleton.state.SetAnimation(0, "Reset", false);

	}

	public void UpdateSatisfaction(int delta){
		if(delta > 0){
			// TODO get new animation
		}
		else if(delta < 0){
			skeleton.state.SetAnimation(0, "LosingHeart", false);
//			skeleton.state.AddAnimation(0, currentWaitingStateString, true, 0)
			skeleton.state.AddAnimation(0, currentWaitingStateString, true, 0.2f).Start += delegate {
				skeleton.skeleton.SetToSetupPose();
				skeleton.skeleton.SetBonesToSetupPose();
				skeleton.skeleton.SetSlotsToSetupPose();
			};
		}
	}
	
	public void SetReadingMenu(){
		skeleton.state.ClearTracks();
		skeleton.state.SetAnimation(0, "ReadingMenu", true);
	}

	public void SetWaitingForOrder(){
		skeleton.state.SetAnimation(0, "WaitingActive", true);
		currentWaitingStateString = "WaitingActive";
	}

	public void SetEating(){
		skeleton.state.SetAnimation(0, "Eating", true);
	}

	public void SetWaitingForCheck(){
		skeleton.state.SetAnimation(0, "WaitingActive", true);
		currentWaitingStateString = "WaitingActive";
	}

	public void SetWaitingForFood(){
		skeleton.state.SetAnimation(0, "WaitingPassive", true);
		currentWaitingStateString = "WaitingPassive";
	}

	public void SetRandomAllergyAttack(){
		int randomIndex = Random.Range(1, 3);	// Get random int between 1 and 2
		skeleton.state.SetAnimation(0, "AllergyAttack" + randomIndex.ToString(), false);
	}

	public void SetSavedAllergyAttack(){
		skeleton.state.SetAnimation(0, "Reset", false);
		skeleton.state.AddAnimation(0, "SavedAllergy", false, 0.2f).Start += delegate {
			skeleton.skeleton.SetToSetupPose();
			skeleton.skeleton.SetBonesToSetupPose();
			skeleton.skeleton.SetSlotsToSetupPose();
		};;
		skeleton.state.AddAnimation(0, "WaitingActive", true, 0.2f).Start += delegate {
			skeleton.skeleton.SetToSetupPose();
			skeleton.skeleton.SetBonesToSetupPose();
			skeleton.skeleton.SetSlotsToSetupPose();
		};;
	}
}
