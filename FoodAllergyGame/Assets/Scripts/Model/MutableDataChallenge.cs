using System.Collections.Generic;

public class MutableDataChallenge {
	public bool IsFirstTimeChallengeEntrance { get; set; }
	public Dictionary <string , ChallengeReward> ChallengeProgress { get; set; }

	public MutableDataChallenge() {
		IsFirstTimeChallengeEntrance = true;

		ChallengeProgress = new Dictionary<string, ChallengeReward>();
		List<ImmutableDataChallenge> temp = DataLoaderChallenge.GetDataList();
		for(int i = 0; i  < temp.Count; i++) {
			ChallengeProgress.Add(temp[i].ID, ChallengeReward.None);
		}
	}

	public void PostLogicCheck() {
		List<ImmutableDataChallenge> temp = DataLoaderChallenge.GetDataList();
		for(int i = 0; i < temp.Count; i++) {
			if(!ChallengeProgress.ContainsKey(temp[i].ID)) {
				ChallengeProgress.Add(temp[i].ID, ChallengeReward.None);
			}
		}
	}
}
