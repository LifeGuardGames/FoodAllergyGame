using UnityEngine;
using System.Collections;

public class CustomerAnimController : MonoBehaviour {
	public SkeletonAnimation skeleton;

	public void SetWaitingInLine(){
		skeleton.state.SetAnimation(0, "WaitingInLine", true);
	}

	public void UpdateSatisfaction(int delta){
		if(delta > 0){
			// TODO get new animation
		}
		else if(delta < 0){
			skeleton.state.SetAnimation(0, "Losing_Heart", false);
		}
	}
	
	public void SetReadingMenu(){
		skeleton.state.SetAnimation(0, "ReadingMenu", true);
	}

	public void SetWaitingForOrder(){
		skeleton.state.SetAnimation(0, "Waiting_Active", true);
	}

	public void SetEating(){
		skeleton.state.SetAnimation(0, "Eating", true);
	}

	public void SetWaitingForCheck(){
		skeleton.state.SetAnimation(0, "Waiting_Active", true);
	}

	public void SetWaitingForFood(){
		skeleton.state.SetAnimation(0, "Waiting_Passive", true);
	}

}
