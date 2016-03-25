using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChallengeDescriptionController : MonoBehaviour {
	public Text description;
	public Text title;
	public Text bronzeBrake;
	public Text silverBrake;
	public Text goldBrake;
	public Button accept;
	public TweenToggle tween;

	private string currentChallengeID;

	public void Populate(string challengeID) {
		currentChallengeID = challengeID;
		ImmutableDataChallenge challengeData = DataLoaderChallenge.GetData(challengeID);
		title.text = LocalizationText.GetText(challengeData.Title);
		description.text = LocalizationText.GetText(challengeData.ChallengeDescription);
		bronzeBrake.text = challengeData.BronzeBreakPoint.ToString();
		silverBrake.text = challengeData.SilverBreakPoint.ToString();
		goldBrake.text = challengeData.GoldBreakPoint.ToString();
		tween.Show();
	}

	public void OnCancelButton(){
		tween.Hide();
	}

	public void OnPlayButton(){
		if(!string.IsNullOrEmpty(currentChallengeID)){
			ChallengeMenuManager.Instance.StartChallenge(currentChallengeID);
		}
		else{
			Debug.LogError("Null string for challengeID");
		}
	}
}
