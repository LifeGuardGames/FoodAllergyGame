using UnityEngine;
using System.Collections;

public class ImmutableDataGoals {

	private string id;
	public string ID {
		get { return id; }
	}

	private int goalPoint;
	public int GoalPoint {
		get { return goalPoint; }
	}

	private int timeLimit;
	public int TimeLimit {
		get { return timeLimit; }
	}

	private int reward;
	public int Reward {
		get { return reward; }
	}

	public ImmutableDataGoals(string id, IXMLNode xmlNode, string error) {
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);
		this.id = id;

		goalPoint = XMLUtils.GetInt(hashElements["Amount"] as IXMLNode);
		timeLimit = XMLUtils.GetInt(hashElements["TimeLimit"] as IXMLNode);
		reward = XMLUtils.GetInt(hashElements["Reward"] as IXMLNode);
	}
}
