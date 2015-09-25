using UnityEngine;
using System.Collections;

public class CustomerAnimController : MonoBehaviour {
	public SkeletonAnimation skeleton;
	private string currentWaitingStateString;	// For use after losing heart, revert to corrosponding waiting animation

	public void SetWaitingInLine(){
		skeleton.state.SetAnimation(0, "WaitingInLine", true);
		currentWaitingStateString = "WaitingInLine";
	}

	public void UpdateSatisfaction(int delta){
		if(delta > 0){
			// TODO get new animation
		}
		else if(delta < 0){
			skeleton.state.ClearTracks();
			skeleton.state.SetAnimation(0, "LosingHeart", false);
			skeleton.state.SetAnimation(1, "Reset", false);
			skeleton.state.SetAnimation(2, currentWaitingStateString, true);
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
		skeleton.state.SetAnimation(1, "SavedAllergy", false);
		skeleton.state.SetAnimation(2, "WaitingActive", true);
	}
}
