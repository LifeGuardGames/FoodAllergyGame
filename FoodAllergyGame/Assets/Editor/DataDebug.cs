﻿using UnityEngine;
using UnityEditor;
using System.Collections;

public class DataDebug : EditorWindow {

	[MenuItem("LGG/DataDebug")]
	public static void ShowWindow(){
		EditorWindow.GetWindow(typeof(DataDebug));
	}

	void OnGUI(){
		GUILayout.Label("DataManager Fields", EditorStyles.boldLabel);
		if(Application.isPlaying){
			if(Application.loadedLevelName != SceneUtils.COMICSCENE){
				EditorGUILayout.TextField("Event", DataManager.Instance.GameData.RestaurantEvent.CurrentEvent);
				EditorGUILayout.TextField("Current cash", DataManager.Instance.GameData.Cash.CurrentCash.ToString());
				EditorGUILayout.TextField("Total cash", DataManager.Instance.GameData.Cash.TotalCash.ToString());
				EditorGUILayout.TextField("Tier", TierManager.Instance.Tier.ToString());
			}
			if(Application.loadedLevelName == SceneUtils.RESTAURANT){
				EditorGUILayout.TextField("Move queue count", TouchManager.Instance.inputQueue.Count.ToString());
			}

		}
	}

	public void OnInspectorUpdate()
	{
		// This will only get called 10 times per second.
		Repaint();
	}
}
