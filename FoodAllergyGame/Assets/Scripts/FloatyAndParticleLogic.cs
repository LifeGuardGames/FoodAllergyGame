using UnityEngine;
using System.Collections;

public class FloatyAndParticleLogic : MonoBehaviour {
	void Start() {
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
