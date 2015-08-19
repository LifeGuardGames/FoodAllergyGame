using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DecoButton : MonoBehaviour {

	public string decoID;
	public bool isBought;

	void Start(){
		this.GetComponent<Button>().onClick.AddListener(this.ChangeSet);

	}

	public void Init(ImmutableDataDecoItem _deco){
		decoID = _deco.ID;
		this.GetComponentInChildren<Text>().text = _deco.ButtonTitle;
		if(_deco.Type == DecoTypes.Kitchen){
			this.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(_deco.SpriteName);
			Debug.Log (_deco.SpriteName + "00");
		}
		else{
			this.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(_deco.SpriteName);
		}
		Debug.Log (_deco.id);
		if(DataManager.Instance.GameData.Decoration.BoughtDeco.ContainsKey(decoID)){
			isBought = true;
		}
	}

	public void ChangeSet(){
		DecoManager.Instance.ChangeSet(decoID);
	}

}
