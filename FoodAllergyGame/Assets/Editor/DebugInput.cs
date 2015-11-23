using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using System.Xml.Serialization;

public class DebugInput : EditorWindow {

	private string CRITICAL_PATH = "/XML/Resources/_Critical.xml";
	private List<Constant> critList;
	private CriticalConstants criticalConstants;
	private Vector2 scrollPosition = Vector2.zero;

	[MenuItem("LGG/Debug Input")]
	public static void ShowWindow(){
		EditorWindow.GetWindow(typeof(DebugInput));
	}

	void OnFocus(){
		criticalConstants = Deserialize<CriticalConstants>(CRITICAL_PATH);
		critList = criticalConstants.CriticalConstantList;
	}

	void OnGUI(){
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false);

		GUILayout.Label("Plist Editor", EditorStyles.boldLabel);
		if(GUILayout.Button("Delete Plist")){
			PlayerPrefs.DeleteAll();
			PlayerPrefs.Save ();
		}
		GUILayout.Label("Debug fields" , EditorStyles.boldLabel);
		if(critList != null){
			string lastDebugType = "Core";	// Used for separators
			foreach(Constant constant in critList){

				// Separator
				if(lastDebugType != constant.ConstantDebug){
					GUILayout.Box("",GUILayout.Width(this.position.width - 30), GUILayout.Height(2));
					lastDebugType = constant.ConstantDebug;
				}

				if(constant.ConstantDebug == "Core"){
					switch(constant.ConstantType){
					case "Bool":
						bool toggleState = EditorGUILayout.Toggle(constant.Name, bool.Parse(constant.ConstantValue));
						constant.ConstantValue = toggleState.ToString();
						break;
					case "String":
						constant.ConstantValue = EditorGUILayout.TextField(constant.Name, constant.ConstantValue);
						break;
					case "Int":
						constant.ConstantValue = EditorGUILayout.TextField(constant.Name, constant.ConstantValue);
						break;
					}
				}
				else if(constant.ConstantDebug == "Tutorial"){
					switch(constant.ConstantType){
					case "Bool":
						bool toggleState = EditorGUILayout.Toggle(constant.Name, bool.Parse(constant.ConstantValue));
						constant.ConstantValue = toggleState.ToString();
						break;
					case "String":
						constant.ConstantValue = EditorGUILayout.TextField(constant.Name, constant.ConstantValue);
						break;
					case "Int":
						constant.ConstantValue = EditorGUILayout.TextField(constant.Name, constant.ConstantValue);
						break;
					}
				}
				else if(constant.ConstantDebug == "Deco"){
					switch(constant.ConstantType){
					case "Bool":
						bool toggleState = EditorGUILayout.Toggle(constant.Name, bool.Parse(constant.ConstantValue));
						constant.ConstantValue = toggleState.ToString();
						break;
					case "String":
						constant.ConstantValue = EditorGUILayout.TextField(constant.Name, constant.ConstantValue);
						break;
					case "Int":
						constant.ConstantValue = EditorGUILayout.TextField(constant.Name, constant.ConstantValue);
						break;
					}
				}
			}
			if(GUILayout.Button("Save", GUILayout.Height(50))){
				Serialize<CriticalConstants>(CRITICAL_PATH, criticalConstants);
			}
		}
		GUILayout.EndScrollView();
	}

	private void Serialize<T>(string filePath, object xmlData){
		XmlSerializer serializer = new XmlSerializer(typeof(T));
		string path = Application.dataPath + filePath;
		
		using(TextWriter writer = new StreamWriter(path, false)){
			serializer.Serialize(writer, (T) xmlData);
		}
		AssetDatabase.Refresh();
	}
	
	private T Deserialize<T>(string filePath){
		XmlSerializer deserializer = new XmlSerializer(typeof(T));
		string path = Application.dataPath + filePath; 
		TextReader reader = new StreamReader(path);
		object obj = deserializer.Deserialize(reader);
		reader.Close(); 
		return (T) obj;
	}
}
#endif