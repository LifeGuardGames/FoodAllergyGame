using UnityEngine;
using System.Collections;

public class CustomerAnimControllerTableSmasher : CustomerAnimController {
	public void SmashTable() {
		Reset();
		skeleton.state.AddAnimation(0, "TableSmash", false, 0f);
		ParticleUtils.PlayTableSmashedParticle(transform.position);
	}
}
