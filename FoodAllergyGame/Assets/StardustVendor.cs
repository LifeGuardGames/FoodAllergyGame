using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StardustVendor : MonoBehaviour {


	public void BuyOneStardust() {
		PurchasingManager.Instance.BuyConsumable(1);
		this.GetComponent<PositionTweenToggle>().Hide();
	}

	public void BuyTwoStardust() {
		PurchasingManager.Instance.BuyConsumable(2);
		this.GetComponent<PositionTweenToggle>().Hide();
	}

	public void BuyThreeStardust() {
		PurchasingManager.Instance.BuyConsumable(3);
		this.GetComponent<PositionTweenToggle>().Hide();
	}
}
