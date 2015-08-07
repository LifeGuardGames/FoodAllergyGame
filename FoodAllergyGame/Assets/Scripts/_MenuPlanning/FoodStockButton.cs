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
	public Text textCost;

	public static GameObject itemBeingDragged;
	private Transform dragAux;
	private Transform trashAux;
	private Vector3 startPosition;
	private Transform startParent;

	private int cost;
	public int Cost{
		get{ return cost; }
	}

	private bool inFoodStockSlot = true;
	public bool InFoodStockSlot{
		get{ return inFoodStockSlot; }
		set{ inFoodStockSlot = value; }
	}
	
	public void Init(ImmutableDataFood foodData){
		foodID = foodData.ID;
		gameObject.name = foodData.ID;
		label.text = GetComponent<Localize>().GetText(foodData.FoodNameKey);
		image.sprite = SpriteCacheManager.Instance.GetFoodSpriteData(foodData.SpriteName);
		cost = foodData.Cost;
		textCost.text = "$" + cost.ToString();

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
		trashAux = MenuManager.Instance.trashSlot.transform;
	}
	
	#region IBeginDragHandler implementation
	public void OnBeginDrag(PointerEventData eventData){
		itemBeingDragged = gameObject;
		startPosition = transform.localPosition;
		startParent = transform.parent;
		GetComponent<CanvasGroup>().blocksRaycasts = false;
		
		// Remove the food from its attachment
		if(MenuManager.Instance.RemoveFoodFromMenuList(foodID)){
			MenuManager.Instance.ChangeMenuCost(Cost * -1);
		}

		// Show trash can if dragging from selected slot
		if(!inFoodStockSlot){
			MenuManager.Instance.ShowTrashCan();
		}
	}
	#endregion
	
	#region IDragHandler implementation
	public void OnDrag(PointerEventData eventData){
		Debug.Log(Input.touches.Length);
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
			if(startParent.gameObject.GetComponent<MenuDragSlot>().isSelectedSlot){
				MenuManager.Instance.AddFoodToMenuList(foodID);
				MenuManager.Instance.ChangeMenuCost(Cost);
			}
		}
		else if(transform.parent == trashAux){
			StartCoroutine(DestroySelf());
		}
		else{	// Save new parent
			startParent = transform.parent;
			startPosition = transform.localPosition;
		}

		// Try to hide trash can no matter what
		MenuManager.Instance.HideTrashCan();
	}
	#endregion

	private IEnumerator DestroySelf(){
		yield return new WaitForEndOfFrame();
		Destroy(gameObject);
	}
}
