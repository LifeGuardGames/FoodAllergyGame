using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {
	public GameObject canvas;
	private GameObject currScene;
	private GameObject tutObject;
	// Use this for initialization
	void Start () {
		currScene = Resources.Load ("TuTObject0")as GameObject;
		tutObject = GameObjectUtils.AddChild(canvas, currScene);
		tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(1));
	}
	public void ChangeScene(int scene){
		Debug.Log (scene);
		switch(scene){
		case 1:
			Destroy(tutObject);
			currScene = Resources.Load ("TuTObject" + (scene.ToString()))as GameObject;
			tutObject = GameObjectUtils.AddChild(canvas, currScene);
			tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(2));
			break;
		case 2:
			Destroy(tutObject);
			currScene = Resources.Load ("TuTObject" + (scene.ToString()))as GameObject;
			tutObject = GameObjectUtils.AddChild(canvas, currScene);
			tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(3));
			break;
		case 3:
			Destroy(tutObject);
			currScene = Resources.Load ("TuTObject" + (scene.ToString()))as GameObject;
			tutObject = GameObjectUtils.AddChild(canvas, currScene);
			tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(4));
			break;
		case 4:
			Destroy(tutObject);
			currScene = Resources.Load ("TuTObject" + (scene.ToString()))as GameObject;
			tutObject = GameObjectUtils.AddChild(canvas, currScene);
			tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(5));
			break;
		case 5:
			Destroy(tutObject);
			currScene = Resources.Load ("TuTObject" + (scene.ToString()))as GameObject;
			tutObject = GameObjectUtils.AddChild(canvas, currScene);
			tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(6));
			break;
		case 6:
			Destroy(tutObject);
			currScene = Resources.Load ("TuTObject" + (scene.ToString()))as GameObject;
			tutObject = GameObjectUtils.AddChild(canvas, currScene);
			tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(7));
			break;
		case 7:
			Destroy(tutObject);
			currScene = Resources.Load ("TuTObject" + (scene.ToString()))as GameObject;
			tutObject = GameObjectUtils.AddChild(canvas, currScene);
			tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(8));
			break;
		case 8:
			Destroy(tutObject);
			currScene = Resources.Load ("TuTObject" + (scene.ToString()))as GameObject;
			tutObject = GameObjectUtils.AddChild(canvas, currScene);
			tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(9));
			break;
		case 9:
			Destroy(tutObject);
			currScene = Resources.Load ("TuTObject" + (scene.ToString()))as GameObject;
			tutObject = GameObjectUtils.AddChild(canvas, currScene);
			tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(10));
			break;
		case 10:
			Destroy(tutObject);
			currScene = Resources.Load ("TuTObject" + (scene.ToString()))as GameObject;
			tutObject = GameObjectUtils.AddChild(canvas, currScene);
			tutObject.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeScene(11));
			break;
		case 11:
			Application.LoadLevel(SceneUtils.MENUPLANNING);
			break;
		}

	}
}
