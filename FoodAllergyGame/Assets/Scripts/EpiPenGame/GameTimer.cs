using System;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour {
	public Text textCounter;

	private float timerTick = 0f;
	private bool isPaused;

	public void ResetTimer() {
		timerTick = 0f;
		isPaused = false;
    }

	public void PauseTimer() {
		isPaused = true;
	}

	public void ContinueTimer() {
		isPaused = false;
	}

	public string Report() {
		TimeSpan span = TimeSpan.FromSeconds(timerTick);
		return span.Minutes + ":" + span.Seconds;
	}
	
	void Update() {
		if(!isPaused) {
			timerTick += Time.deltaTime;
			TimeSpan span = TimeSpan.FromSeconds(timerTick);
            textCounter.text = span.Minutes + ":" + span.Seconds;
		}
	}
}
