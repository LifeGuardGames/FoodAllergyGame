using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CustomerUIController : MonoBehaviour {

	public Image satisfaction1;
	public Image satisfaction2;
	public Image satisfaction3;

	public Image skull;
	public Image waiting;
	public Image star;

//	public GameObject thoughtObject;
//	public Image allergyImage;
//	public Text allergyText;

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

//	public void ToggleAllergyShow(bool isShowAllergy, Allergies allergy){
//		allergyText.gameObject.SetActive(isShowAllergy);
//		thoughtObject.gameObject.SetActive(isShowAllergy);
//
//		if(allergy == Allergies.None){
//			allergyText.text = "No allergies";
//			thoughtObject.gameObject.SetActive(false);	// Turn it off, dont need it
//		}
//		else{
//			allergyText.text = "Has " + allergy.ToString() + " allergies";
//			allergyImage.sprite = SpriteCacheManager.GetAllergySpriteData(allergy);
//		}
//	}

	public void ToggleWait(bool isWaiting){
		waiting.gameObject.SetActive(isWaiting ? true : false);
	}

	public void ToggleStar(bool isDoneWithMeal){
		star.gameObject.SetActive(isDoneWithMeal ? true : false);
	}

	public void ToggleAllergyAttack(bool isAllergyAttack){
		skull.gameObject.SetActive(isAllergyAttack ? true : false);
	}
}

