using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDAnimator : Singleton<HUDAnimator> {
	public GameObject coin;
	public GameObject tier;
	public GameObject coinFlyPrefab;
	//public GameObject starChunkPrefab;
	//public RectTransform starChunkParent;
	public float coinTravelTime;
	public Text coinText;
	public int currentCoinsAux;
	public int targetCoinsAux;
	public int deltaCoinsAux;
	private Vector3 spawnPos;

	public Image tierBar;
	public Animation tierBarPopAnim;
	private int oldTotalCash;			// Used for beginning and for tweening, kind of hacky
	private int newTotalCash;
	private bool firstStarChunkAux;		// Toggle for first chunk finish tweening
	private Vector2 fullSizeBar;
	private NotificationQueueDataTierProgress tierCaller;

	public FireworksController fireworksController;
	
	void Start() {
		// StartScene will do its own explicit call from StartManager
		if(SceneManager.GetActiveScene().name != SceneUtils.START) {
			Initialize(CashManager.Instance.TotalCash);
		}
	}

	public void Initialize(int cashProgress) {
		coin.GetComponentInChildren<Text>().text = CashManager.Instance.CurrentCash.ToString();
		//fullSizeBar = tierBar.rectTransform.sizeDelta;
		//float percentage = DataLoaderTiers.GetPercentProgressInTier(cashProgress);
		//tierBar.rectTransform.sizeDelta = new Vector2(fullSizeBar.x * percentage, fullSizeBar.y);
	}

	#region Coin Animation
	public void CoinAnimationStart(int deltaCoins, Vector3 floatyPosition) {
		PrepCoinAnimation(deltaCoins, new Vector3(500, 500, 0), floatyPosition);
	}

	private void PrepCoinAnimation(int deltaCoins, Vector3 startPos, Vector3 floatyPos) {
		deltaCoinsAux = deltaCoins;
		currentCoinsAux = CashManager.Instance.CurrentCash + (-deltaCoins);	// Change is made internally already
		targetCoinsAux = CashManager.Instance.CurrentCash;
		spawnPos = startPos;
		// first we generate a coin and have it path to the hud
		//TODO generate coin
		// after we start a corutine that will end when the first coin reaches the Hud
		StartCoroutine("ChangeMoney");
		StartCoroutine("MakeMoney");
		ParticleAndFloatyUtils.PlayCoinFloaty(floatyPos, deltaCoins);
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

	#region Tier Animation Sequences
	public void StartStarChunkTweenSpawning(NotificationQueueDataTierProgress tierCaller, int oldTotalCash, int newTotalCash){
		// Just start the bar tweening instead of chunk flying
		//StartTierAnimation(tierCaller, oldTotalCash, newTotalCash);

		//////////////////////////////////////////
		this.tierCaller = tierCaller;
		this.oldTotalCash = oldTotalCash;
		this.newTotalCash = newTotalCash;

		//firstStarChunkAux = true;
		//int chunkCount = (newTotalCash - oldTotalCash) / 30;        // Change this for different chunk numbers
		////Debug.Log("Chunks to tween " + chunkCount + " " + (newTotalCash - oldTotalCash));
		//StartCoroutine(StartStarChunkTweenSpawningHelper(chunkCount));	// NOTE: Must show atleast one tween
	}

	//public IEnumerator StartStarChunkTweenSpawningHelper(int chunkCount){
	//	for(int i = 0; i <= chunkCount; i++){
	//		AudioManager.Instance.PlayClip("StarChunkAppear");

	//		GameObject go = GameObjectUtils.AddChildGUI(starChunkParent.gameObject, starChunkPrefab);
	//		Vector3 reference = StartManager.Instance.DinerEntranceUIController.transform.position + new Vector3(0, 100f, 0);
	//		go.transform.position = new Vector3(reference.x, reference.y, 0);
	//		go.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

	//		// Sanitize out z component
	//		Vector3 destination = new Vector3(tier.transform.position.x, tier.transform.position.y, 0);

	//		// Addition tweening behavior
	//		Vector3[] path = new Vector3[4];
	//		path[0] = go.transform.position;
	//		Vector3 randomPoint = GameObjectUtils.GetRandomPointOnCircumference(go.transform.position, 300f);
	//		path[1] = randomPoint;
	//		path[2] = path[1];
	//		path[3] = destination;

	//		LeanTween.moveLocal(go, path, 1f).setEase(LeanTweenType.easeInQuad).setOnComplete(OnStarChunkArrived).setDestroyOnComplete(true);
	//		LeanTween.rotate(go.GetComponent<RectTransform>(), UnityEngine.Random.Range(-360, 360), 1f);
	//		LeanTween.scale(go, new Vector3(1, 1, 1), 1f).setEase(LeanTweenType.easeOutBack);
	//		yield return new WaitForSeconds(0.1f);
	//	}
	//}

	//private void OnStarChunkArrived(){
	//	if(firstStarChunkAux){	// Do things when first chunk has arrived
	//		firstStarChunkAux = false;
	//		tierBarPopAnim.Play();
	//		StartTierAnimation(tierCaller, oldTotalCash, newTotalCash);
	//	}
	//	AudioManager.Instance.PlayClip("StarChunkArrive");
	//}

	/*private void StartTierAnimation(NotificationQueueDataTierProgress tierCaller, int oldTotalCash, int newTotalCash) {
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
	}*/

	//private void ChangePercentage(float amount) {
		//tierBar.rectTransform.sizeDelta = new Vector2(fullSizeBar.x * amount, fullSizeBar.y);
	//}

	// Update the old tier to the next tier up and call StartTierAnimation again
	private void OnTweenCompleteTierUp() {
		// Get the current old tier
		int currentOldTier = DataLoaderTiers.GetTierFromCash(oldTotalCash);

		// Assign the floor limit total cash value of the next tier up
		ImmutableDataTiers tierData = DataLoaderTiers.GetData("Tier" + StringUtils.FormatIntToDoubleDigitString(currentOldTier + 1));
		if(tierData != null) {
			oldTotalCash = DataLoaderTiers.GetData("Tier" + StringUtils.FormatIntToDoubleDigitString(currentOldTier + 1)).CashCutoffFloor;

			// Reset the UI
			//tierBar.rectTransform.sizeDelta = new Vector2(0f, fullSizeBar.y);

			// Do any animation effects here
			fireworksController.StartFireworks();

			// Pass that total cash into the animate function again
			//StartTierAnimation(tierCaller, oldTotalCash, newTotalCash);
		}
		else {
			Debug.LogWarning("Reached max tiers");  //TODO check max tiers
			tierCaller.Finish();
		}
	}

	// Tier sequence complete
	private void OnTweenCompleteFinishTier() {
		tierCaller.Finish();
	}
	#endregion
}
