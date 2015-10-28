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
	public Image allergy3;
	
	public Image allergyNodeImage1;
	public Image allergyNodeImage2;
	public Image allergyNodeImage3;

	public RectTransform allergyNode1;
	public RectTransform allergyNode2;
	public RectTransform allergyNode3;
	public RectTransform slotNode;

	public Animator foodButtonAnimator;

	private Transform dragAux;
	private Transform trashAux;
	private Vector3 startPosition;
	private Transform startParent;

	private bool inFoodStockSlot = true;
	public bool InFoodStockSlot{
		get{ return inFoodStockSlot; }
		set{ inFoodStockSlot = value; }
	}

	void Awake() {
		allergyNode1.localScale = new Vector3(0, 0, 1);
		allergyNode2.localScale = new Vector3(0, 0, 1);
		allergyNode3.localScale = new Vector3(0, 0, 1);
		slotNode.localScale = new Vector3(0, 0, 1);
    }
	
	public void Init(ImmutableDataFood foodData){
		foodID = foodData.ID;
		gameObject.name = foodData.ID;
		label.text = LocalizationText.GetText(foodData.FoodNameKey);
		image.sprite = SpriteCacheManager.GetFoodSpriteData(foodData.SpriteName);

		// Set allergy sprite indicators
		// NOTE: None counts as an allergy
		allergy1.enabled = false;
		allergy2.enabled = false;
		allergy3.enabled = false;
		if(foodData.AllergyList.Count > 0){		// Cascading addition
			Sprite allergySprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[0]);
			allergy1.enabled = true;
			allergy1.sprite = allergySprite;
			allergyNodeImage1.sprite = allergySprite;
		}
		if(foodData.AllergyList.Count > 1){
			Sprite allergySprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[1]);
			allergy2.enabled = true;
			allergy2.sprite = allergySprite;
			allergyNodeImage2.sprite = allergySprite;
		}
		if(foodData.AllergyList.Count > 2){
			Sprite allergySprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[2]);
			allergy3.enabled = true;
			allergy1.sprite = allergySprite;
			allergyNodeImage3.sprite = allergySprite;
		}
	}

	void Start(){
		dragAux = MenuManager.Instance.dragAux;
		trashAux = MenuManager.Instance.trashSlot.transform;
	}
	
	#region IBeginDragHandler implementation
	public void OnBeginDrag(PointerEventData eventData){
		//		if(canDrag){
		//			canDrag = false;
		foodButtonAnimator.SetTrigger("PickedUp");
			itemBeingDragged = gameObject;
			startPosition = transform.localPosition;
			startParent = transform.parent;
			GetComponent<CanvasGroup>().blocksRaycasts = false;

		// Remove the food from its attachment
		MenuManager.Instance.RemoveFoodFromMenuList(foodID);

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
		if (transform.parent == dragAux) {
			transform.SetParent(startParent);
			transform.localPosition = startPosition;
			if(startParent.gameObject.GetComponent<MenuDragSlot>().isSelectedSlot){
				MenuManager.Instance.AddFoodToMenuList(foodID);
				foodButtonAnimator.SetTrigger("PutDownSelected");
			}
			else {
				foodButtonAnimator.SetTrigger("PutDownUnselected");
			}
		}
		else if(transform.parent == trashAux){
			StartCoroutine(DestroySelf());
		}
		else{	// Save new parent
			startParent = transform.parent;
			startPosition = transform.localPosition;

			if(startParent.gameObject.GetComponent<MenuDragSlot>().isSelectedSlot) {
				foodButtonAnimator.SetTrigger("PutDownSelected");
			}
			else {
				foodButtonAnimator.SetTrigger("PutDownUnselected");
			}
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
