using UnityEngine;
using System.Collections;

public class CustomerAnimationControllerGossiper : CustomerAnimController {

	public void Gossip() {
		skeletonAnim.state.SetAnimation(0, "Talking", false).Complete +=
			delegate {
				Gossip();
			};
	}
}
