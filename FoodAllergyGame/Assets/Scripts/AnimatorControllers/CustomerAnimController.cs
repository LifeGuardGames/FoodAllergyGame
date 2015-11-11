using UnityEngine;
using System.Collections;

public class CustomerAnimController : MonoBehaviour {
	public SkeletonAnimation skeleton;
	private string currentWaitingStateString;	// For use after losing heart, revert to corrosponding waiting animation

	public bool isLimitAllergyAttackAnim = false;

	public void Start() {
		skeleton.state.SetAnimation(0, "AllergyAttack1", false);
	}

	public void SetWaitingInLine(){
		skeleton.state.SetAnimation(0, "WaitingInLine", true);
		currentWaitingStateString = "WaitingInLine";
	}

//	void OnGUI(){
//		if(GUI.Button(new Rect(100, 100, 100, 100), "1")){
//			Debug.Log("Setup pose");
//			skeleton.state.ClearTracks();
//			skeleton.state.SetAnimation(0, "Reset", false);
////			skeleton.skeleton.SetToSetupPose();
//		}
//		if(GUI.Button(new Rect(200, 100, 100, 100), "2")){
//			UpdateSatisfaction(-1);
//		}
//		if(GUI.Button(new Rect(300, 100, 100, 100), "3")){
//			skeleton.state.SetAnimation(0, "Reset", false);
//		}
//	}

	public void Reset(){
		skeleton.state.ClearTracks();
		skeleton.state.SetAnimation(0, "Reset", false);
	}

	public void UpdateSatisfaction(int delta){
		if(delta > 0){
			// TODO get new animation
		}
		else if(delta < 0){
			skeleton.state.SetAnimation(0, "LosingHeart", false).Complete += delegate {
				Reset();
				skeleton.state.AddAnimation(0, currentWaitingStateString, true, 0f);
			};
		}
	}
	
	public void SetReadingMenu(){
		Reset();
		skeleton.state.AddAnimation(0, "ReadingMenu", true, 0f);
	}

	public void SetWaitingForOrder(){
		Reset();
		skeleton.state.AddAnimation(0, "WaitingActive", true, 0f);
		currentWaitingStateString = "WaitingActive";
	}

	public void SetEating(){
		Reset();
		skeleton.state.AddAnimation(0, "Eating", true, 0f);
	}

	public void SetWaitingForCheck(){
		Reset();
		skeleton.state.AddAnimation(0, "WaitingActive", true, 0f);
		currentWaitingStateString = "WaitingActive";
	}

	public void SetWaitingForFood(){
		Reset();
		skeleton.state.AddAnimation(0, "WaitingPassive", true, 0f);
		currentWaitingStateString = "WaitingPassive";
	}

	public void SetRandomAllergyAttack(){
		Reset();
		if(isLimitAllergyAttackAnim){
			skeleton.state.AddAnimation(0, "AllergyAttack1", false, 0f);
		}
		else{
			int randomIndex = Random.Range(1, 3);	// Get random int between 1 and 2
			skeleton.state.AddAnimation(0, "AllergyAttack" + randomIndex.ToString(), false, 0f);
		}
	}

	public void SetSavedAllergyAttack(){
		Reset();
		skeleton.state.AddAnimation(0, "SavedAllergy", false, 0f);
		skeleton.state.AddAnimation(0, "WaitingActive", true, 0f);
	}
}
