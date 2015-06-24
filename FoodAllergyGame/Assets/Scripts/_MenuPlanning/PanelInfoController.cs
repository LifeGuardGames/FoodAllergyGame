using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelInfoController : MonoBehaviour {

	public Image image;
	public Text foodName;
	public Text allergiesLabel;
	public Text keywordsLabel;

	void Start(){
		ToggleVisibility(false);
		this.gameObject.AddComponent<Localize>();
	}

	private void ToggleVisibility(bool isVisible){
		foreach(Transform child in gameObject.transform){
			child.gameObject.SetActive(isVisible);	// Toggle the whole gameobject for now
		}
	}

	public void ShowInfo(InfoType infoType, string ID){
		if(infoType == InfoType.Food){
			ImmutableDataFood foodData = DataLoaderFood.GetData(ID);
			image.sprite = SpriteCacheManager.Instance.GetSpriteData(foodData.SpriteName);
			foodName.text = GetComponent<Localize>().setText(foodData.FoodNameKey);

			// Concat the allergies list
			allergiesLabel.text = "";
			for(int i = 0; i < foodData.AllergyList.Count; i++){
				allergiesLabel.text += foodData.AllergyList[i];
				if(i < foodData.AllergyList.Count - 1){	// Put in a newline if not last element
					allergiesLabel.text += "\n";
				}
			}

			// Concat the keywords list
			keywordsLabel.text = "";
			for(int i = 0; i < foodData.KeywordList.Count; i++){
				keywordsLabel.text += foodData.KeywordList[i];
				if(i < foodData.KeywordList.Count - 1){	// Put in a newline if not last element
					keywordsLabel.text += "\n";
				}
			}
		}
		else if(infoType == InfoType.Customer){
			// TODO implement customer showing here
		}

		ToggleVisibility(true);
	}
}
