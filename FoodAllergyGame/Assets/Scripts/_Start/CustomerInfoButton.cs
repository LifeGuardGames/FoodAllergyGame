/*
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class CustomerInfoButton : MonoBehaviour {
	public string customerID;
	public Image image;
	public Text label;
	public Button button;
	
	public void Init(ImmutableDataCustomer customerData){
		customerID = customerData.ID;
		gameObject.name = customerData.ID;
		label.text = LocalizationText.GetText(customerData.CustomerNameKey);
		image.sprite = SpriteCacheManager.GetCustomerSpriteData(customerData.SpriteName);
	}
	
	public void OnButtonClick(){
		if(string.Equals(SceneManager.GetActiveScene().name, SceneUtils.START)){
			InfoManager.Instance.ShowDetail(InfoType.Customer, customerID);
		}
	}
}
*/
