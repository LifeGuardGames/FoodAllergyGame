using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MenuDragStockSlot : MonoBehaviour, IDropHandler {
	public GameObject item{
		get{
			if(transform.childCount > 0){
				return transform.GetChild(0).gameObject;
			}
			return null;
		}
	}

	#region IDropHandler implementation
	public void OnDrop(PointerEventData eventData){
		if(!item){
			FoodStockButton foodButton = FoodStockButton.itemBeingDragged.GetComponent<FoodStockButton>();
			foodButton.InFoodStockSlot = true;
			foodButton.transform.SetParent(transform);
			foodButton.transform.localPosition = Vector3.zero;
		}
	}
	#endregion
}

