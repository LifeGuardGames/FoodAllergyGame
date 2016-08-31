using UnityEngine;
public abstract class MapNode : MonoBehaviour {
	// Node as been reached, toggleAnimations
	public abstract void ToggleReached(bool isSetup);
}
