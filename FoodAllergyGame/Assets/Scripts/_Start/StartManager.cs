using UnityEngine;
using System.Collections;

public class StartManager : Singleton<StartManager>{

	public TweenToggleDemux startDemux;
	public TweenToggleDemux infoCategoriesDemux;
	public TweenToggleDemux infoDetailDemux;

	void Start(){
		//TODO Generate event from data
		DataManager.Instance.SetEvent( "Event0" + UnityEngine.Random.Range(0,6).ToString());


		//TODO Set up visuals and appearances for that day


	}

	public void OnPlayButtonClicked(){
		TransitionManager.Instance.TransitionScene(SceneUtils.MENUPLANNING);
	}

	public void OnDecoButtonClicked(){

	}

	public void OnInfoButtonClicked(){

	}

	public void ShowStartDemux(){
		infoDetailDemux.Hide();
		infoCategoriesDemux.Hide();
		startDemux.Show();
	}
	
	public void ShowInfoCategoriesDemux(){
		infoDetailDemux.Hide();
		infoCategoriesDemux.Show();
		startDemux.Hide();
		InfoManager.Instance.ClearDetail();
	}
	
	public void ShowInfoDetailDemux(){
		infoDetailDemux.Show();
		infoCategoriesDemux.Hide();
		startDemux.Hide();
	}
}
