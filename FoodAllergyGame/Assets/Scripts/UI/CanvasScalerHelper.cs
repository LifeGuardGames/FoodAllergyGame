using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class CanvasScalerHelper : Singleton<CanvasScalerHelper> {
	private CanvasScaler canvasScaler;
	private Vector2 canvasScreenScale;
	private bool canvasScaleInitialized = false;

	void Start() {
		canvasScaler = GetComponent<CanvasScaler>();
	}

	public Vector2 GetCanvasScreenScale() {
		if(!canvasScaleInitialized) {
			canvasScaleInitialized = true;
			canvasScreenScale = new Vector2(canvasScaler.referenceResolution.x / Screen.width,
				canvasScaler.referenceResolution.y / Screen.height);
		}
		return canvasScreenScale;
	}
}
