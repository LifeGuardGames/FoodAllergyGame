using UnityEngine;
using System.Collections;

public class CustomerAnimationCotrollerBlackOut : CustomerAnimController{

	public Customer blackie;

	public void BlackOut() {
		skeletonAnim.state.SetAnimation(0, "AllHeartsLost", false).Complete +=
			delegate {
				RestaurantManager.Instance.Blackout();
				blackie.DestroySelf(0.0f);
			};
	}
}
