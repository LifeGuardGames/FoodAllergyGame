using System.Collections;

public class ImmutableDataTempoGoal {

	private string id;
	public string ID {
		get { return id; }
	}

	private int tier;
	public int Tier {
		get { return tier; }
	}

	private float goalPointTierPercentage;
	public float GoalPointTierPercentage {
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
		tier = XMLUtils.GetInt(hashElements["Tier"] as IXMLNode);
		goalPointTierPercentage = XMLUtils.GetFloat(hashElements["Percentage"] as IXMLNode);
		timeLimit = XMLUtils.GetInt(hashElements["TimeLimit"] as IXMLNode);
		reward = XMLUtils.GetInt(hashElements["Reward"] as IXMLNode);
	}
}
