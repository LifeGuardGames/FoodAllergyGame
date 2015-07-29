using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class UIHelper : EditorWindow {
	private GameObject[] tagsList;
	private string currentScene = "";
	private GameObject UIElementListObject;	// Object in the scene that keeps of all UIElements
	private List<GameObject> UIElementsList;
	private Dictionary<GameObject, bool> elementBoolList = new Dictionary<GameObject, bool>();
	private bool isCompileAux = false;

	[MenuItem("LGG/UIHelper")]
	public static void ShowWindow(){
		EditorWindow.GetWindow(typeof(UIHelper));
	}

	public void OnInspectorUpdate(){
		if(EditorApplication.isCompiling){
			isCompileAux = true;
		}
		bool isFinishedCompiling = isCompileAux && !EditorApplication.isCompiling;

		if(currentScene != EditorApplication.currentScene || isFinishedCompiling){
			isCompileAux = false;
//			Debug.Log("Loading ui helpers");
			Repaint();
			// Attempt to load
			currentScene = EditorApplication.currentScene;
			tagsList = GameObject.FindGameObjectsWithTag("UIElementList");
			if(tagsList.Length > 0){
				UIElementsList = GameObject.FindGameObjectsWithTag("UIElementList")[0].GetComponent<UIHelperList>().UIElementList;

				elementBoolList = new Dictionary<GameObject, bool>();
				foreach(GameObject go in UIElementsList){
					elementBoolList.Add(go, go.activeSelf);
				}
			}
		}
	}

	void OnGUI(){
		if(tagsList != null && tagsList.Length > 0){
			GUILayout.Label("Toggle UI Elements", EditorStyles.boldLabel);

			if(GUILayout.Button("Disable All")){
				DisableUIElements();
			}

			foreach(GameObject go in UIElementsList){
				GUILayout.BeginHorizontal();
				GUILayout.Toggle(elementBoolList[go], "");
				GUILayout.Label(go.name);
				if(GUILayout.Button("Solo", GUILayout.Width(50))){
					SoloUI(go);
				}
				GUILayout.EndHorizontal();
			}

			if(GUILayout.Button("Reset", GUILayout.Height(50))){
				ResetUIElements();
			}
		}
	}

	// Singles out the UI element passed into the list
	private void SoloUI(GameObject soloGo){
		foreach(GameObject go in UIElementsList){
			go.SetActive(go == soloGo ? true : false);
		}
	}

	// Disables all the UI elements
	private void DisableUIElements(){
		foreach(GameObject go in UIElementsList){
			go.SetActive(false);
		}
	}

	// Go through the UI list and reset them to its original enable state
	private void ResetUIElements(){
		foreach(GameObject go in UIElementsList){
			go.SetActive(elementBoolList[go]);
		}
	}
}
