using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MenuDragSlot : MonoBehaviour, IDropHandler {

	public bool isSelectedSlot = true; // If false, this is a food stock slot

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
			if(isSelectedSlot && MenuManager.Instance.AddFoodToMenuList(foodButton.foodID)){
				FoodStockButton.itemBeingDragged.transform.SetParent(transform);
				FoodStockButton.itemBeingDragged.transform.localPosition = Vector3.zero;
				MenuManager.Instance.ChangeNetCash(foodButton.Cost * -1);
			}
			else if(!isSelectedSlot){
				FoodStockButton.itemBeingDragged.transform.SetParent(transform);
				FoodStockButton.itemBeingDragged.transform.localPosition = Vector3.zero;
			}
		}
	}
	#endregion
}

