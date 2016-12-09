using UnityEngine;

public class CustomerCoolKid : Customer {
	public GameObject thoughtBubble;

	public override void Init(int num, ImmutableDataEvents mode) {
		type = CustomerTypes.CoolKid;
		base.Init(num, mode);
		menuTimer *= 2;
		eatTimer *= 2;
	}

	public override void Init(int num, ImmutableDataChallenge mode) {
		type = CustomerTypes.CoolKid;
		base.Init(num, mode);
	}

	public override void GoToPlayArea(Vector3 playAreaSpot, int spotIndex, int deltaSatisfaction){
		UpdateSatisfaction(-1);
		//thoughtBubble.SetActive(true);
		//Invoke("DisableThoughtBubble", 3f);
        base.GoToPlayArea(playAreaSpot, spotIndex, deltaSatisfaction);
	}

	public void Disturbed() {
		UpdateSatisfaction(-1);
	}

	//public void DisableThoughtBubble() {
	//	thoughtBubble.SetActive(false);
	//}
}
