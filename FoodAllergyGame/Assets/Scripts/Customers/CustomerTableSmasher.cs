using UnityEngine;
using System.Collections;

public class CustomerTableSmasher : Customer {

	public override void Init (int num, ImmutableDataEvents mode) {
		base.Init (num, mode);
		type = CustomerTypes.TableSmasher;
	}

	public override void Init(int num, ImmutableDataChallenge mode) {
		base.Init(num, mode);
		type = CustomerTypes.TableSmasher;
	}


	//void OnGUI() {
	//	if(GUI.Button(new Rect(100, 100, 100, 100), "SMASHHH")) {
	//		state = CustomerStates.WaitForFood;
	//		NotifyLeave();
	//	}
	//}
}

