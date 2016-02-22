﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FloatyAndParticleLogic : MonoBehaviour {
	void Start() {
		RectTransform UIElement = GetComponent<RectTransform>();
		RectTransform canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
		Vector2 viewportPosition;

		if(SceneManager.GetActiveScene().name == SceneUtils.DECO){
			viewportPosition = Camera.main.ScreenToViewportPoint(transform.position);
		}
		else{
			viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
		}

		Vector2 worldObjectScreenPos = new Vector2(
			((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
			((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

		Debug.Log(transform.localScale);
		UIElement.anchoredPosition = worldObjectScreenPos;
		LeanTween.value(gameObject, ReduceVisibility, 1.0f,  0.0f, 1.0f);
		LeanTween.value(gameObject, Move, transform.position, new Vector3(transform.position.x, transform.position.y + 50, transform.position.z), 1.0f);  
		StartCoroutine(DestroyEffect());

	}

	private void ReduceVisibility(float amount){
		gameObject.GetComponent<CanvasGroup>().alpha =  amount; 
	}

	private void Move(Vector3 pos){
		transform.position = pos;
	}

	IEnumerator DestroyEffect(){
		yield return new WaitForSeconds(3.0f);
		Destroy(gameObject);
	}
}
