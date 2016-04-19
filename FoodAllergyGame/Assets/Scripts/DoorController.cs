using UnityEngine;

/// <summary>
/// All calls from RestaurantUIManager
/// </summary>
public class DoorController : MonoBehaviour {
	public TweenToggleDemux doorDemux;
	public SpriteRenderer skyTopLayer;
	public TweenToggle doorLockdownTween;

	private Color skyTopLayerColor;

	void Awake() {
		skyTopLayerColor = skyTopLayer.color;
	}

	public void ResetDayAlpha() {
		skyTopLayer.color = skyTopLayerColor;
	}

	public void DayAlphaTweenUpdate(float percentage) {
		skyTopLayer.color = new Color(skyTopLayerColor.r, skyTopLayerColor.g, skyTopLayerColor.b, 1f - percentage);
	}

	public void OpenAndCloseDoor(){
		doorDemux.Hide();
	}

	private void CloseDoor(){
		doorDemux.Show();
	}

	// End of day restaurant, lock the door
	public void LockdownDoor() {
		doorLockdownTween.Show();
    }
}
