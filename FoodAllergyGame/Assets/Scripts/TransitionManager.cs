using UnityEngine;
using System.Collections;

/// <summary>
/// Transition manager
/// Only used for scene transitions, make sure each scene
/// finishes what it needs to save before transitioning
/// </summary>
public class TransitionManager : Singleton<TransitionManager> {
	public void TransitionScene(string scene){
		Application.LoadLevel(scene);
	}
}
