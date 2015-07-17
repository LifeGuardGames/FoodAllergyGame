using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class FoodStockButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{

	public string foodID;
	public Image image;
	public Text label;
	public Image allergy1;
	public Image allergy2;

	public static GameObject itemBeingDragged;
	private Transform dragAux;
	private Vector3 startPosition;
	private Transform startParent;
	
	public void Init(ImmutableDataFood foodData){
		foodID = foodData.ID;
		gameObject.name = foodData.ID;
		label.text = GetComponent<Localize>().GetText(foodData.FoodNameKey);
		image.sprite = SpriteCacheManager.Instance.GetFoodSpriteData(foodData.SpriteName);

		// Set allergy sprite indicators if present
		if(foodData.AllergyList.Count == 1){
			if(foodData.AllergyList[0] == Allergies.None){
				allergy1.enabled = false;
				allergy2.enabled = false;
			}
			else{
				allergy1.enabled = true;
				allergy1.sprite = SpriteCacheManager.Instance.GetAllergySpriteData(foodData.AllergyList[0]);
				allergy2.enabled = false;
			}
		}
		else if(foodData.AllergyList.Count == 2){
			allergy1.enabled = true;
			allergy1.sprite = SpriteCacheManager.Instance.GetAllergySpriteData(foodData.AllergyList[0]);
			allergy2.enabled = true;
			allergy2.sprite = SpriteCacheManager.Instance.GetAllergySpriteData(foodData.AllergyList[1]);
		}
	}

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
		MenuManager.Instance.RemoveFoodFromMenuList(foodID);
		MenuManager.Instance.ShowFoodInfo(foodID);
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
		MenuManager.Instance.HideFoodInfo();
	}
	#endregion
}
