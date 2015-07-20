//// Copyright (c) 2015 LifeGuard Games Inc.

using UnityEngine;
using UnityEditor;
using System.Collections;

public class SceneItem : Editor {

	[MenuItem("Open Scene/ComicScene")]
	public static void OpenComicScene(){
		OpenScene(SceneUtils.COMICSCENE);
	}

	[MenuItem("Open Scene/StartScene")]
	public static void OpenStartScene(){
		OpenScene(SceneUtils.START);
	}

	[MenuItem("Open Scene/TutScene")]
	public static void OpenTutScene(){
		OpenScene(SceneUtils.TUTSCENE);
	}

	[MenuItem("Open Scene/MenuPlanning")]
	public static void OpenMenuPlanning(){
		OpenScene(SceneUtils.MENUPLANNING);
	}

	[MenuItem("Open Scene/Restaurant")]
	public static void OpenRestaurant(){
		OpenScene(SceneUtils.RESTAURANT);
	}

	static void OpenScene(string name){
		if(EditorApplication.SaveCurrentSceneIfUserWantsTo()){
			EditorApplication.OpenScene("Assets/Scenes/" + name + ".unity");
		}
	}
}
