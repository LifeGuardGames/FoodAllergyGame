using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DecoTutorialController : MonoBehaviour {

	public Image sprite;
	public Image spriteMid;
	public Image spriteEnd;
	public Localize loc;

	public void Init(DecoTypes type){
		ImmutableDataDecoTut tutData = DataLoaderDecoTut.GetData(type.ToString());
		sprite.sprite = Resources.Load<Sprite>(tutData.Image);
		Debug.Log (tutData.ImageMid);
		spriteMid.sprite = Resources.Load<Sprite>(tutData.ImageMid);
		spriteEnd.sprite = Resources.Load<Sprite>(tutData.ImageEnd);
		loc.key = tutData.Text;
	}

	public void OnOkayButtonClicked(){
		gameObject.SetActive(false);
	}
}
