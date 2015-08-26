using UnityEngine;
using System.Collections;

public class StartObject : MonoBehaviour {

	public string spriteName;
	public string aniKey;
	// Use this for initialization
	public void Init (ImmutableDataStartObjects data) {
		this.name = data.ID;
		spriteName = data.SpriteName;
		aniKey = data.AnimationKey;
		if(!DataManager.Instance.GameData.StartObject.shouldAnimate.ContainsKey(data.ID)){
			//TODO play animation
			DataManager.Instance.GameData.StartObject.shouldAnimate.Add(data.ID,true);
		}
	}	
}
