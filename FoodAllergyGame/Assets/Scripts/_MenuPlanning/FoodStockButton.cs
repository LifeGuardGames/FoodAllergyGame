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

	public Image panelImage;
	public Sprite silverPanel;
	public Sprite goldPanel;

	public Sprite slot1Sprite;
	public Sprite slot2Sprite;
	public Sprite slot3Sprite;

	public Image allergy1;
	public Image allergy2;
	public Image allergy3;

	public Image allergyNode1;
	public Image allergyNode2;
	public Image allergyNode3;
	public Image slotNode;

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
		slotNode.transform.localScale = new Vector3(0, 0, 1);
	}
	
	public void Init(ImmutableDataFood foodData){
		foodID = foodData.ID;
		gameObject.name = foodData.ID;
		label.text = LocalizationText.GetText(foodData.FoodNameKey);
		image.sprite = SpriteCacheManager.GetFoodSpriteData(foodData.SpriteName);

		// Different panel tiers for different reward levels
		switch(foodData.Reward) {
			case 3:
				panelImage.sprite = silverPanel;
                break;
			case 4:
				panelImage.sprite = goldPanel;
				break;
			default:
				// Dont do anything
				break;
		}

		slotNode.sprite = SpriteCacheManager.GetSlotSpriteData(foodData.Slots);

		// Set allergy sprite indicators
		// NOTE: None counts as an allergy
		if(foodData.AllergyList.Count == 1){
			allergy1.enabled = true;
			allergy1.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[0]);
			allergy2.enabled = false;
			allergy3.enabled = false;

			allergyNode1.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[0]);
			Destroy(allergyNode2.GetComponent<Image>());        // Destroy everything but not object itself
			foreach(Transform child in allergyNode2.transform) {
				Destroy(child.gameObject);
			}
			Destroy(allergyNode3.GetComponent<Image>());        // Destroy everything but not object itself
			foreach(Transform child in allergyNode3.transform) {
				Destroy(child.gameObject);
			}
		}
		if(foodData.AllergyList.Count == 2){
			allergy1.enabled = true;
			allergy1.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[0]);
			allergy2.enabled = true;
			allergy2.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[1]);
			allergy3.enabled = false;

			allergyNode1.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[0]);
			allergyNode2.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[1]);
			Destroy(allergyNode3.GetComponent<Image>());        // Destroy everything but not object itself
			foreach(Transform child in allergyNode3.transform) {
				Destroy(child.gameObject);
			}
		}
		if(foodData.AllergyList.Count == 3){
			allergy1.enabled = true;
			allergy1.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[0]);
			allergy2.enabled = true;
			allergy2.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[1]);
			allergy3.enabled = true;
			allergy3.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[2]);

			allergyNode1.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[0]);
			allergyNode2.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[1]);
			allergyNode3.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[2]);
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
		AudioManager.Instance.PlayClip("Button1Down");

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
		AudioManager.Instance.PlayClip("Button1Up");
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
