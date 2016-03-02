using UnityEngine;

public class CustomerAnimController : MonoBehaviour {
	public SkeletonAnimation skeletonAnim;
	public ParticleSystem puke;
	protected string currentWaitingStateString;	// For use after losing heart, revert to corrosponding waiting animation

	public bool isLimitAllergyAttackAnim = false;

	void Start() {
		skeletonAnim.state.Start += delegate {
			skeletonAnim.skeleton.SetToSetupPose();     // NOTE: Make sure default mix time is 0!!!
		};
	}

	public void SetWaitingInLine(){
		skeletonAnim.state.SetAnimation(0, "WaitingInLine", true);
		currentWaitingStateString = "WaitingInLine";
	}

	public void UpdateSatisfaction(int delta){
		if(delta > 0){
			// TODO get new animation
		}
		else if(delta < 0){
			skeletonAnim.state.SetAnimation(0, "LosingHeart", false);
			skeletonAnim.state.AddAnimation(0, currentWaitingStateString, true, 0f);
		}
	}
	
	public void SetReadingMenu(){
		skeletonAnim.state.SetAnimation(0, "ReadingMenu", true);
	}

	public void SetWaitingForOrder(){
		skeletonAnim.state.SetAnimation(0, "WaitingActive", true);
		currentWaitingStateString = "WaitingActive";
	}

	public void SetEating(){
		skeletonAnim.state.SetAnimation(0, "Eating", true);
	}

	public void SetWaitingForCheck(){
		skeletonAnim.state.SetAnimation(0, "WaitingActive", true);
		currentWaitingStateString = "WaitingActive";
	}

	public void SetWaitingForFood(){
		skeletonAnim.state.SetAnimation(0, "WaitingPassive", true);
		currentWaitingStateString = "WaitingPassive";
	}

	public virtual void SetRandomAllergyAttack(){
		if(isLimitAllergyAttackAnim){
			skeletonAnim.state.SetAnimation(0, "AllergyAttack1", false);
		}
		else{
			int randomIndex = Random.Range(1, 3);	// Get random int between 1 and 2
			skeletonAnim.state.SetAnimation(0, "AllergyAttack" + randomIndex.ToString(), false);
		}
	}

	public virtual void SetSavedAllergyAttack(){
		skeletonAnim.state.SetAnimation(0, "SavedAllergy", false);
	}
}
