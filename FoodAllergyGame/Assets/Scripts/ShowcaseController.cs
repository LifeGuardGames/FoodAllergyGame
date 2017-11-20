using UnityEngine;
using UnityEngine.UI;

// Pretty delicate class here, there are alot of tween and demuxes that are dependent on state
public class ShowcaseController : MonoBehaviour {
	public TweenToggle fadeTween;				// Only for fade
	public GameObject content;					// Only for title, description, and image
	public GameObject buttonParentBought;		// Only for bought buttons
	public GameObject buttonParentUnbought;		// Only for unbought buttons
	public GameObject buttonParentActive;		// Only for active buttons

	public Text titleText;
	public Text descriptionText;
	public Text coinCostText;
	public Text iapCostText;
	public Image decoImage;
	public Image costImage;

	private ImmutableDataDecoItem currentDeco = null;

	public void ShowInfo(string decoID){
		ShowInfo(DataLoaderDecoItem.GetData(decoID));
	}

	// An item is clicked, show the showcase UI plus its buttons
	public void ShowInfo(ImmutableDataDecoItem decoData) {
		fadeTween.Show();
		currentDeco = decoData;
		decoImage.sprite = SpriteCacheManager.GetDecoSpriteData(decoData.SpriteName);
		titleText.text = LocalizationText.GetText(decoData.TitleKey);

		descriptionText.text = LocalizationText.GetText(decoData.DescriptionKey);
		if(descriptionText.text == "None") {						// Hard code ignore empty text
			descriptionText.text = "";
		}
		
		// Show the corrosponding buttons based on item state
		if(!DecoManager.Instance.IsDecoUnlocked(decoData.ID)) {		// Locked
			buttonParentBought.SetActive(false);
			buttonParentUnbought.SetActive(false);
			buttonParentActive.SetActive(false);
		}
		else if(DecoManager.IsDecoBought(decoData.ID)){
			if(DecoManager.IsDecoActive(decoData.ID)){				// Bought/Active
				buttonParentBought.SetActive(false);
				buttonParentUnbought.SetActive(false);

				// Core decorations can not be removed
				if(DecoManager.IsDecoRemoveAllowed(decoData.Type)) {
					buttonParentActive.SetActive(true);
				}
				else {
					buttonParentActive.SetActive(false);
				}
            }
			else{													// Bought/Inactive
				buttonParentBought.SetActive(true);
				buttonParentUnbought.SetActive(false);
				buttonParentActive.SetActive(false);
			}
		}
		else{														// Not Bought/Inactive
			buttonParentBought.SetActive(false);
			buttonParentUnbought.SetActive(true);
			buttonParentActive.SetActive(false);

			if(decoData.DecoTabType != DecoTabTypes.IAP) {
				costImage.sprite = Resources.Load<Sprite>("Coin");
				costImage.enabled = true;

				iapCostText.enabled = false;
				coinCostText.enabled = true;
				coinCostText.text = decoData.Cost.ToString();
			}
			else {
				costImage.enabled = false;

				coinCostText.enabled = false;
				iapCostText.enabled = true;
				iapCostText.text = DataManager.Instance.GameData.DayTracker.IsAmazonUnderground ? "Free" : "$0.99";
			}
		}
		content.SetActive(true);
	}

	public void OnBuyButtonClicked(){
		DecoManager.Instance.SetDeco(currentDeco.ID, currentDeco.Type);
	}

	public void OnEquipButtonClicked(){
		DecoManager.Instance.SetDeco(currentDeco.ID, currentDeco.Type);
	}

	public void OnRemoveButtonClicked(){
		DecoManager.Instance.SetDeco(null, currentDeco.Type);
	}

	// Hide everything but dont touch UI state
	public void EnterViewMode(){
		Debug.Log("Entering view mode");
		content.SetActive(false);
		buttonParentBought.SetActive(false);
		buttonParentUnbought.SetActive(false);
		buttonParentActive.SetActive(false);
		fadeTween.Hide();
	}

	// Showing everything with same UI state
	public void ExitViewMode(){
		Debug.Log("Exiting view mode");

        ShowInfo(currentDeco);
        fadeTween.Show();
	}
}
