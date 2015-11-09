using UnityEngine;
using System.Collections;

public class PropManager : MonoBehaviour {

	public GameObject propEventLeft;
	public GameObject propEventRight;
	public GameObject propRandomLeft;
	public GameObject propRandomRight;
	public GameObject propGrowthLeft;
	public GameObject propGrowthRight;

	public void SetUpScene() {
		PropNodeEvent();
		PropNodeRandom();
	}

	private void PropNodeEvent() {
		ImmutableDataEvents data = DataLoaderEvents.GetData(DataManager.Instance.GetEvent());
		//GameObjectUtils.AddChild(propEventLeft,data.eventPropLeft);
		//GameObjectUtils.AddChild(propEventRight, data.eventPropRight);
	}

	private void PropNodeRandom() {
		// pull a random prop and assign new 
		GameObjectUtils.AddChild(propRandomLeft, Resources.Load(DataLoaderRandomNode.GetData("Random0"+ Random.Range(0,10).ToString()).ID)as GameObject);
	}

	private void PropNodeGrowth() {
		//set the sprite according to the difference in tiers
		GameObjectUtils.AddChild(propGrowthLeft, Resources.Load(DataLoaderGrowthNode.GetPropsByTier(TierManager.Instance.Tier,"Tree").ID)as GameObject);
	}
}
