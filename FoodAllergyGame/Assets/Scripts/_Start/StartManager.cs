using UnityEngine;
using System.Collections;

public class StartManager : Singleton<StartManager>{

	void Start(){
		//TODO Generate event from data


		//TODO Set up visuals and appearances for that day


	}

	public void OnPlayButtonClicked(){
		TransitionManager.Instance.TransitionScene(SceneUtils.MENUPLANNING);
	}

	public void OnDecoButtonClicked(){

	}

	public void OnInfoByttonClicked(){

	}
}
