using UnityEngine;
using System.Collections;

public class AllergyChartButton : MonoBehaviour, IClickObject {

	#region IClickObject implementation
	public void OnClicked(){
		RestaurantManager.Instance.GetAllergyUIController().OnOpenButton();
	}
	#endregion
}
