using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DecoTutorialController : MonoBehaviour {

	public Image sprite;
	public Image sprite2;
	public Localize loc;

	public void Init(DecoTypes type){
		sprite.sprite = Resources.Load<Sprite>(type.ToString() + "Tut");
		sprite2.sprite = Resources.Load<Sprite>(type.ToString() + "Tut2");
		loc.key = type.ToString() + "Tut";
	}

	public void OnOkayButtonClicked(){
		gameObject.SetActive(false);
	}
}
