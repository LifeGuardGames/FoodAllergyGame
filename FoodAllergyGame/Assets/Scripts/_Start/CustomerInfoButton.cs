using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CustomerInfoButton : MonoBehaviour {
	public string customerID;
	public Image image;
	public Text label;
	public Button button;
	
	public void Init(ImmutableDataCustomer customerData){
		customerID = customerData.ID;
		gameObject.name = customerData.ID;
		label.text = GetComponent<Localize>().GetText(customerData.CustomerNameKey);
		image.sprite = SpriteCacheManager.Instance.GetCustomerSpriteData(customerData.SpriteName);
	}
	
	public void OnButtonClick(){
		if(string.Equals(Application.loadedLevelName, SceneUtils.START)){
			InfoManager.Instance.ShowDetail(InfoType.Customer, customerID);
		}
	}
}

