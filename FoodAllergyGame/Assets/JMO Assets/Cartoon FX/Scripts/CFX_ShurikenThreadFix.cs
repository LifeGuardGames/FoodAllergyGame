using UnityEngine;
using System.Collections;

// Cartoon FX  - (c) 2015, Jean Moreno

// Drag/Drop this script on a Particle System (or an object having Particle System objects as children) to prevent a Shuriken bug
// where a system would emit at its original instantiated position before being translated, resulting in particles in-between
// the two positions.
// Possibly a threading bug from Unity (as of 3.5.4)

public class CFX_ShurikenThreadFix : MonoBehaviour
{
	private ParticleSystem[] systems;
	
	void OnEnable()
	{
		systems = GetComponentsInChildren<ParticleSystem>();

#pragma warning disable 0618
		foreach(ParticleSystem ps in systems)
			ps.enableEmission = false;
#pragma warning restore 0618

		StartCoroutine("WaitFrame");
	}
	
	IEnumerator WaitFrame()
	{
		yield return null;

#pragma warning disable 0618
		foreach(ParticleSystem ps in systems)
		{
			ps.enableEmission = true;
			ps.Play(true);
		}
#pragma warning restore 0618
	}
}