using UnityEngine;
using System.Collections;

public class Medic : Singleton<Medic> {

	public GameObject startPos;
	public int medicCost;
	public int MedicCost{
		get{return medicCost;}
	}

	private Vector3 firstCustomerPositionAux;
	private Vector3 medicOffset = new Vector3(-100f, 0f, 0f);

	public GameObject thoughtBubbleParent;
	public SpriteRenderer thoughtBubbleSkin;

	public Color defaultSkinColor;
	public Color regularSkinColor;
	public Color impatientSkinColor;
	public Color coolKidSkinColor;
	public Color tableSmasherSkinColor;

	// Have the medic leave its base before moving to customers	
	public void SetOutFromHome(Vector3 customerPosition){
		AudioManager.Instance.PlayClip("MedicEnter");
		firstCustomerPositionAux = customerPosition + medicOffset;
		LeanTween.cancel(gameObject);
		LeanTween.move(gameObject, new Vector3(transform.position.x + 300f, transform.position.y, transform.position.z), 1.0f)
			.setOnComplete(OnSetOutFromHomeFinished);
	}

	private void OnSetOutFromHomeFinished(){
		MoveToLocation(firstCustomerPositionAux);
	}

	// Move toward the next customer on the list
	public void MoveToLocation(Vector3 customer){
		this.gameObject.GetComponentInChildren<Animation>().Play("MedicCartwheel");
		//If the waiter is already at its location, just call what it needs to call
		if(transform.position == customer){
			SaveCustomer();
		}
	
		// Otherwise, move to the location and wait for callback
		else{
			LeanTween.cancel(gameObject);
			LeanTween.move(gameObject, customer, 1.0f)
			.setEase(LeanTweenType.easeInOutQuad)
				.setOnComplete(SaveCustomer);
		}
	}
	// if no one is sick theres no need for the medic to leave his office so he goes back
	public void MoveHome(){
		this.gameObject.GetComponentInChildren<Animation>().Play("MedicReverseCartwheel");
		LeanTween.cancel(gameObject);
		LeanTween.move(gameObject, startPos.transform.position, 1.0f)
			.setEase(LeanTweenType.easeInOutQuad);
	}

	// Stops the customer from having an allergy attack
	public void SaveCustomer(){
		StartCoroutine("TreatCustomer");

	}

	public void BillRestaurant (int expense){
		medicCost += expense;
	}

	private IEnumerator TreatCustomer(){
		// Saved the person in time
		if(RestaurantManager.Instance.sickCustomers.Count > 0){
			ShowThoughtBubble();
			RestaurantManager.Instance.sickCustomers[0].GetComponent<Customer>().saved = true;
			yield return new WaitForSeconds(3.0f);
			RestaurantManager.Instance.sickCustomers[0].GetComponent<Customer>().currBehav.Reason();
		}
		// Missed the person... oh well
		else{
			HideThoughtBubble();
			MoveHome();
		}

		// Check if there are other customers that needs to be saved
		if(	RestaurantManager.Instance.sickCustomers.Count == 0){
			HideThoughtBubble();
			MoveHome();
		}
		else{
			HideThoughtBubble();
			MoveToLocation(RestaurantManager.Instance.sickCustomers[0].transform.position + medicOffset);
		}
	}

	private void ShowThoughtBubble() {
		// Change the skin type to that of the customer by type
		CustomerTypes type = RestaurantManager.Instance.sickCustomers[0].GetComponent<Customer>().type;
		switch(type) {
			case CustomerTypes.Normal:
				thoughtBubbleSkin.color = regularSkinColor;
                break;
			case CustomerTypes.TableSmasher:
				thoughtBubbleSkin.color = tableSmasherSkinColor;
				break;
			case CustomerTypes.Impatient:
				thoughtBubbleSkin.color = impatientSkinColor;
				break;
			case CustomerTypes.CoolKid:
				thoughtBubbleSkin.color = coolKidSkinColor;
				break;
			default:
				thoughtBubbleSkin.color = defaultSkinColor;
				break;
		}
		thoughtBubbleParent.SetActive(true);
	}

	private void HideThoughtBubble() {
		thoughtBubbleParent.SetActive(false);
	}
}
