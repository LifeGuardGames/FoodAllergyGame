using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AllergyChartUIEntry : MonoBehaviour {

	public Image imageAllergy;
	public GameObject grid;
	public Text textNone;
	public Text textAllergy;
	public GameObject equals;

	public void Init(Allergies alg){
		Debug.Log (alg.ToString ());
		if(alg == Allergies.None){
			textNone.gameObject.SetActive(true);
			textAllergy.gameObject.SetActive(false);
			equals.SetActive(false);
			imageAllergy.gameObject.SetActive(false);
		}
		else{
			textAllergy.gameObject.SetActive(true);
			textAllergy.text = alg.ToString ();
			textNone.gameObject.SetActive(false);
			imageAllergy.gameObject.SetActive(true);
			equals.SetActive(true);
			imageAllergy.sprite = SpriteCacheManager.Instance.GetAllergySpriteData(alg);
			foreach (ImmutableDataFood foodData in FoodManager.Instance.menuList){
				if(foodData.AllergyList.Contains(alg)){
					Debug.Log (foodData.ID);
					GameObject foodImg = new GameObject();
					foodImg.AddComponent<Image>();
					foodImg.GetComponent<Image>().sprite = SpriteCacheManager.Instance.GetFoodSpriteData(foodData.SpriteName);
					GameObject entry = GameObjectUtils.AddChildGUI(grid, foodImg);
					Destroy(foodImg);
				}
			}
		}
	}
}
