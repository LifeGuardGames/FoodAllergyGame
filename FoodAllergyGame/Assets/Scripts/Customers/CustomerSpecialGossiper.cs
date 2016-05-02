using UnityEngine;
using System.Collections;

/// <summary>
/// Custom special customer that appears in challenge only, no regular customer logic
/// </summary>
public class CustomerSpecialGossiper : MonoBehaviour {

	public MeshRenderer customerMeshRenderer;
	public CustomerUIController customerUI;
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
		transform.position = new Vector3(-475, 656, 0);
		startPosition = transform.position;
		Gossip();
	}

	public void Gossip() {
		if(RestaurantManager.Instance.actTables > 0) {
			int rand = UnityEngine.Random.Range(0, 4);
			while(RestaurantManager.Instance.GetTable(rand) == null) {
				rand = UnityEngine.Random.Range(0, 4);
			}
			//Debug.Log ("Goissping " + rand.ToString());
			if(!RestaurantManager.Instance.GetTable(rand).isGossiped && RestaurantManager.Instance.GetTable(rand).inUse) {
				transform.SetParent(RestaurantManager.Instance.GetTable(rand).Node.transform);
				SetBaseSortingOrder(RestaurantManager.Instance.GetTable(rand).Node.GetComponent<Node>().BaseSortingOrder);
				transform.localPosition = Vector3.zero;
				RestaurantManager.Instance.GetTable(rand).isGossiped = true;
				tableNum = rand;
				CustomerAnimationControllerGossiper ani = customerAnim as CustomerAnimationControllerGossiper;
				ani.Gossip();
				if(mod != GossiperMode.None) {
					StartCoroutine("Annoy");
				}
			}
			else {
				StartCoroutine(WaitAFew());
			}
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
				RestaurantManager.Instance.GetTable(tableNum).seat.GetChild(0).GetComponent<CustomerEater>().pastBehav = RestaurantManager.Instance.GetTable(tableNum).seat.GetChild(0).GetComponent<Customer>().currBehav;
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
	
	/// <summary>
	/// Base sorting order from the node placed itself
	/// If customer mesh order is B, customer UI canvas is set to B+5, table will be set in the middle B+1 and B+2
	/// </summary>
	public void SetBaseSortingOrder(int baseSortingOrder) {
		customerMeshRenderer.sortingOrder = baseSortingOrder;
		customerUI.UpdateSortingOrder(baseSortingOrder + 5);
	}
}
