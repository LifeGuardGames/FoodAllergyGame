using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDAnimator : Singleton<HUDAnimator> {
	public GameObject coin;
	public GameObject coinFlyPrefab;
	public float coinTravelTime;
	public Text coinText;
	public int currentCoinsAux;
	public int targetCoinsAux;
	public int deltaCoinsAux;
	private Vector3 spawnPos;

	public Image tierBar;
	public Text tierText;
	private int oldTotalCash;
	private int newTotalCash;
	private Vector2 fullSizeBar;
	private NotificationQueueDataTierProgress tierCaller;

	public FireworksController fireworksController;

	void Start() {
		coin.GetComponentInChildren<Text>().text = CashManager.Instance.CurrentCash.ToString();

		// Change the appearance of the tier bar depending on which scene it is in
		fullSizeBar = tierBar.rectTransform.sizeDelta;
		float percentage;
		int tierNumber;
		if(Application.loadedLevelName != SceneUtils.START) {
			percentage = DataLoaderTiers.GetPercentProgressInTier(CashManager.Instance.TotalCash);
			tierNumber = DataLoaderTiers.GetTierFromCash(CashManager.Instance.TotalCash);
		}
		else {
			percentage = DataLoaderTiers.GetPercentProgressInTier(CashManager.Instance.LastSeenTotalCash);
			tierNumber = DataLoaderTiers.GetTierFromCash(CashManager.Instance.LastSeenTotalCash);
		}
		tierBar.rectTransform.sizeDelta = new Vector2(fullSizeBar.x * percentage, fullSizeBar.y);
		tierText.text = tierNumber.ToString();
	}

	#region Coin Animation
	public void CashAnimationStart(int deltaCoins, Vector3 floatyPosition) {
		PrepCashAnimation(deltaCoins, new Vector3(500, 500, 0), floatyPosition);
	}

	private void PrepCashAnimation(int deltaCoins, Vector3 startPos, Vector3 floatyPos) {
		deltaCoinsAux = deltaCoins;
		currentCoinsAux = CashManager.Instance.CurrentCash + (-deltaCoins);	// Change is made internally already
		targetCoinsAux = CashManager.Instance.CurrentCash;
		spawnPos = startPos;
		// first we generate a coin and have it path to the hud
		//TODO generate coin
		// after we start a corutine that will end when the first coin reaches the Hud
		StartCoroutine("ChangeMoney");
		StartCoroutine("MakeMoney");
		ParticleUtils.PlayMoneyFloaty(floatyPos, deltaCoins);
		// then we will keep spawning coins as the hud changes
		// the last coin should reach about the same time as the hud reaches it's target
	}

	private IEnumerator MakeMoney() {
		for(int i = 0; i < deltaCoinsAux / 15; i++) {
			GameObject go;
			go = GameObjectUtils.AddChildGUI(null, coinFlyPrefab);
			go.transform.position = spawnPos;
			go.transform.SetParent(coin.transform.parent);
			// Addition tweening behavior
			Vector3[] path = new Vector3[4];
			path[0] = go.transform.localPosition;
			Vector3 randomPoint = GameObjectUtils.GetRandomPointOnCircumference(go.transform.localPosition, 180f);
			path[1] = randomPoint;
			path[2] = path[1];
			path[3] = (coin.transform.GetChild(1) as RectTransform).anchoredPosition;

			LeanTween.moveLocal(go, path, coinTravelTime).setEase(LeanTweenType.easeInQuad).setDestroyOnComplete(true);
			//LeanTween.move(go,Coin.transform.GetChild(1).transform.position, coinTravelTime).setDestroyOnComplete(true);
			yield return new WaitForSeconds(0.1f);
		}
	}

	private IEnumerator ChangeMoney() {
		yield return new WaitForSeconds(coinTravelTime);
		int step = 5;
		while(currentCoinsAux != CashManager.Instance.CurrentCash) {
			if(deltaCoinsAux > 0) {
				currentCoinsAux = Mathf.Min(currentCoinsAux += step, targetCoinsAux);
			}
			else {
				currentCoinsAux = Mathf.Max(currentCoinsAux -= step, targetCoinsAux);
			}
			coinText.text = currentCoinsAux.ToString();
			// wait one frame
			yield return 0;
		}
	}
	#endregion

	#region Tier Animation
	public void StartTierAnimation(NotificationQueueDataTierProgress tierCaller, int oldTotalCash, int newTotalCash) {
		this.tierCaller = tierCaller;
		this.oldTotalCash = oldTotalCash;
		this.newTotalCash = newTotalCash;

		float startPercentage = DataLoaderTiers.GetPercentProgressInTier(oldTotalCash);

		// If the tiers don't change, just animate it
        if(DataLoaderTiers.GetTierFromCash(oldTotalCash) == DataLoaderTiers.GetTierFromCash(newTotalCash)) {
			float endPercentage = DataLoaderTiers.GetPercentProgressInTier(newTotalCash);
			LeanTween.cancel(this.gameObject);
			LeanTween.value(this.gameObject, ChangePercentage, startPercentage, endPercentage, 1f)
				.setOnComplete(OnTweenCompleteFinishTier);
		}
		// Tiers change
		// NOTE: We do a self call on finish tier animation starting with an updated oldTotalCash
		else {
			float endPercentage = 1f;   // Full bar
			LeanTween.value(this.gameObject, ChangePercentage, startPercentage, endPercentage, 0.7f)
				.setOnComplete(OnTweenCompleteTierUp);
		}
	}

	public void ChangePercentage(float amount) {
		tierBar.rectTransform.sizeDelta = new Vector2(fullSizeBar.x * amount, fullSizeBar.y);
	}

	// Update the old tier to the next tier up and call StartTierAnimation again
	private void OnTweenCompleteTierUp() {
		// Get the current old tier
		int currentOldTier = DataLoaderTiers.GetTierFromCash(oldTotalCash);

		// Assign the floor limit total cash value of the next tier up
		ImmutableDataTiers tierData = DataLoaderTiers.GetData("Tier" + StringUtils.FormatIntToDoubleDigitString(currentOldTier + 1));
		if(tierData != null) {
			oldTotalCash = DataLoaderTiers.GetData("Tier" + StringUtils.FormatIntToDoubleDigitString(currentOldTier + 1)).CashCutoffFloor;

			// Reset the UI
			tierText.text = DataLoaderTiers.GetTierFromCash(oldTotalCash).ToString();
			tierBar.rectTransform.sizeDelta = new Vector2(0f, fullSizeBar.y);

			// Do any animation effects here
			AudioManager.Instance.PlayClip("TierUp");
			fireworksController.StartFireworks();

			// Pass that total cash into the animate function again
			StartTierAnimation(tierCaller, oldTotalCash, newTotalCash);
		}
		else {
			Debug.LogWarning("Reached max tiers");  //TODO check max tiers
		}
	}

	// Tier sequence complete
	private void OnTweenCompleteFinishTier() {
		CashManager.Instance.SyncLastSeenTotalCash();
		tierCaller.Finish();
	}
	#endregion
}
