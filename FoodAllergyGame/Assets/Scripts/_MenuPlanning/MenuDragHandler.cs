using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MenuDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public static GameObject itemBeingDragged;
	private Transform dragAux;
	private Vector3 startPosition;
	private Transform startParent;

	void Start(){
		dragAux = MenuManager.Instance.dragAux;
	}

	#region IBeginDragHandler implementation
	public void OnBeginDrag(PointerEventData eventData){
		itemBeingDragged = gameObject;
		startPosition = transform.localPosition;
		startParent = transform.parent;
		GetComponent<CanvasGroup>().blocksRaycasts = false;

		// Remove the food from its attachment
		FoodStockButton foodButton = itemBeingDragged.GetComponent<FoodStockButton>();
		MenuManager.Instance.RemoveFoodFromMenuList(foodButton.foodID);
	}
	#endregion

	#region IDragHandler implementation
	public void OnDrag(PointerEventData eventData){
		transform.SetParent(dragAux);
		transform.position = Input.mousePosition;
	}
	#endregion

	#region IEndDragHandler implementation
	// NOTE Make sure this is called after OnDrop() from the slot class
	public void OnEndDrag(PointerEventData eventData){
		itemBeingDragged = null;
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		if(transform.parent == dragAux){
			transform.SetParent(startParent);
			transform.localPosition = startPosition;
		}
		else{	// Save new parent
			startParent = transform.parent;
			startPosition = transform.localPosition;
		}
	}
	#endregion
}
