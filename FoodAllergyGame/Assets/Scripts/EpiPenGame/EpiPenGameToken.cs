﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EpiPenGameToken : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public static GameObject itemBeingDragged;
	public int tokenNumber;
	public Animator animator;
	public Animation checkMarkAnim;
	public Animation xMarkAnim;

	private Transform startParent;

	public bool isLocked;
	public bool IsLocked {
		get { return isLocked; }
		set { isLocked = value; }
	}

	public void Init(int _tokenNumber, bool _isLocked) {
		isLocked = _isLocked;
		tokenNumber = _tokenNumber;
        gameObject.name = "Token" + _tokenNumber;
		SetAnimateState(false);
		HideMark();
    }

	// Init function for the animation aux token
	public void AuxInit() {
		HideMark();
	}

	public void SetAnimateState(bool isAnimate) {
		animator.Play(isAnimate ? "AnimateState" : "TokenState");
	}

	// Show the punch mark for either a check or an X
	public void SetMark(bool isCorrect, bool playAnimation) {
		if(isCorrect) {
			checkMarkAnim.gameObject.SetActive(true);
			xMarkAnim.gameObject.SetActive(false);
			if(playAnimation) {
				checkMarkAnim.Play();
			}
			AudioManager.Instance.PlayClip("EpipenRight");
		}
		else {
			checkMarkAnim.gameObject.SetActive(false);
			xMarkAnim.gameObject.SetActive(true);
			if(playAnimation) {
				xMarkAnim.Play();
			}
			AudioManager.Instance.PlayClip("EpipenWrong");
		}
	}
	
	public void HideMark() {
		checkMarkAnim.gameObject.SetActive(false);
		xMarkAnim.gameObject.SetActive(false);
	}

	public void OnBeginDrag(PointerEventData eventData) {
		if(!isLocked && !EpiPenGameManager.Instance.isTutorial) {
			itemBeingDragged = gameObject;
			startParent = transform.parent;
			GetComponent<CanvasGroup>().blocksRaycasts = false;

			transform.SetParent(EpiPenGameManager.Instance.activeDragParent);
			AudioManager.Instance.PlayClip("PickUp");
		}
	}

	public void OnDrag(PointerEventData data) {
		if(!isLocked && !EpiPenGameManager.Instance.isTutorial) {
			// Screen space overlay dragging - http://forum.unity3d.com/threads/mouse-position-for-screen-space-camera.294458/
#if UNITY_EDITOR
			Vector3 pointerPosition = Input.mousePosition;
#else
			Vector3 pointerPosition = Input.GetTouch(0).position;
#endif
			pointerPosition.z = EpiPenGameManager.Instance.GetCanvasDistanceOffset();
			itemBeingDragged.transform.position = Camera.main.ScreenToWorldPoint(pointerPosition);
		}
	}

	/// <summary>
	/// Make sure this is called AFTER OnDrop for the slots
	/// </summary>
	/// <param name="eventData"></param>
	public void OnEndDrag(PointerEventData eventData) {
		if(!isLocked && !EpiPenGameManager.Instance.isTutorial) {
			itemBeingDragged = null;

			// Reset to its start position, invalid dragging target
			if(transform.parent == EpiPenGameManager.Instance.activeDragParent) {
				transform.SetParent(startParent);
				transform.localPosition = Vector3.zero;
            }
			AudioManager.Instance.PlayClip("Drop");
			GetComponent<CanvasGroup>().blocksRaycasts = true;

			EpiPenGameManager.Instance.TokenPlaced();
		}
	}

	public Transform GetStartPosition() {
		return startParent;
	}
}
