using UnityEngine;
using System.Collections;

public class CustomerGossiper : Customer{

	public Behav pastBehav;


	public override void OrderTaken (ImmutableDataFood food){
		base.OrderTaken (food);
		int rand = Random.Range(0,10);
		if(rand > 7){
			Gossip();
		}
	}
	public override void Eating ()	{
		base.Eating ();
		int rand = Random.Range(0,10);
		if(rand > 7){
			Gossip();
		}
	}
	public void Gossip(){
		pastBehav = currBehav;
	}

	public void GoAway(){
		customerAnim.skeletonAnim.state.SetAnimation(0, "WaitingPassive", false);
		currBehav.Reason();
		currBehav = pastBehav; 
	}
}
