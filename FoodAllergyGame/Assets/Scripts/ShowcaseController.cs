using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Pretty delicate class here, there are alot of tween and demuxes that are dependent on state
public class ShowcaseController : MonoBehaviour {

	public TweenToggle fadeTween;						// Only for fade
	public TweenToggleDemux showcaseDemux;				// Only for title, description, and image
	public TweenToggleDemux buttonParentBoughtDemux;	// Only for bought buttons
	public TweenToggleDemux buttonParentUnboughtDemux;  // Only for unbought buttons
	public TweenToggleDemux buttonParentActiveDemux;	// Only for active buttons

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
	private void StartCurrentDeco(ImmutableDataDecoItem decoData) {
		fadeTween.Show();
		decoImage.sprite = SpriteCacheManager.GetDecoSpriteData(decoData.SpriteName);
		titleText.text = LocalizationText.GetText(decoData.TitleKey);

		descriptionText.text = LocalizationText.GetText(decoData.DescriptionKey);
		if(descriptionText.text == "None") {						// Hard code ignore empty text
			descriptionText.text = "";
		}

		costText.text = decoData.Cost.ToString();
		
		// Show the corrosponding buttons based on item state
		if(!DecoManager.Instance.IsDecoUnlocked(decoData.ID)) {		// Locked
			buttonParentBoughtDemux.Hide();
			buttonParentUnboughtDemux.Hide();
			buttonParentActiveDemux.Hide();
		}
		else if(DecoManager.IsDecoBought(decoData.ID)){
			if(DecoManager.IsDecoActive(decoData.ID)){				// Bought/Active
				buttonParentBoughtDemux.Hide();
				buttonParentUnboughtDemux.Hide();

				// Core decorations can not be removed
				if(DecoManager.IsDecoRemoveAllowed(decoData.Type)) {
					buttonParentActiveDemux.Show();
				}
				else {
					buttonParentActiveDemux.Hide();
				}
            }
			else{													// Bought/Inactive
				buttonParentBoughtDemux.Show();
				buttonParentUnboughtDemux.Hide();
				buttonParentActiveDemux.Hide();
            }
		}
		else{														// Inactive
			buttonParentBoughtDemux.Hide();
			buttonParentUnboughtDemux.Show();
			buttonParentActiveDemux.Hide();
        }
		showcaseDemux.Show();
	}

	public void OnBuyButtonClicked(){
		if(currentDeco.Type != DecoTypes.IAP) {
			DecoManager.Instance.SetDeco(currentDeco.ID, currentDeco.Type);
		}
		else {
			DecoManager.Instance.SetDeco(currentDeco.ID, currentDeco.IapType);
		}
	}

	public void OnEquipButtonClicked(){
		if(currentDeco.Type != DecoTypes.IAP) {
			DecoManager.Instance.SetDeco(currentDeco.ID, currentDeco.Type);
		}
		else {
			DecoManager.Instance.SetDeco(currentDeco.ID, currentDeco.IapType);
		}
	}

	public void OnRemoveButtonClicked(){
		if(currentDeco.Type != DecoTypes.IAP) {
			DecoManager.Instance.SetDeco(currentDeco.ID, currentDeco.Type);
		}
		else {
			DecoManager.Instance.SetDeco(currentDeco.ID, currentDeco.IapType);
		}
	}

	// Hide everything but dont touch UI state
	public void EnterViewMode(){
		showcaseDemux.HideFunctionName = "";	// Disable hide callback
		showcaseDemux.Hide();
		buttonParentBoughtDemux.Hide();
		buttonParentUnboughtDemux.Hide();
		buttonParentActiveDemux.Hide();
		fadeTween.Hide();
	}

	// Showing everything with same UI state
	public void ExitViewMode(){
		// Clear last current deco for fresh display
		ImmutableDataDecoItem decoAux = currentDeco;
		currentDeco = null;
        ShowInfo(decoAux);

        fadeTween.Show();
		showcaseDemux.HideFunctionName = auxCallbackString;		// Enable hide callback
	}
}
