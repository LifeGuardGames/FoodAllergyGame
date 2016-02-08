using UnityEngine;
using UnityEngine.EventSystems;

public class EpiPenGamePanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public static GameObject itemBeingDragged;
	public int order;

	private Vector3 startPosition;
	private Transform startParent;

	public bool isCorrect = true;

	public void Locked() {
		isCorrect = true;
	}

	public void OnBeginDrag(PointerEventData eventData) {
		if(!isCorrect) { 
			if(transform.parent.GetComponent<EpiPenGameComicSlot>() != null) {
				EpiPenGameManager.Instance.submittedAnswers.Remove(transform.parent.GetComponent<EpiPenGameComicSlot>().slotNumber);
			}
			itemBeingDragged = gameObject;
			startPosition = transform.localPosition;
			startParent = transform.parent;
			GetComponent<CanvasGroup>().blocksRaycasts = false;

			//AudioManager.Instance.PlayClip("Button1Down");
		}
	}

	public void OnDrag(PointerEventData data) {
		if(!isCorrect) {
#if UNITY_EDITOR
			itemBeingDragged.transform.position = Input.mousePosition;
#else
			itemBeingDragged.transform.position = Input.GetTouch(0).position;
#endif
		}
	}

	public void OnEndDrag(PointerEventData eventData) {
		if(!isCorrect) {
			itemBeingDragged = null;
			//AudioManager.Instance.PlayClip("Button1Up");
			GetComponent<CanvasGroup>().blocksRaycasts = true;
			//	transform.SetParent(startParent);
			//	transform.localPosition = startPosition;
			if(transform.parent == startParent) {
				EpiPenGameManager.Instance.PlaceInPos(this);
			}
		}
	}
}
