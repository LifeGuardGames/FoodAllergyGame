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
		//LoadObject (DataLoaderRandomNode.GetRandomData());
	}

	private void PropNodeGrowth() {
		//set the sprite according to the difference in tiers
		//LoadObject (DataLoaderGrowthNode.GetDataWithinTierRange());
	}
}
