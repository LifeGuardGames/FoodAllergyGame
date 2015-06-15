using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Order : MonoBehaviour{
	// ID of the food
	public string foodID;

	// Table number that ordered the dish used to find which table this goes too.
	public int tableNumber;

	// Is this order cooked?
	private bool isCooked;
	public bool IsCooked{
		get{
			return isCooked;
		}
		set{
			isCooked = value;
			gameObject.GetComponent<Renderer>().material = isCooked ? cookedMaterial : uncookedMaterial;
		}
	}

	public Allergies allergy;

	// TEMP
	public Material uncookedMaterial;
	public Material cookedMaterial;


	// Initialize the order when it is first spawned
	public void Init(string foodID, int tableNumber, Allergies _allergy){
		this.foodID = foodID;
		this.tableNumber = tableNumber;
		isCooked = false;
		allergy = _allergy;
		this.gameObject.GetComponentInChildren<Text>().text = tableNumber.ToString();
	}

	public void StartCooking(){
		GetComponent<MeshRenderer>().enabled = false;
		GetComponentInChildren<Text>().enabled = false;
		StartCoroutine("Cooking");
	}

	private IEnumerator Cooking(){
		yield return new WaitForSeconds(5.0f);
		IsCooked = true;
		GetComponent<MeshRenderer>().enabled = true;
		GetComponentInChildren<Text>().enabled = true;
		GameObject.Find("Kitchen").GetComponent<KitchenManager>().Cooked(this.gameObject);
	}

	public void Canceled(){
		StopCoroutine("Cooking");
		Destroy(this.gameObject);
	}
}
