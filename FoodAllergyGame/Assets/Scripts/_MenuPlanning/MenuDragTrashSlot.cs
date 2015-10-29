using UnityEngine;
using UnityEngine.EventSystems; 
using System.Collections;

public class MenuDragTrashSlot : MonoBehaviour, IDropHandler {

	#region IDropHandler implementation
	public void OnDrop(PointerEventData eventData){
		FoodStockButton foodButton = FoodStockButton.itemBeingDragged.GetComponent<FoodStockButton>();

		// Switch parent out of drag aux, so we know not to reset to old parent
		foodButton.transform.SetParent(transform);
		foodButton.transform.localPosition = Vector3.zero;

		// Object is already removed when dragging, so just refresh the page
		MenuManager.Instance.RefreshCurrentPage();
	}
	#endregion
}
