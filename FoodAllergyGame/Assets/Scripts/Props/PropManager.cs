using UnityEngine;
using System.Collections;

public class PropManager : Singleton<PropManager> {
	public int minimumSpawnTier = 0;
	public GameObject propRandomOrbit;	// Random props are space props that orbit (rotation) the planetoid

	public void InitProps() {
		if(TierManager.Instance.Tier >= minimumSpawnTier) {
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

	private void SpawnPropNodeEvent() {
		//ImmutableDataEvents data = DataLoaderEvents.GetData(DataManager.Instance.GetEvent());
		//GameObjectUtils.AddChild(propEventLeft,data.eventPropLeft);
		//GameObjectUtils.AddChild(propEventRight, data.eventPropRight);
	}

	private void SpawnPropNodeGrowth() {
		//set the sprite according to the difference in tiers
		//GameObjectUtils.AddChild(propGrowthLeft, Resources.Load(DataLoaderGrowthNode.GetPropsByTier(TierManager.Instance.Tier,"Tree").ID)as GameObject);
	}
}
