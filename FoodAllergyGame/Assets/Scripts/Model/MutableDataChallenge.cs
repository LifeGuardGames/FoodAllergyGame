using System.Collections.Generic;

public class MutableDataChallenge {
	public Dictionary <string , ChallengeReward> ChallengeProgress { get; set; }

	public MutableDataChallenge() {
		ChallengeProgress = new Dictionary<string, ChallengeReward>();
		List<ImmutableDataChallenge> temp = DataLoaderChallenge.GetDataList();
		for(int i = 0; i  < temp.Count; i++) {
			ChallengeProgress.Add(temp[i].ID, ChallengeReward.None);
		}
	}
}
