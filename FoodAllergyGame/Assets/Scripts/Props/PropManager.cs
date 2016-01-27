using UnityEngine;
using System.Collections;

public class PropManager : Singleton<PropManager> {
	public int minimumSpawnTier = 0;
	public GameObject propRandomOrbit;	// Random props are space props that orbit (rotation) the planetoid

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
		BroadcastMessage("GrowthPropInit");
	}

	private void SpawnPropNodeEvent() {
		
	}
}
