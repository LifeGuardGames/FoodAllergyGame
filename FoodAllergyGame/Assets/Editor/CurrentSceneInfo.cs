using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.SceneManagement;

public class CurrentSceneInfo : EditorWindow {

	[MenuItem("LGG/Current Scene Info")]
	public static void ShowWindow(){
		EditorWindow.GetWindow(typeof(CurrentSceneInfo));
	}

	void OnGUI(){
		GUILayout.Label("DataManager Fields", EditorStyles.boldLabel);
		if(Application.isPlaying){
			if(SceneManager.GetActiveScene().name != SceneUtils.COMICSCENE){
				EditorGUILayout.TextField("Event", DataManager.Instance.GameData.RestaurantEvent.CurrentEvent);
				EditorGUILayout.TextField("Current cash", DataManager.Instance.GameData.Cash.CurrentCash.ToString());
				EditorGUILayout.TextField("Total cash", DataManager.Instance.GameData.Cash.TotalCash.ToString());
				EditorGUILayout.TextField("Prev total cash", DataManager.Instance.GameData.Cash.LastSeenTotalCash.ToString());
				EditorGUILayout.TextField("Tier", TierManager.Instance.CurrentTier.ToString());
				EditorGUILayout.TextField("Notif queue count", NotificationManager.Instance.NotificationQueueCount.ToString());
			}
			if(SceneManager.GetActiveScene().name == SceneUtils.RESTAURANT){
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
