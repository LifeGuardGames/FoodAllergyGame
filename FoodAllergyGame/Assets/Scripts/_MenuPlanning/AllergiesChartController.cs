using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AllergiesChartController : MonoBehaviour {

	public Color low;
	public Color medium;
	public Color high;

	public Text peanutCountText;
	public Text dairyCountText;
	public Text wheatCountText;
	public Image peanutBar;
	public Image wheatBar;
	public Image dairyBar;

	public RectTransform peanutSpriteBase;
	public RectTransform PeanutSpriteBase{
		get{ return peanutSpriteBase; }
	}

	public RectTransform dairySpriteBase;
	public RectTransform DairySpriteBase{
		get{ return dairySpriteBase; }
	}

	public RectTransform wheatSpriteBase;
	public RectTransform WheatSpriteBase{
		get{ return wheatSpriteBase; }
	}

	private int colorChunkLimit;
	private int menuSize;

	void Start(){
		peanutBar.rectTransform.sizeDelta = new Vector2(50f, 0f);
		wheatBar.rectTransform.sizeDelta = new Vector2(50f, 0f);
		dairyBar.rectTransform.sizeDelta = new Vector2(50f, 0f);
	}

	public void Init(int menuSize){
		colorChunkLimit = menuSize / 3;			// Divisor with cutoff
		this.menuSize = menuSize;
	}

	public void UpdateChart(){
		int peanutCount = 0;
		int dairyCount = 0;
		int wheatCount = 0;

		foreach(string menuFoodID in MenuManager.Instance.SelectedMenuStringList){
			ImmutableDataFood foodData = DataLoaderFood.GetData(menuFoodID);
			foreach(Allergies allergy in foodData.AllergyList){
				switch(allergy){
				case Allergies.Peanut:
					peanutCount++;
					break;
				case Allergies.Dairy:
					dairyCount++;
					break;
				case Allergies.Wheat:
					wheatCount++;
					break;
				default:
					break;
				}
			}
		}

		peanutCountText.text = peanutCount.ToString();
		ChangeBar(peanutBar, peanutCount);

		dairyCountText.text = dairyCount.ToString();
		ChangeBar(dairyBar, dairyCount);

		wheatCountText.text = wheatCount.ToString();
		ChangeBar(wheatBar, wheatCount);
	}

	private void ChangeBar(Image image, int count){
		// Change the color of the bar
		int colorTier = count / colorChunkLimit;
		switch(colorTier){
		case 0:	// Do nothing
			break;
		case 1:
			image.color = low;
			break;
		case 2:
			image.color = medium;
			break;
		case 3:
			image.color = high;
			break;
		default:
			Debug.LogError("Invalid count for chart: " + colorTier);
			break;
		}

		// Change the size of the bar based on total menuSize count
		float percentageHeight = (float)count / (float)menuSize;
		image.rectTransform.sizeDelta = new Vector2(50f, percentageHeight * 238);
	}
}
