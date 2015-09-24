using UnityEngine;
using System.Collections;

public class ParticleAndFloatyManager : Singleton<ParticleAndFloatyManager> {

	public void  PlayMoneyFloaty(Vector3 pos, int amount){
		ParticleUtils.PlayMoneyFloaty(pos,amount);
	}

	public void  PlayDecoChangePoof(Vector3 pos){
		ParticleUtils.PlayDecoChangePoof(pos);
	}

	public void  PlayHandsFullFloaty(Vector3 pos){
		ParticleUtils.PlayHandsFullFloaty(pos);
	}
}
