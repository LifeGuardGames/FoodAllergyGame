using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour{
	public GameObject canvas;
	private GameObject currScene;
	private GameObject tutObject;
	// Use this for initialization
	void Start(){
		// here we want to spawn the first tutorial scene so that we don't have a blank scene
		currScene = Resources.Load("TuTObject0")as GameObject;
		tutObject = GameObjectUtils.AddChildGUI(canvas, currScene);
		tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(1));
	}

	//each time the user hits the button in the tutorial scene we will destroy the current scene load in a new scene 
	//and attach this function to the current button
	//NOTE attaching during runtime is necessary as the buttons are created during runtime
	public void ChangeScene(int scene){
		//Debug.Log (scene);
		switch(scene){
		case 1:
			Destroy(tutObject);
			currScene = Resources.Load("TuTObject" + (scene.ToString()))as GameObject;
			tutObject = GameObjectUtils.AddChildGUI(canvas, currScene);
			tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(2));
			AudioManager.Instance.PlayClip("CustomerEnter");
			break;
		case 12:
			Destroy(tutObject);
			currScene = Resources.Load("TuTObject" + (scene.ToString()))as GameObject;
			tutObject = GameObjectUtils.AddChildGUI(canvas, currScene);
			tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(13));
			AudioManager.Instance.PlayClip("CustomerEnter");
			break;
		case 13:
			Destroy(tutObject);
			currScene = Resources.Load("TuTObject" + (scene.ToString()))as GameObject;
			tutObject = GameObjectUtils.AddChildGUI(canvas, currScene);
			tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(5));
			AudioManager.Instance.PlayClip("CustomerEnter");
			break;
		case 2:
			Destroy(tutObject);
			currScene = Resources.Load("TuTObject" + (scene.ToString()))as GameObject;
			tutObject = GameObjectUtils.AddChildGUI(canvas, currScene);
			tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(3));
			AudioManager.Instance.PlayClip("CustomerSelected");
			break;
		case 3:
			Destroy(tutObject);
			currScene = Resources.Load("TuTObject" + (scene.ToString()))as GameObject;
			tutObject = GameObjectUtils.AddChildGUI(canvas, currScene);
			tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(4));
			AudioManager.Instance.PlayClip("CustomerSeated");
			break;
		case 4:
			Destroy(tutObject);
			currScene = Resources.Load("TuTObject" + (scene.ToString()))as GameObject;
			tutObject = GameObjectUtils.AddChildGUI(canvas, currScene);
			tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(12));
			// TODO-SOUND Order time sound here
			break;
		case 5:
			Destroy(tutObject);
			currScene = Resources.Load("TuTObject" + (scene.ToString()))as GameObject;
			tutObject = GameObjectUtils.AddChildGUI(canvas, currScene);
			tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(6));
			break;
		case 6:
			Destroy(tutObject);
			currScene = Resources.Load("TuTObject" + (scene.ToString()))as GameObject;
			tutObject = GameObjectUtils.AddChildGUI(canvas, currScene);
			tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(7));
			AudioManager.Instance.PlayClip("WriteDownOrder");
			break;
		case 7:
			Destroy(tutObject);
			currScene = Resources.Load("TuTObject" + (scene.ToString()))as GameObject;
			tutObject = GameObjectUtils.AddChildGUI(canvas, currScene);
			tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(8));
			AudioManager.Instance.PlayClip("GiveOrder");
			AudioManager.Instance.PlayClip("OrderReady");
			break;
		case 8:
			Destroy(tutObject);
			currScene = Resources.Load("TuTObject" + (scene.ToString()))as GameObject;
			tutObject = GameObjectUtils.AddChildGUI(canvas, currScene);
			tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(9));
			AudioManager.Instance.PlayClip("OrderPickUp");
			break;
		case 9:
			Destroy(tutObject);
			currScene = Resources.Load("TuTObject" + (scene.ToString()))as GameObject;
			tutObject = GameObjectUtils.AddChildGUI(canvas, currScene);
			tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(10));
			AudioManager.Instance.PlayClip("CustomerEating");
			break;
		case 10:
			Destroy(tutObject);
			currScene = Resources.Load("TuTObject" + (scene.ToString()))as GameObject;
			tutObject = GameObjectUtils.AddChildGUI(canvas, currScene);
			tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(11));
			AudioManager.Instance.PlayClip("CustomerReadyForCheck");
			break;
		case 11:
			//once the final button is clicked we boot into menu
			Application.LoadLevel(SceneUtils.RESTAURANT);
			break;
		}

	}
}
