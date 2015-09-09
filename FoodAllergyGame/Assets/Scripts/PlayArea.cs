using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayArea : MonoBehaviour, IWaiterSelection {

	public int maxLimit;
	private int currCus;
	public Transform spot1;
	public Transform spot2;
	private bool spot1Available = true;
	private bool spot2Available = true;
	private List<bool> availability;

	public SpriteRenderer spriteRenderer;
	
	void Start () {
		maxLimit = 2;
		availability = new List<bool>();

		if(DataManager.Instance.IsDebug){
			gameObject.SetActive(Constants.GetConstant<bool>("PlayAreaOn"));
		}
		else{
			ImmutableDataDecoItem decoData = DataManager.Instance.GetActiveDecoData(DecoTypes.PlayArea);
			if(decoData.SpriteName == "None"){
				gameObject.SetActive(false);
			}
			else{
				spriteRenderer.sprite = SpriteCacheManager.GetDecoSpriteData(decoData.SpriteName);
			}
		}
	}

	public void OnClicked(){
		if(Waiter.Instance.CurrentLineCustomer != null && currCus < maxLimit){
			Waiter.Instance.CurrentLineCustomer.transform.localScale = Vector3.one;
			currCus++;
			StartCoroutine("PlayTime");
			Vector3 playAreaSpot;
			if(spot1Available){
				playAreaSpot = spot1.position;
				spot1Available = false;
				availability.Add(spot1Available);
			}
			else{
				playAreaSpot = spot2.position;
				spot2Available = false;
				availability.Add(spot2Available);
			}
			Waiter.Instance.CurrentLineCustomer.GetComponent<Customer>().GoToPlayArea(playAreaSpot);
		}
	}

	public bool IsQueueable(){
		return false;
	}

	public void OnWaiterArrived(){
	}

	private IEnumerator PlayTime(){
		yield return new WaitForSeconds(10.0f);
		currCus--;
		availability[0] = true;
		availability.RemoveAt(0);
	}
}
