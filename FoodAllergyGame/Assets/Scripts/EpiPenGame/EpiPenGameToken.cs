using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EpiPenGameToken : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public static GameObject itemBeingDragged;
	public int order;
	public Animator animator;

	private Transform startParent;

	private bool isLocked;
	public bool IsLocked {
		get { return isLocked; }
		set { isLocked = value; }
	}

	public void Init(int tokenOrder, bool isLockedIncoming) {
		isLocked = isLockedIncoming;
        gameObject.name = "Token" + tokenOrder;
		order = tokenOrder;
		SetAnimateState(false);
    }

	public void SetAnimateState(bool isAnimate) {
		animator.Play(isAnimate ? "AnimateState" : "TokenState");
	}

	public void OnBeginDrag(PointerEventData eventData) {
		if(!isLocked) {
			//if(transform.parent.GetComponent<EpiPenGameSlot>() != null && transform.parent.GetComponent<EpiPenGameSlot>().isFinalSlot) {
			//	EpiPenGameManager.Instance.submittedAnswers.Remove(transform.parent.GetComponent<EpiPenGameSlot>().slotNumber);
			//}
			itemBeingDragged = gameObject;
			startParent = transform.parent;
			GetComponent<CanvasGroup>().blocksRaycasts = false;

			transform.SetParent(EpiPenGameManager.Instance.activeDragParent);
			//AudioManager.Instance.PlayClip("Button1Down");
		}
	}

	public void OnDrag(PointerEventData data) {
		if(!isLocked) {
#if UNITY_EDITOR
			itemBeingDragged.transform.position = Input.mousePosition;
#else
			itemBeingDragged.transform.position = Input.GetTouch(0).position;
#endif
		}
	}

	/// <summary>
	/// Make sure this is called AFTER OnDrop for the slots
	/// </summary>
	/// <param name="eventData"></param>
	public void OnEndDrag(PointerEventData eventData) {
		if(!isLocked) {
			itemBeingDragged = null;

			// Reset to its start position, invalid dragging target
			if(transform.parent == EpiPenGameManager.Instance.activeDragParent) {
				transform.SetParent(startParent);
				transform.localPosition = Vector3.zero;
            }
			//AudioManager.Instance.PlayClip("Button1Up");
			GetComponent<CanvasGroup>().blocksRaycasts = true;

			EpiPenGameManager.Instance.TokenPlaced();
		}
	}
}
