using UnityEngine;

public class DecoTabController : MonoBehaviour {
	public DecoTypes deco;

	void Start() {
		Debug.Log("UNlocked ? " + DecoManager.Instance.IsCategoryUnlocked(deco));
		if(!DecoManager.Instance.IsCategoryUnlocked(deco)) {
			gameObject.SetActive(false);
		}
	}
}
