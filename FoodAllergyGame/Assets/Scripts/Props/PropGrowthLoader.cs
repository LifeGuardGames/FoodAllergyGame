using UnityEngine;
using System.Collections;

public class PropGrowthLoader : MonoBehaviour {

	public string growthPropKey;

	public void PropGrowthInit() {
		// Check the key and the condition for it
		ImmutableDataPropGrowth propGrowthData = DataLoaderPropGrowth.GetPropsByTier(TierManager.Instance.CurrentTier, growthPropKey);
		if(propGrowthData != null) {
			GameObject go = Resources.Load(propGrowthData.PrefabName) as GameObject;
			GameObjectUtils.AddChild(gameObject, go);
		}
	}
}
