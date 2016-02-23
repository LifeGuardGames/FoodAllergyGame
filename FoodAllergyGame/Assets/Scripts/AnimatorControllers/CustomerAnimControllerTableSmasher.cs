using UnityEngine;
using System.Collections;

public class CustomerAnimControllerTableSmasher : CustomerAnimController {
	public void SmashTable() {
		skeletonAnim.state.SetAnimation(0, "TableSmash", false);
		ParticleUtils.PlayTableSmashedParticle(transform.position);
	}
}
