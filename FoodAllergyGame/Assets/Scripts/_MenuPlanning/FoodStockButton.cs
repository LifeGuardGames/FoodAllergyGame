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

	public Sprite slot1Sprite;
	public Sprite slot2Sprite;
	public Sprite slot3Sprite;

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
	public Image slotImage;

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
		slotNode.localScale = new Vector3(0, 0, 1);
	}
	
	public void Init(ImmutableDataFood foodData){
		foodID = foodData.ID;
		gameObject.name = foodData.ID;
		label.text = LocalizationText.GetText(foodData.FoodNameKey);
		image.sprite = SpriteCacheManager.GetFoodSpriteData(foodData.SpriteName);

		switch(foodData.Slots) {
			case 1:
				slotImage.sprite = slot1Sprite;
                break;
			case 2:
				slotImage.sprite = slot2Sprite;
				break;
			case 3:
				slotImage.sprite = slot3Sprite;
				break;
			default:
				Debug.LogError("Bad slot number " + foodData.Slots);
				break;
		}

		// Set allergy sprite indicators
		// NOTE: None counts as an allergy
		if(foodData.AllergyList.Count == 1){
			allergy1.enabled = true;
			allergy2.enabled = false;
			allergy3.enabled = false;

			allergy1.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[0]);
			allergyNodeImage1.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[0]);
			Destroy(allergyNode2.GetComponent<Image>());        // Destroy everything but not object itself
			foreach(Transform child in allergyNode2) {
				Destroy(child.gameObject);
			}
			Destroy(allergyNode3.GetComponent<Image>());        // Destroy everything but not object itself
			foreach(Transform child in allergyNode3) {
				Destroy(child.gameObject);
			}
		}
		if(foodData.AllergyList.Count == 2){
			allergy1.enabled = true;
			allergy2.enabled = true;
			allergy3.enabled = false;

			allergy1.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[0]);
			allergyNodeImage1.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[0]);
			allergy2.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[1]);
			allergyNodeImage2.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[1]);
			Destroy(allergyNode3.GetComponent<Image>());        // Destroy everything but not object itself
			foreach(Transform child in allergyNode3) {
				Destroy(child.gameObject);
			}
		}
		if(foodData.AllergyList.Count == 3){
			allergy1.enabled = true;
			allergy2.enabled = true;
			allergy3.enabled = true;

			allergy1.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[0]);
			allergyNodeImage1.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[0]);
			allergy2.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[1]);
			allergyNodeImage2.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[1]);
			allergy3.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[2]);
			allergyNodeImage3.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[2]);
		}
	}

	void Start(){
		dragAux = MenuManager.Instance.dragAux;
		trashAux = MenuManager.Instance.trashSlot.transform;
	}
	
	#region IBeginDragHandler implementation
	public void OnBeginDrag(PointerEventData eventData){
		foodButtonAnimator.SetTrigger("PickedUp");
		itemBeingDragged = gameObject;
		startPosition = transform.localPosition;
		startParent = transform.parent;
		GetComponent<CanvasGroup>().blocksRaycasts = false;

		itemBeingDragged.transform.SetParent(dragAux);

		if(!inFoodStockSlot){
			// Show trash can if dragging from selected slot
			MenuManager.Instance.ShowTrashCan();
			MenuManager.Instance.RemoveFoodFromMenuList(foodID);
		}
	}
	#endregion
	
	#region IDragHandler implementation
	public void OnDrag(PointerEventData eventData){
		#if UNITY_EDITOR
		itemBeingDragged.transform.position = Input.mousePosition;
		#else
		itemBeingDragged.transform.position = Input.GetTouch(0).position;
		#endif
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
			if(startParent.name == "SelectedGrid") {
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
			if(startParent.name == "SelectedGrid") {
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
