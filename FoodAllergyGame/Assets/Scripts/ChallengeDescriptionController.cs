using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChallengeDescriptionController : MonoBehaviour {

	public Text description;
	public Text title;
	public Text bronzeBrake;
	public Text silverBrake;
	public Text goldBrake;
	public Button accept;
	public TweenToggle tween;

	public void Populate(string challengeID) {
		ImmutableDataChallenge details = DataLoaderChallenge.GetData(challengeID);
		title.GetComponent<Localize>().key = details.Title;
		title.GetComponent<Localize>().LocalizeText();
		description.GetComponent<Localize>().key = details.ChallengeDescription;
		description.GetComponent<Localize>().LocalizeText(); ;
		bronzeBrake.text = details.BronzeBreakPoint.ToString();
		silverBrake.text = details.SilverBreakPoint.ToString();
		goldBrake.text = details.GoldBreakPoint.ToString();
		accept.onClick.AddListener(() => ChallengeMenuManager.Instance.StartChallenge(challengeID));
		tween.Show();
	}

	public void Cancel() {
		tween.Hide();
	} 
}
