using UnityEngine;
using System.Collections;

public class TestingPosition : MonoBehaviour {

	void Awake(){
		Debug.Log(gameObject.GetComponent<RectTransform>().anchoredPosition3D);
	}

	void Start () {
		Debug.Log(gameObject.GetComponent<RectTransform>().anchoredPosition3D);
	}
	
	void Update () {
		Debug.Log(gameObject.GetComponent<RectTransform>().anchoredPosition3D);
	}
}
