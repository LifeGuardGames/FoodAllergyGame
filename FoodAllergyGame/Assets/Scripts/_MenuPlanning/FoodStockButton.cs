using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class FoodStockButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{
	public static GameObject itemBeingDragged;
//	public static bool canDrag = true;			// Prevent item from being dragged before all animation is done

	public string foodID;
	public Image image;
	public Text label;
	public Image allergy1;
	public Image allergy2;
	public Text textCost;

	public GameObject allergyNode;
	public Image allergyNodeImage;
	public Image allergyNodeLine;
	private Vector2 allergyNodeStartPosition;

	private ImmutableDataFood foodData;
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
		this.foodData = foodData;
		foodID = foodData.ID;
		gameObject.name = foodData.ID;
		label.text = LocalizationText.GetText(foodData.FoodNameKey);
		image.sprite = SpriteCacheManager.Instance.GetFoodSpriteData(foodData.SpriteName);
		cost = foodData.Cost;
		textCost.text = "$" + cost.ToString();

		// Set allergy sprite indicators if present
		if(foodData.AllergyList.Count == 1){
			allergy1.enabled = true;
			allergy1.sprite = SpriteCacheManager.Instance.GetAllergySpriteData(foodData.AllergyList[0]);
			allergy2.enabled = false;
			allergyNodeImage.sprite = allergy1.sprite = SpriteCacheManager.Instance.GetAllergySpriteData(foodData.AllergyList[0]);
		}
		else if(foodData.AllergyList.Count == 2){
			allergy1.enabled = true;
			allergy1.sprite = SpriteCacheManager.Instance.GetAllergySpriteData(foodData.AllergyList[0]);
			allergy2.enabled = true;
			allergy2.sprite = SpriteCacheManager.Instance.GetAllergySpriteData(foodData.AllergyList[1]);
		}

		allergyNodeStartPosition = (allergyNode.transform as RectTransform).anchoredPosition;
		allergyNode.SetActive(false);
	}

	void Start(){
		dragAux = MenuManager.Instance.dragAux;
		trashAux = MenuManager.Instance.trashSlot.transform;
	}
	
	#region IBeginDragHandler implementation
	public void OnBeginDrag(PointerEventData eventData){
//		if(canDrag){
//			canDrag = false;

			itemBeingDragged = gameObject;
			startPosition = transform.localPosition;
			startParent = transform.parent;
			GetComponent<CanvasGroup>().blocksRaycasts = false;

			// Show allergy nodes
			allergyNode.SetActive(true);

			// Remove the food from its attachment
			if(MenuManager.Instance.RemoveFoodFromMenuList(foodID)){
				MenuManager.Instance.ChangeMenuCost(Cost * -1);
			}

			// Show trash can if dragging from selected slot
			if(!inFoodStockSlot){
				MenuManager.Instance.ShowTrashCan();
			}
//		}
	}
	#endregion
	
	#region IDragHandler implementation
	public void OnDrag(PointerEventData eventData){
		// This is a frame check, clever workaround
//		if(itemBeingDragged != null){
			itemBeingDragged.transform.SetParent(dragAux);

			#if UNITY_EDITOR
			itemBeingDragged.transform.position = Input.mousePosition;
			#else
			itemBeingDragged.transform.position = Input.GetTouch(0).position;
			#endif
//		}
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

			allergyNode.SetActive(false);
		}

		// Try to hide trash can no matter what
		MenuManager.Instance.HideTrashCan();
	}
	#endregion

	private void NodeDoneTweening(){
		// Dont actually delete the node when it flies to the bar, just hide it and reset position/parent
		allergyNode.SetActive(false);
		allergyNode.transform.SetParent(transform);
		(allergyNode.transform as RectTransform).anchoredPosition = allergyNodeStartPosition;

		// Update the chart accordingly after flying is done
//		MenuManager.Instance.allergiesChartController.UpdateChart();

		// Unlock drag here
//		canDrag = true;
	}

	private IEnumerator DestroySelf(){
		yield return new WaitForEndOfFrame();
		Destroy(gameObject);
	}
}
