using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Controller to show food tips
/// The food showing is forced to cycle randomly until all is shown (choose randomly form bucket and remove picked each time)
/// Once all the elements have shown, reset the bucket and start over again
/// If new foods are unlocked, reset your list
/// </summary>
public class FoodTipController : MonoBehaviour {

	private int unlockedFoodCount = -1;		// Init to force initialization
	private List<ImmutableDataFood> internalFoodList;

	void Start(){
		RefreshInternalList();
	}

	private void RefreshInternalList(){
		List<ImmutableDataFood> newFoodList = DataLoaderFood.GetDataListWithinTier(TierManager.Instance.Tier);
		if(newFoodList.Count != unlockedFoodCount){
			internalFoodList = newFoodList;
			unlockedFoodCount = internalFoodList.Count;

			// TODO Reset other stuff here
		}
	}

	public void ShowFoodTip(){

	}
}
