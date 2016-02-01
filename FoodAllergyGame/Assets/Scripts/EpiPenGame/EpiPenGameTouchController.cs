using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class EpiPenGameTouchController : MonoBehaviour, IBeginDragHandler,IDragHandler, IEndDragHandler{

	public static GameObject itemBeingDragged;
	public int order;

	private Transform dragAux;
	private Transform trashAux;
	private Vector3 startPosition;
	private Transform startParent;
	public Image panelImage;

	public void OnBeginDrag(PointerEventData eventData) {
		itemBeingDragged = gameObject;
		startPosition = transform.localPosition;
		startParent = transform.parent;
		GetComponent<CanvasGroup>().blocksRaycasts = false;

		itemBeingDragged.transform.SetParent(dragAux);
		AudioManager.Instance.PlayClip("Button1Down");

	}

	public void OnDrag(PointerEventData data) {
#if UNITY_EDITOR
		itemBeingDragged.transform.position = Input.mousePosition;
#else
		itemBeingDragged.transform.position = Input.GetTouch(0).position;
#endif
	}


	public void OnEndDrag(PointerEventData eventData) {
		itemBeingDragged = null;
		AudioManager.Instance.PlayClip("Button1Up");
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		if(transform.parent == dragAux) {
			transform.SetParent(startParent);
			transform.localPosition = startPosition;
			if(startParent.name == "SelectedGrid") {
				
			}
			else {
				
			}
		}
		else if(transform.parent == trashAux) {
			StartCoroutine(DestroySelf());
		}
		else {  // Save new parent
			startParent = transform.parent;
			startPosition = transform.localPosition;
			if(startParent.name == "SelectedGrid") {
				
			}
			else {
				
			}
		}

		// Try to hide trash can no matter what
		MenuManager.Instance.HideTrashCan();
	}
	private IEnumerator DestroySelf() {
		yield return new WaitForEndOfFrame();
		Destroy(gameObject);
	}
}
