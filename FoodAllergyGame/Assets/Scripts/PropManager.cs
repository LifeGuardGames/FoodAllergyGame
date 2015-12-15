using UnityEngine;
using System.Collections;

public class PropManager : Singleton<PropManager> {

	public int minimumSpawnTier = 0;

	public GameObject propRandomLeft;
	public GameObject propRandomRight;

	public GameObject propEventLeft;
	public GameObject propEventRight;

	public GameObject propGrowthLeft;
	public GameObject propGrowthRight;

	public void InitProps() {
		if(TierManager.Instance.Tier >= minimumSpawnTier) {
			PropNodeRandom();
			PropNodeEvent();
		}
	}

	private void PropNodeRandom() {
		ImmutableDataPropRandom propRandom = DataLoaderPropsRandom.GetRandomData();

		// Choose between right or left node spawning
		float xScale = 1f;
		GameObject spawnLocation = propRandomLeft;
		if(UnityEngine.Random.Range(0, 2) == 1) {
			// Left direction
			xScale = -1f;
			spawnLocation = propRandomRight;
		}
		GameObject prefab = Resources.Load(propRandom.PrefabName) as GameObject;
		GameObject go =  GameObjectUtils.AddChildWithPosition(spawnLocation, prefab);
		go.transform.localScale = new Vector3(xScale, 1f, 1f);
	}

	private void PropNodeEvent() {
		//ImmutableDataEvents data = DataLoaderEvents.GetData(DataManager.Instance.GetEvent());
		//GameObjectUtils.AddChild(propEventLeft,data.eventPropLeft);
		//GameObjectUtils.AddChild(propEventRight, data.eventPropRight);
	}

	private void PropNodeGrowth() {
		//set the sprite according to the difference in tiers
		//GameObjectUtils.AddChild(propGrowthLeft, Resources.Load(DataLoaderGrowthNode.GetPropsByTier(TierManager.Instance.Tier,"Tree").ID)as GameObject);
	}
}
