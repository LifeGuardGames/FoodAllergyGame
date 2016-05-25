using UnityEngine;

public class CustomerCoolKid : Customer {
	public GameObject thoughtBubble;

	public override void Init(int num, ImmutableDataEvents mode) {
		base.Init(num, mode);
		type = CustomerTypes.CoolKid;
	}

	public override void Init(int num, ImmutableDataChallenge mode) {
		base.Init(num, mode);
		type = CustomerTypes.CoolKid;
	}

	public override void GoToPlayArea(Vector3 playAreaSpot, int spotIndex, int deltaSatisfaction){
		satisfaction--;
		//thoughtBubble.SetActive(true);
		//Invoke("DisableThoughtBubble", 3f);
        base.GoToPlayArea(playAreaSpot, spotIndex, deltaSatisfaction);
	}

	//public void DisableThoughtBubble() {
	//	thoughtBubble.SetActive(false);
	//}
}
