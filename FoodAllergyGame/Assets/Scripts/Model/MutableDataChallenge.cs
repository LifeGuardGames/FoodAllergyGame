
using System.Collections;
using System.Collections.Generic;

public class MutableDataChallenge{

	public Dictionary <string , ChallengeReward> challengeProgress;

	public MutableDataChallenge() {
		challengeProgress = new Dictionary<string, ChallengeReward>();
		List<ImmutableDataChallenge> temp = DataLoaderChallenge.GetDataList();
		for(int i = 0; i  < temp.Count; i++) {
			challengeProgress.Add(temp[i].ID, ChallengeReward.None);
		}
	}
}
