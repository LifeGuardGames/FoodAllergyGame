using System.Collections;
using UnityEngine;

public class AdPerfectlyFree : MonoBehaviour {
	public const string AD_KEY = "PerfectlyFree";
	public TweenToggle tweenToggle;

	public void ShowPanel() {
		// Wait for tween toggle to finish initializing
		StartCoroutine(ShowAfterFrames());
    }

	private IEnumerator ShowAfterFrames() {
		yield return 0;
		yield return 0;
		// Track Ad showing
		AnalyticsManager.Instance.AdImpression(AD_KEY);
		tweenToggle.Show();
	}

	public void HidePanel() {
		tweenToggle.Hide();
	}

	public void OnHideComplete() {
		Destroy(gameObject);
	}

	public void OnButtonClicked() {
		Debug.Log("BUTTON");
		// Track Button Clicked
		AnalyticsManager.Instance.AdConversion(AD_KEY, AdConversionType.ButtonClick);
		Application.OpenURL("http://bit.ly/2nFILnS");
	}

	public void OnAnythingClicked() {
		Debug.Log("ANYTHING");
		// Track Anything Clicked
		AnalyticsManager.Instance.AdConversion(AD_KEY, AdConversionType.PageClick);
		Application.OpenURL("http://bit.ly/2nFILnS");
	}
}
