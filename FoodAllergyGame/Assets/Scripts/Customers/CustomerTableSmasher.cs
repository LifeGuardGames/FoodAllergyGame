using UnityEngine;
using System.Collections;

public class CustomerTableSmasher : Customer {

	public override void Init (int num, ImmutableDataEvents mode) {
		type = CustomerTypes.TableSmasher;
		base.Init(num, mode);
	}

	public override void Init(int num, ImmutableDataChallenge mode) {
		type = CustomerTypes.TableSmasher;
		base.Init(num, mode);
	}

	public override void UpdateSatisfaction(int delta) {
		base.UpdateSatisfaction(delta);
		if(satisfaction > 0 && delta < 0) {
			NotifyLeave();
		}
	}
	//void OnGUI() {
	//	if(GUI.Button(new Rect(100, 100, 100, 100), "SMASHHH")) {
	//		state = CustomerStates.WaitForFood;
	//		NotifyLeave();
	//	}
	//}
}

