//// Copyright (c) 2015 LifeGuard Games Inc.

using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditor.SceneManagement;

public class SceneItem : Editor {

	[MenuItem("Open Scene/ComicScene")]
	public static void OpenComicScene(){
		OpenScene(SceneUtils.COMICSCENE);
	}

	[MenuItem("Open Scene/StartScene")]
	public static void OpenStartScene(){
		OpenScene(SceneUtils.START);
	}

	[MenuItem("Open Scene/MenuPlanning")]
	public static void OpenMenuPlanning(){
		OpenScene(SceneUtils.MENUPLANNING);
	}

	[MenuItem("Open Scene/Restaurant")]
	public static void OpenRestaurant(){
		OpenScene(SceneUtils.RESTAURANT);
	}

	[MenuItem("Open Scene/DecoScene")]
	public static void OpenDeco(){
		OpenScene(SceneUtils.DECO);
	}

	[MenuItem("Open Scene/ChallengeMenu")]
	public static void OpenChallengeMenu() {
		OpenScene(SceneUtils.CHALLENGEMENU);
	}

	[MenuItem("Open Scene/EpiPenGame")]
	public static void OpenEpiPenGame() {
		OpenScene(SceneUtils.EPIPEN);
	}

	[MenuItem("Open Scene/CheatyScene")]
	public static void OpenCheaty() {
		OpenScene(SceneUtils.CHEATY);
	}

	static void OpenScene(string name){
		if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()){
			EditorSceneManager.OpenScene("Assets/Scenes/" + name + ".unity");
		}
	}
}
