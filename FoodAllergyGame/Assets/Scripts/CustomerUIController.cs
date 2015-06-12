using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CustomerUIController : MonoBehaviour {

	public Image satisfaction1;
	public Image satisfaction2;
	public Image satisfaction3;

	public Image skull;

	public Image waiting;

	public void UpdateSatisfaction(int satisfaction){
		if(satisfaction <= 0){
			satisfaction1.gameObject.SetActive(false);
			satisfaction2.gameObject.SetActive(false);
			satisfaction3.gameObject.SetActive(false);
		}
		else if(satisfaction == 1){
			satisfaction1.gameObject.SetActive(true);
			satisfaction2.gameObject.SetActive(false);
			satisfaction3.gameObject.SetActive(false);
		}
		else if(satisfaction == 2){
			satisfaction1.gameObject.SetActive(true);
			satisfaction2.gameObject.SetActive(true);
			satisfaction3.gameObject.SetActive(false);
		}
		else if(satisfaction >= 3){
			satisfaction1.gameObject.SetActive(true);
			satisfaction2.gameObject.SetActive(true);
			satisfaction3.gameObject.SetActive(true);
		}
	}

	public void ToggleWait(bool isWaiting){
		waiting.gameObject.SetActive(isWaiting ? true : false);
	}

	public void ToggleAllergyAttack(bool isAllergyAttack){
		skull.gameObject.SetActive(isAllergyAttack ? true : false);
	}
}

