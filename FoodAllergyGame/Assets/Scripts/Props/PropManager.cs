using UnityEngine;

/// <summary>
/// Initialize props in this class.
/// Props are objects that fill the StartScene space with no real function.
/// Their function is to give the world a sense of randomness or growth depending on the prop type.
/// </summary>
public class PropManager : Singleton<PropManager> {
	public int minimumSpawnTier = 0;
	public GameObject propRandomOrbit;  // Random props are space props that orbit (rotation) the planetoid
	public GameObject propGrowthParent;	// Growth props get more busy with each tier up

	public void InitProps() {
		if(TierManager.Instance.CurrentTier >= minimumSpawnTier) {
			SpawnPropNodeRandom();
			SpawnPropNodeGrowth();
			SpawnPropNodeEvent();
		}
	}

	private void SpawnPropNodeRandom() {
		ImmutableDataPropRandom propRandom = DataLoaderPropsRandom.GetRandomData();

		// Choose between right or left node spawning
		float xScale = 1f;
		if(UnityEngine.Random.Range(0, 2) == 1) {
			// Left direction
			xScale = -1f;
		}
		GameObject prefab = Resources.Load(propRandom.PrefabName) as GameObject;
		GameObject go =  GameObjectUtils.AddChildWithPosition(propRandomOrbit, prefab);
		go.transform.localScale = new Vector3(xScale, 1f, 1f);
	}

	// Broadcast a message telling all prop growth nodes needs to initialize themselves
	private void SpawnPropNodeGrowth() {
		propGrowthParent.BroadcastMessage("PropGrowthInit", SendMessageOptions.DontRequireReceiver);
	}

	private void SpawnPropNodeEvent() {
		
	}
}
