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
				foodButton.InFoodStockSlot = false;
				foodButton.transform.SetParent(transform);
				foodButton.transform.localPosition = Vector3.zero;
				MenuManager.Instance.ChangeMenuCost(foodButton.Cost);
			}
			else if(!isSelectedSlot){
				foodButton.InFoodStockSlot = true;
				foodButton.transform.SetParent(transform);
				foodButton.transform.localPosition = Vector3.zero;
			}
		}
	}
	#endregion
}

