using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MenuDragSelectedSlot : MonoBehaviour, IDropHandler {

	#region IDropHandler implementation
	public void OnDrop(PointerEventData eventData){
		FoodStockButton foodButton = FoodStockButton.itemBeingDragged.GetComponent<FoodStockButton>();
		if(MenuManager.Instance.AddFoodToMenuList(foodButton.foodID)){
			foodButton.InFoodStockSlot = false;
			foodButton.transform.SetParent(MenuManager.Instance.SelectedGrid);
		}
	}
	#endregion
}

