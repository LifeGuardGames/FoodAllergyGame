using UnityEngine;
using System.Collections;

/// <summary>
/// Controller to show food tips
/// The food showing is forced to cycle randomly until all is shown (choose randomly form bucket and remove picked each time)
/// Once all the elements have shown, reset the bucket and start over again
/// If new foods are unlocked, reset your list
/// </summary>
public class FoodTipController : MonoBehaviour {

	private int unlockedFoodCount = -1;		// Init to force initialization

	void Start(){


		//bool isNewFoodUnlocked = unlockedFoodCount == FoodManager.Instance.FoodStockList
	}

	public void ShowFoodTip(){

	}
}
