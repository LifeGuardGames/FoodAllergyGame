using UnityEngine;
using System.Collections;

public class AllergyChartUIController : MonoBehaviour {

	public TweenToggle tween;
	public GameObject grid;
	public GameObject chartEntryPrefab;

	void Start(){
//		foreach(ImmutableDataFood foodData in FoodManager.Instance.MenuList){
//			GameObject entry = GameObjectUtils.AddChildGUI(grid, chartEntryPrefab);
//			entry.GetComponent<AllergyChartUIEntry>().Init(foodData.ID);
//		}
	}

	public void OnOpenButton(){
		tween.Show();
	}

	public void OnCloseButton(){
		tween.Hide();
	}
}
