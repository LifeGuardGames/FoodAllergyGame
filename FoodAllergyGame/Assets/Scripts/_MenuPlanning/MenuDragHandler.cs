using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MenuDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public static GameObject itemBeingDragged;
	private Transform dragAux;
	private Vector3 startPosition;
	private Transform startParent;

	void Start(){
		dragAux = MenuManager.Instance.dragAux;
	}

	#region IBeginDragHandler implementation
	public void OnBeginDrag(PointerEventData eventData){
		itemBeingDragged = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}
	#endregion

	#region IDragHandler implementation
	public void OnDrag(PointerEventData eventData){
		transform.SetParent(dragAux);
		transform.position = Input.mousePosition;
	}
	#endregion

	#region IEndDragHandler implementation
	public void OnEndDrag(PointerEventData eventData){
		itemBeingDragged = null;
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		if(transform.parent == startParent){
			transform.position = startPosition;
		}
	}
	#endregion
}
