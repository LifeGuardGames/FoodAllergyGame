using UnityEngine;
using System.Collections;

public class AllergyChartButton : MonoBehaviour, IClickObject {

	#region IClickObject implementation
	public void OnClicked(){
		Debug.Log("SFSDF");
		RestaurantManager.Instance.GetAllergyUIController().OnOpenButton();
	}
	#endregion
}
