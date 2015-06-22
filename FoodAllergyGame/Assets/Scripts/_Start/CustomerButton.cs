using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CustomerButton : MonoBehaviour {

	public string customerID;
	public Image image;
	public Text label;
	public Button button;
	
	public void Init(ImmutableDataCustomer customerData){
		customerID = customerData.ID;
		gameObject.name = customerData.ID;
		label.text = customerData.CustomerNameKey;
//		image.sprite = SpriteCacheManager.Instance.GetSpriteData(customerData.SpriteName);
	}
	
	public void OnButtonClick(){
		if(string.Equals(Application.loadedLevelName, SceneUtils.START)){
			InfoManager.Instance.ShowDetail(InfoManager.InfoType.Customer, customerID);
		}
	}
}
