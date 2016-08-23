using System.Collections;

public class ImmutableDataTempoGoal {

	private string id;
	public string ID {
		get { return id; }
	}

	private int goalPointTierPercentage;
	public int GoalPointTierPercentage {
		get { return goalPointTierPercentage; }
	}

	private int timeLimit;
	public int TimeLimit {
		get { return timeLimit; }
	}

	private int reward;
	public int Reward {
		get { return reward; }
	}

	public ImmutableDataTempoGoal(string id, IXMLNode xmlNode, string error) {
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);
		this.id = id;

		goalPointTierPercentage = XMLUtils.GetInt(hashElements["Percentage"] as IXMLNode);
		timeLimit = XMLUtils.GetInt(hashElements["TimeLimit"] as IXMLNode);
		reward = XMLUtils.GetInt(hashElements["Reward"] as IXMLNode);
	}
}
