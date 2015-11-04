using UnityEngine;
using System.Collections;

public class DecoTabController : MonoBehaviour {
	public DecoTypes deco;
	// Use this for initialization
	void Start () {
		if(!TierManager.Instance.IsCategoryUnlocked(deco)) {
			this.gameObject.SetActive(false);
		}
	}
}
