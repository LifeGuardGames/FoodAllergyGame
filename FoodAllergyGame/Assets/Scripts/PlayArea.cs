using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayArea : Singleton<PlayArea>, IWaiterSelection {

	public List<Transform> spotList;			// This needs to be populated, all data derived from here
	public int deltaSatisfaction = 0;			// Changes to the customers heart

	private int maxSpots;
	public float timeMultiplier = 1.0f;
	public int breakdownChance = 0;
	public bool isBroken;
	private List<bool> spotAvailabilityList;	// Populated at runtime, keep track of spot occupancy
	public GameObject highLightSpot1;
	public GameObject highLightSpot2;
	public ParticleSystem doneParticle;
	public bool cantLeave = false;
	public Animator animator;       // Used for clicking

	void Start(){
		maxSpots = spotList.Count;

		// Populate the spot availability list set them all to true
		spotAvailabilityList = new List<bool>();
		for(int i = 0; i < maxSpots; i++){
			spotAvailabilityList.Add(true);
		}
		if(DataManager.Instance.GetChallenge() == "ChallengeTut1") {
			spotAvailabilityList[0] = false;
			spotAvailabilityList[1] = false;
		}
		TurnOffHighLights();
    }

	public void HighLightSpots(){
		if(spotAvailabilityList[0] == true && DataManager.Instance.GetChallenge() != "ChallengeTut1"){
			highLightSpot1.SetActive(true);
		}
		if(spotAvailabilityList.Count > 1 && DataManager.Instance.GetChallenge() != "ChallengeTut1") {
			if(spotAvailabilityList[1] == true && highLightSpot2 != null) {
				highLightSpot2.SetActive(true);
			}
		}
	}

	public void TurnOffHighLights(){
		highLightSpot1.SetActive(false);
		if(highLightSpot2 != null) {
			highLightSpot2.SetActive(false);
		}
	}

	public void OnClicked(){
		int availableSpotIndex = spotAvailabilityList.IndexOf(true);
		if(Waiter.Instance.CurrentLineCustomer != null && availableSpotIndex != -1 && !isBroken){
			Customer selectedCustomer = Waiter.Instance.CurrentLineCustomer.GetComponent<Customer>();
			selectedCustomer.transform.localScale = Vector3.one;

			Vector3 playAreaSpot = spotList[availableSpotIndex].position;
			spotAvailabilityList[availableSpotIndex] = false;

			selectedCustomer.GoToPlayArea(playAreaSpot, availableSpotIndex, deltaSatisfaction);

			// Turn off the active customer highlights
			RestaurantManager.Instance.CustomerLineSelectHighlightOff();

			AudioManager.Instance.PlayClip("ArcadePlay");
		}
	}

	public bool IsQueueable(){
		return false;
	}

	public void OnWaiterArrived(){
	}

	public virtual void OnPressAnim() {
		animator.SetTrigger("ClickPulse");
	}

	public void AddQueueUI() {
	}

	public void UpdateQueueUI(int order) {
	}

	public void DestroyQueueUI() {
	}

	public void DestroyAllQueueUI() {

	}

	// Called from Customer.PlayTime
	// Play time ended, reset the availability
	public void EndPlayTime(int spotIndex){
		spotAvailabilityList[spotIndex] = true;
		if(breakdownChance > 0){
			if(UnityEngine.Random.Range(0,10) < breakdownChance){
				isBroken = true;
				StartCoroutine("RepairProtocal");
			}
		}
		AudioManager.Instance.PlayClip("ArcadeOver");
		doneParticle.Play();
	}

	IEnumerator RepairProtocal(){
		yield return new WaitForSeconds(5.0f);
		isBroken = false;
	}

	public void OpenUpSpots() {
		spotAvailabilityList[0] = true;
		spotAvailabilityList[1] = true;
	}
}
