using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Pretty delicate class here, there are alot of tween and demuxes that are dependent on state
public class ShowcaseController : MonoBehaviour {

	public TweenToggle fadeTween;						// Only for fade
	public TweenToggleDemux showcaseDemux;				// Only for title, description, and image
	public TweenToggleDemux buttonParentBoughtDemux;	// Only for bought buttons
	public TweenToggleDemux buttonParentUnboughtDemux;	// Only for unbought buttons

	public Text titleText;
	public Text descriptionText;
	public Text costText;
	public Image decoImage;

	private ImmutableDataDecoItem currentDeco = null;
	private string auxCallbackString = "CallbackHelper";	// Toggles the callback on showcaseDemux if present
															// Save aux copy, turn off in preview mode

	public void ShowInfo(string decoID){
		ImmutableDataDecoItem decoData = DataLoaderDecoItem.GetData(decoID);
		ShowInfo(decoData);
	}

	public void ShowInfo(ImmutableDataDecoItem decoData){
		// Initial case, first start, just show all data immediately
		if(currentDeco == null){
			currentDeco = decoData;
			StartCurrentDeco(decoData);
		}
		// Already showing something else, tween that out before tweening new ones
		else{

			currentDeco = decoData;
			// Hide callback will handle the rest
			showcaseDemux.Hide();
		}
	}

	public void CallbackHelper(){
		StartCurrentDeco(currentDeco);
	}

	// Callback - An item is clicked, show the showcase UI plus its buttons
	private void StartCurrentDeco(ImmutableDataDecoItem decoData){
		fadeTween.Show();

		decoImage.enabled = decoData.SpriteName == "None" ? false : true;
		decoImage.sprite = SpriteCacheManager.GetDecoSpriteData(decoData.SpriteName);
		titleText.text = LocalizationText.GetText(decoData.TitleKey);

		descriptionText.text = LocalizationText.GetText(decoData.DescriptionKey);
		if(descriptionText.text.Contains("No text for ")){	// Hard code ignore empty text
			descriptionText.text = "";
		}

		costText.text = decoData.Cost.ToString();

		// Show the corrosponding buttons based on item state
		if(DecoManager.IsDecoBought(decoData.ID)){
			if(DecoManager.IsDecoActive(decoData.ID)){
				buttonParentBoughtDemux.Hide();
				buttonParentUnboughtDemux.Hide();
			}
			else{
				buttonParentBoughtDemux.Show();
				buttonParentUnboughtDemux.Hide();
			}
		}
		else{
			buttonParentBoughtDemux.Hide();
			buttonParentUnboughtDemux.Show();
		}
		showcaseDemux.Show();
	}

//	public void OnTryButtonClicked(){
//		buttonParentBoughtDemux.Hide();
//		buttonParentUnboughtDemux.Hide();
//		backButtonTween.Show();
//	}

	public void OnBuyButtonClicked(){
		DecoManager.Instance.SetDeco(currentDeco.ID);
	}

	public void OnEquipButtonClicked(){
		DecoManager.Instance.SetDeco(currentDeco.ID);
	}

	// Hide everything but dont touch UI state
	public void EnterViewMode(){
		showcaseDemux.HideFunctionName = "";	// Disable hide callback
		showcaseDemux.Hide();
		buttonParentBoughtDemux.Hide();
		buttonParentUnboughtDemux.Hide();
		fadeTween.Hide();
	}

	// Showing everything with same UI state
	public void ExitViewMode(){
		showcaseDemux.Show();
		fadeTween.Show();
		showcaseDemux.HideFunctionName = auxCallbackString;		// Enable hide callback
	}
}
