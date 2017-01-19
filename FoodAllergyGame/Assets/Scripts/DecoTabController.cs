using UnityEngine;

public class DecoTabController : MonoBehaviour {
	public DecoTabTypes decoTabType;

	void Start() {
		if(!DecoManager.Instance.IsTabCategoryUnlocked(decoTabType)) {
			gameObject.SetActive(false);
		}
	}
}
