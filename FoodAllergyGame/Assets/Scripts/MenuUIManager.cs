using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuUIManager : MonoBehaviour {

	public GameObject button1;
	public GameObject button2;
	public GameObject button3;
	public List <ImmutableDataFood> choices;
	public int tableNum;
	public GameObject order;

	void Start(){
		choices = new List<ImmutableDataFood>();
		//order = new GameObject ();
	}

	public void ShowChoices(List <ImmutableDataFood> foodChoices, int table){
		choices = foodChoices;
		tableNum = table;
		Debug.Log (foodChoices[0].ID.ToString());
		button1.GetComponent<Image>().sprite = Resources.Load(foodChoices[0].SpriteName)as Sprite;
		button2.GetComponent<Image>().sprite = Resources.Load(foodChoices[1].SpriteName)as Sprite;
		//button3.GetComponent<Image>().sprite = Resources.Load(foodChoices[2].SpriteName)as Sprite;
		button1.SetActive(true);
		button2.SetActive(true);
		//button3.SetActive(true);

	}
	public void ProduceOrder(int choice){
		button1.SetActive(false);
		button2.SetActive(false);
		GameObject orderObj = Instantiate(order,new Vector3 (0,0,0), order.transform.rotation)as GameObject;
		Debug.Log (choices[choice].ID.ToString());
		orderObj.GetComponent<Order>().Init(choices[choice].ID,tableNum);
		Waiter.Instance.writeDownOrder(orderObj);
	}
}
