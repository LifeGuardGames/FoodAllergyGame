using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DecoAdder : MonoBehaviour {
	public DecoTypes type;

	// Use this for initialization
	void Start () {
		DecoManager.Instance.SceneObjects.Add(type, this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
