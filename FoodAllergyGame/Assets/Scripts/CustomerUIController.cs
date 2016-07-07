using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CustomerUIController : MonoBehaviour {
	public Canvas uiCanvas;
	public Image satisfaction1;
	public Image satisfaction2;
	public Image satisfaction3;
	public Image satisfaction4;
	
	public Image waiting;
	public Image star;

	public ParticleSystem angryParticle;

	/// <summary>
	/// Handled from customer base class only
	/// </summary>
	public void UpdateSortingOrder(int canvasSortingOrder) {
		uiCanvas.sortingOrder = canvasSortingOrder;
	}

	public void LosingHeart(int satisfaction) {
		if(satisfaction == 1) {
			satisfaction1.gameObject.GetComponent<Animation>().Play("HeartFlash");
		}
		else if(satisfaction == 2) {
			satisfaction2.gameObject.GetComponent<Animation>().Play("HeartFlash");
		}
		else if(satisfaction == 3) {
			satisfaction3.gameObject.GetComponent<Animation>().Play("HeartFlash");
		}
	}

	public void StopLosingHeart(int satisfaction) {
		if(satisfaction == 1) {
			satisfaction1.gameObject.GetComponent<Animation>().Stop("HeartFlash");
			satisfaction2.gameObject.GetComponent<Animation>().Stop("HeartFlash");
			satisfaction3.gameObject.GetComponent<Animation>().Stop("HeartFlash");
			satisfaction1.gameObject.GetComponent<Image>().enabled = true;
		}
		else if(satisfaction == 2) {
			satisfaction1.gameObject.GetComponent<Animation>().Stop("HeartFlash");
			satisfaction2.gameObject.GetComponent<Animation>().Stop("HeartFlash");
			satisfaction3.gameObject.GetComponent<Animation>().Stop("HeartFlash");
			satisfaction2.gameObject.GetComponent<Image>().enabled = true;
		}
		else if(satisfaction == 3) {
			satisfaction1.gameObject.GetComponent<Animation>().Stop("HeartFlash");
			satisfaction2.gameObject.GetComponent<Animation>().Stop("HeartFlash");
			satisfaction3.gameObject.GetComponent<Animation>().Stop("HeartFlash");
			satisfaction3.gameObject.GetComponent<Image>().enabled = true;
		}
	}

	/// <summary>
	/// Update absolute satisfaction
	/// </summary>
	/// <param name="satisfaction"></param>
	public void UpdateSatisfaction(int satisfaction, bool checkDelta = false, int delta = 0){
		if(satisfaction <= 0){
			satisfaction1.gameObject.SetActive(false);
			satisfaction2.gameObject.SetActive(false);
			satisfaction3.gameObject.SetActive(false);
			if(satisfaction4 != null) {
				satisfaction4.gameObject.SetActive(false);
			}
		}
		else if(satisfaction == 1){
			satisfaction1.gameObject.SetActive(true);
			satisfaction2.gameObject.SetActive(false);
			satisfaction3.gameObject.SetActive(false);
			if(satisfaction4 != null) {
				satisfaction4.gameObject.SetActive(false);
			}

			if(checkDelta) {
				if(delta < 0) {
					angryParticle.Play();
				}
			}
		}
		else if(satisfaction == 2){
			satisfaction1.gameObject.SetActive(true);
			satisfaction2.gameObject.SetActive(true);
			satisfaction3.gameObject.SetActive(false);
			if(satisfaction4 != null) {
				satisfaction4.gameObject.SetActive(false);
			}

			if(checkDelta) {
				if(delta < 0) {
					angryParticle.Play();
				}
			}
		}
		else if(satisfaction == 3){
			satisfaction1.gameObject.SetActive(true);
			satisfaction2.gameObject.SetActive(true);
			satisfaction3.gameObject.SetActive(true);
			if(satisfaction4 != null) {
				satisfaction4.gameObject.SetActive(false);
			}

			if(checkDelta) {
				if(delta < 0) {
					angryParticle.Play();
				}
			}
		}
		else if(satisfaction == 4) {
			satisfaction1.gameObject.SetActive(true);
			satisfaction2.gameObject.SetActive(true);
			satisfaction3.gameObject.SetActive(true);
			if(satisfaction4 != null) {
				satisfaction4.gameObject.SetActive(true);
			}
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
		// Do nothing here
	}
}

