using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDAnimator : Singleton<HUDAnimator> {
	public GameObject coin;
	public GameObject coinFlyPrefab;
	public float coinTravelTime;
	public int currCash;
	public int target;
	public int difference;
	private Vector3 spawnPos;

	public Image tierBar;
	public Text tierText;

	void Start(){
		coin.GetComponentInChildren<Text>().text = DataManager.Instance.GameData.Cash.CurrentCash.ToString();

		
//		Vector2 fullBarSize = tierBar.rectTransform.sizeDelta;
		// HACK take care of this for other scenes
//		float percentage = DataLoaderTiers.GetPercentProgressInTier(DataManager.Instance.GameData.Cash.LastSeenTotalCash);
//		tierBar.rectTransform	// TODO Finish this
	}

	#region Coin Animation
	public void CoinsEarned(int amount, Vector3 floatyPosition){
		CalculateCoins(amount, new Vector3(500,500,0), floatyPosition);
	}

	public void CalculateCoins(int amount, Vector3 startPos, Vector3 floatyPos){
		difference = amount;
		currCash = DataManager.Instance.GameData.Cash.TotalCash;
		DataManager.Instance.GameData.Cash.TotalCash = currCash + amount;
		target = DataManager.Instance.GameData.Cash.TotalCash;
		spawnPos = startPos;
		// first we generate a coin and have it path to the hud
		//TODO generate coin
		// after we start a corutine that will end when the first coin reaches the Hud
		StartCoroutine("ChangeMoney");
		StartCoroutine ("MakeMoney");
		ParticleUtils.PlayMoneyFloaty(floatyPos,amount);
		// then we will keep spawning coins as the hud changes
		// the last coin should reach about the same time as the hud reaches it's target
	}

	private IEnumerator MakeMoney(){
		for(int i = 0; i < difference / 15; i++){
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

	private IEnumerator ChangeMoney(){
		yield return new WaitForSeconds(coinTravelTime);
		int step = 2;
		if(difference < 0){
			step = -2;
		}
		while (currCash != DataManager.Instance.GameData.Cash.TotalCash){
			if(difference > 0){
				currCash = Mathf.Min(currCash+step, target);
			}
			else{
				currCash = Mathf.Max(currCash+step, target);
			}
			coin.GetComponentInChildren<Text>().text = currCash.ToString();
			// wait one frame
			yield return 0;
		}
	}
	#endregion

	#region Tier Animation
//	public void PercentToShow
	#endregion
}
