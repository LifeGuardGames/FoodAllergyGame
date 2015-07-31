using UnityEditor;
using UnityEngine;
using System.Collections;

public class SatisfactionAIProfiler : EditorWindow {
	private int testsToRun = 10;
	private int steps = 0;

	[MenuItem("LGG/SatisfactionAIProfiler")]
	public static void ShowWindow(){
		EditorWindow.GetWindow(typeof(SatisfactionAIProfiler));
	}


	void OnGUI(){
//		GUILayout.Label("Toggle UI Elements", EditorStyles.boldLabel);
		
		if(GUILayout.Button("Run AI script")){
			RunTest();
		}
	}

	private void RunTest(){
		Debug.Log("Running profile test...");
		SatisfactionAI ai = new SatisfactionAI();
		for(int i = 0; i < testsToRun; i++){
			Debug.Log("Test " + i.ToString() + ":");
			SingleTest(ai);
		}
	}

	// Returns number of steps for a single test
	private int SingleTest(SatisfactionAI ai){
		Debug.Log("Count : ");
		Debug.Log("Steps : ");
	}
}
