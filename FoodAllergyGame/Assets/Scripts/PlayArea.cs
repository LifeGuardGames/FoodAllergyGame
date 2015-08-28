using UnityEngine;
using System.Collections;

public class PlayArea : MonoBehaviour, IWaiterSelection {

	public int maxLimit;
	private int currCus;

	// Use this for initialization
	void Start () {
		maxLimit = 2;
		this.gameObject.SetActive(Constants.GetConstant<bool>("PlayAreaOn"));
		//ImmutableDataDecoItem _sprite = DataLoaderDecoItem.GetData(RestaurantManager.Instance.GetCurrentSprite(DecoTypes.PlayArea));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void OnClicked(){
		if(Waiter.Instance.CurrentLineCustomer != null && currCus < maxLimit){
			Waiter.Instance.CurrentLineCustomer.transform.localScale = Vector3.one;
			currCus++;
			StartCoroutine("PlayTime");
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
	}
}
