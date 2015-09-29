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
	public Image decoImage;

	private string currentDeco = "";
	private string auxCallbackString = "StartCurrentDeco";	// Toggles the callback on showcaseDemux if present
															// Save aux copy, turn off in preview mode

	public void ShowInfo(string decoID){
		ImmutableDataDecoItem decoData = DataLoaderDecoItem.GetData(decoID);
		ShowInfo(decoData);
	}

	public void ShowInfo(ImmutableDataDecoItem decoData){
		// Initial case, first start, just show all data immediately
		if(string.IsNullOrEmpty(currentDeco)){
			currentDeco = decoData.ID;
			StartCurrentDeco(decoData);
		}
		// Already showing something else, tween that out before tweening new ones
		else{
			// Hide callback will handle the rest
			showcaseDemux.Hide();
		}
	}

	// Callback - An item is clicked, show the showcase UI plus its buttons
	private void StartCurrentDeco(ImmutableDataDecoItem decoData){
		fadeTween.Show();
		
		Debug.Log(decoData.ID);
		Debug.Log(decoData.DescriptionKey );
		decoImage.sprite = SpriteCacheManager.GetDecoSpriteData(decoData.SpriteName);
		titleText.text = LocalizationText.GetText(decoData.TitleKey);
		descriptionText.text = LocalizationText.GetText(decoData.DescriptionKey);

		// Show the corrosponding buttons based on item state
		if(DecoManager.IsDecoBought(decoData.ID)){
			if(!DecoManager.IsDecoActive(decoData.ID)){
				buttonParentBoughtDemux.Show();
			}
		}
		else{
			buttonParentUnboughtDemux.Show();
		}
	}

	public void OnTryButtonClicked(){

	}

	public void OnBuyButtonClicked(){

	}

	public void OnEquipButtonClicked(){

	}

	// Hide everything but dont touch UI state
	public void EnterPreviewMode(){
		showcaseDemux.HideFunctionName = "";	// Disable hide callback
		showcaseDemux.Hide();
	}

	// Showing everything with same UI state
	public void ExitPreviewMode(){
		showcaseDemux.HideFunctionName = auxCallbackString;		// Enable hide callback
		showcaseDemux.Show();
	}
}
