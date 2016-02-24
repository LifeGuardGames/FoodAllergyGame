using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour {

	public float time = 0.0f;
	public Text counter;
	
	// Update is called once per frame
	void Update () {
		if(!EpiPenGameManager.Instance.isGameover) {
			time += Time.deltaTime;
			float min = Mathf.Floor(time / 60);
			float seconds = Mathf.Floor(time % 60);
			counter.text = min.ToString("00") + " : " + Mathf.RoundToInt(seconds).ToString("00");
		}
	}
}
