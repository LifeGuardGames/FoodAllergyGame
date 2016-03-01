using UnityEngine;

public class DecoTabController : MonoBehaviour {
	public DecoTypes deco;

	void Start() {
		if(!DecoManager.Instance.IsCategoryUnlocked(deco)) {
			gameObject.SetActive(false);
		}
	}
}
