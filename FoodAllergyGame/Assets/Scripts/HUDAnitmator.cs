using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDAnitmator : Singleton<HUDAnitmator> {

	public GameObject Coin;
	public GameObject coinPrefab;
	public float coinTravelTime;
	public int currCash;
	public int target;
	public int difference;
	private Vector3 spawnPos;

	void Start(){
		Coin = GameObject.Find("Cash");
	}

	public void CoinsEarned(int amount){
		CalculateCoins(amount, new Vector3 (500,500,0));
	}

	public void CalculateCoins (int amount, Vector3 startPos){
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

		// then we will keep spawning coins as the hud changes
		// the last coin should reach about the same time as the hud reaches it's target
	}

	IEnumerator MakeMoney(){
		for(int i = 0; i < difference /15; i++){
			GameObject go;
			go = GameObjectUtils.AddChildGUI(null, coinPrefab);
			go.transform.position = spawnPos;
			go.transform.SetParent(Coin.transform.parent);
			// Addition tweening behavior
			Vector3[] path = new Vector3[4];
			path[0] = go.transform.localPosition;
			Vector3 randomPoint = GameObjectUtils.GetRandomPointOnCircumference(go.transform.localPosition, 180f);
			path[1] = randomPoint;
			path[2] = path[1];
			path[3] = (Coin.transform.GetChild(1) as RectTransform).anchoredPosition;
			
			LeanTween.moveLocal(go, path, coinTravelTime).setEase(LeanTweenType.easeInQuad).setDestroyOnComplete(true);
			//LeanTween.move(go,Coin.transform.GetChild(1).transform.position, coinTravelTime).setDestroyOnComplete(true);
			yield return new WaitForSeconds(0.1f);
		}
	}

	IEnumerator ChangeMoney(){
		yield return new WaitForSeconds(coinTravelTime);
		int step = 1;
		if(difference < 0){
			step = -1;
		}
		while (currCash != DataManager.Instance.GameData.Cash.TotalCash){
			if(difference > 0){
				currCash = Mathf.Min(currCash+step, target);
			}
			else{
				currCash = Mathf.Max(currCash+step, target);
			}
			Coin.GetComponentInChildren<Text>().text = currCash.ToString();
			// wait one frame
			yield return 0;
		}
	}
}
