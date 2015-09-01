using UnityEngine;
using System.Collections;
using UnityEngine.Analytics;
using System.Collections.Generic;

/// <summary>
/// Transition manager
/// Only used for scene transitions, make sure each scene
/// finishes what it needs to save before transitioning
/// </summary>
public class TransitionManager : Singleton<TransitionManager> {
	public void TransitionScene(string scene){
		Analytics.CustomEvent("Change scene from: " + Application.loadedLevelName,new Dictionary<string, object>{});
		Analytics.CustomEvent("Change scene to: " + scene,new Dictionary<string, object>{});
		Application.LoadLevel(scene);
	}
}
