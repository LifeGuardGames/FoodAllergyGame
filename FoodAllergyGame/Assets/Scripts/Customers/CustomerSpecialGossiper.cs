using UnityEngine;
using System.Collections;

public class CustomerSpecialGossiper : MonoBehaviour {

	public CustomerAnimController customerAnim; // handles animations
	private int tableNum;
	private GossiperMode mod = GossiperMode.None;
	private Vector3 startPosition;

	public void init(int _mod) {
		switch(_mod) {
			case 1:
				mod = GossiperMode.TableSmasher;
				break;
			case 2:
				mod = GossiperMode.Eater;
				break;
			case 3:
				mod = GossiperMode.Blackout;
				break;
			case 4:
				mod = GossiperMode.All;
				break;
		}
		customerAnim.SetWaitingInLine();
		startPosition = transform.position;
		Gossip();
	}

	public void Gossip() {
		int rand = UnityEngine.Random.Range(0, 4);
		//Debug.Log ("Goissping " + rand.ToString());
		if(!RestaurantManager.Instance.GetTable(rand).isGossiped && RestaurantManager.Instance.GetTable(rand).inUse) {
			transform.SetParent(RestaurantManager.Instance.GetTable(rand).Node.transform);
			transform.localPosition = Vector3.zero;
			RestaurantManager.Instance.GetTable(rand).isGossiped = true;
			tableNum = rand;
			if(mod != GossiperMode.None) {
				StartCoroutine("Annoy");
			}
		}
		else {
			StartCoroutine(WaitAFew());
		}
	}
	public void GoAway() {
		transform.position = startPosition;
		StartCoroutine(WaitAFew());
		StopCoroutine("Annoy");
	}

	IEnumerator WaitAFew() {
		yield return new WaitForSeconds(5.0f);
		Gossip();
	}

	IEnumerator Annoy() {
		yield return new WaitForSeconds(5.0f);
		if(mod == GossiperMode.TableSmasher) {
			if(RestaurantManager.Instance.GetTable(tableNum).seat.GetChild(0).GetComponent<CustomerTableSmasher>() != null) {
				RestaurantManager.Instance.GetTable(tableNum).seat.GetChild(0).GetComponent<CustomerTableSmasher>().Annoyed();
				GoAway();
            }
        }
		else if(mod == GossiperMode.Eater) {
			if(RestaurantManager.Instance.GetTable(tableNum).seat.GetChild(0).GetComponent<CustomerEater>() != null) {
				RestaurantManager.Instance.GetTable(tableNum).seat.GetChild(0).GetComponent<CustomerEater>().pastBehav = RestaurantManager.Instance.GetTable(tableNum).seat.GetChild(0).GetComponent<CustomerEater>().currBehav;
                RestaurantManager.Instance.GetTable(tableNum).seat.GetChild(0).GetComponent<CustomerEater>().Annoyed();
				GoAway();
            }
		}
		else if(mod == GossiperMode.Blackout) {
			if(RestaurantManager.Instance.GetTable(tableNum).seat.GetChild(0).GetComponent<CustomerBlackOut>() != null) {
				RestaurantManager.Instance.GetTable(tableNum).seat.GetChild(0).GetComponent<CustomerBlackOut>().Annoyed();
				GoAway();
            }
		}
		else if (mod == GossiperMode.All) {
			if(RestaurantManager.Instance.GetTable(tableNum).seat.GetChild(0).GetComponent<Customer>() != null) {
				Debug.Log("Annoy");
				RestaurantManager.Instance.GetTable(tableNum).seat.GetChild(0).GetComponent<Customer>().Annoyed();
				GoAway();
            }
		}
	}

}
