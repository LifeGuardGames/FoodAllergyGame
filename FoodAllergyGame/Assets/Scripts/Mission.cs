using UnityEngine;
using System.Collections;

public class Mission {
	public MissionType misType {set; get; }
	public string missionKey { set; get; }
	public int amount { set; get; }
	public int progress = 0;
	public int reward { set; get; }
}


