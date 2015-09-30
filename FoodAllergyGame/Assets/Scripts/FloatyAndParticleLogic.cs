using UnityEngine;
using System.Collections;

public class FloatyAndParticleLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.SetParent(GameObject.Find ("Canvas").transform);
		RectTransform UI_Element = this.GetComponent<RectTransform>();
		RectTransform CanvasRect = GameObject.Find ("Canvas").GetComponent<RectTransform>();
		Vector2 ViewportPosition=Camera.main.ScreenToViewportPoint(this.transform.position);
		Vector2 WorldObject_ScreenPosition=new Vector2(
			((ViewportPosition.x*CanvasRect.sizeDelta.x)-(CanvasRect.sizeDelta.x*0.5f)),
			((ViewportPosition.y*CanvasRect.sizeDelta.y)-(CanvasRect.sizeDelta.y*0.5f)));
		UI_Element.anchoredPosition=WorldObject_ScreenPosition;
		LeanTween.value(this.gameObject, ReduceVisibility, 1.0f,  0.0f, 1.0f);
		LeanTween.value(this.gameObject, Move, this.transform.position, new Vector3(this.transform.position.x, this.transform.position.y + 50, this.transform.position.z), 1.0f);  
		StartCoroutine(DestroyEffect());
	}

	private void Move(Vector3 pos){
		this.transform.position = pos;
	}

	private void ReduceVisibility(float amount){
		this.gameObject.GetComponent<CanvasGroup>().alpha =  amount; 
	}

	IEnumerator DestroyEffect(){
		yield return new WaitForSeconds(3.0f);
		Destroy(this.gameObject);
	}
}
