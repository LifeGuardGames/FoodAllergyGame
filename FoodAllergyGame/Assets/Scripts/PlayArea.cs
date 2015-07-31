using UnityEngine;
using System.Collections;

public class PlayArea : MonoBehaviour, IWaiterSelection {

	public int maxLimit;
	private int currCus;

	// Use this for initialization
	void Start () {
		maxLimit = 2;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void OnClicked(){
		if(Waiter.Instance.currentLineCustomer != null && currCus < maxLimit){
			Waiter.Instance.currentLineCustomer.transform.localScale = Vector3.one;
			currCus++;
			StartCoroutine("PlayTime");
			Waiter.Instance.currentLineCustomer.GetComponent<Customer>().Playing();
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
