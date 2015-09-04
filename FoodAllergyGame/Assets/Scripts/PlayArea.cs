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


	// Use this for initialization
	void Start () {
		maxLimit = 2;
		if(DataManager.Instance.IsDebug){
			this.gameObject.SetActive(Constants.GetConstant<bool>("PlayAreaOn"));
		}
		availability = new List<bool>();
		ImmutableDataDecoItem _sprite = DataManager.Instance.GetActiveDecoData(DecoTypes.PlayArea);
		if(_sprite.SpriteName == "None"){
			this.gameObject.SetActive(false);
		}
		else{
			this.GetComponent<SpriteRenderer>().sprite = SpriteCacheManager.GetDecoSpriteData(_sprite.SpriteName);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void OnClicked(){
		if(Waiter.Instance.CurrentLineCustomer != null && currCus < maxLimit){
			Waiter.Instance.CurrentLineCustomer.transform.localScale = Vector3.one;
			currCus++;
			StartCoroutine("PlayTime");
			if(spot1Available){
				Waiter.Instance.CurrentLineCustomer.transform.position = spot1.position;
				spot1Available = false;
				availability.Add(spot1Available);
			}
			else{
				Waiter.Instance.CurrentLineCustomer.transform.position = spot2.position;
				spot2Available = false;
				availability.Add(spot2Available);
			}
			Waiter.Instance.CurrentLineCustomer.GetComponent<Customer>().Playing();
		}
	}

	public bool IsQueueable(){
		return false;
	}

	public void OnWaiterArrived(){
	}

	IEnumerator PlayTime(){
		yield return new WaitForSeconds(10.0f);
		currCus--;
		availability[0] = true;
		availability.RemoveAt(0);
	}
}
