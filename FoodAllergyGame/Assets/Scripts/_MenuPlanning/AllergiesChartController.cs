using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AllergiesChartController : MonoBehaviour {

	public Color low;
	public Color medium;
	public Color high;

	public Text peanutText;
	public Text peanutCountText;
	public Text dairyText;
	public Text dairyCountText;
	public Text wheatText;
	public Text wheatCountText;

	public void UpdateChart(List<string> menuList){
		int peanutCount = 0;
		int dairyCount = 0;
		int wheatCount = 0;

		foreach(string menuFoodID in menuList){
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
		ChangeLabelColors(peanutText, peanutCountText, peanutCount);

		dairyCountText.text = dairyCount.ToString();
		ChangeLabelColors(dairyText, dairyCountText, dairyCount);

		wheatCountText.text = wheatCount.ToString();
		ChangeLabelColors(wheatText, wheatCountText, wheatCount);
	}

	private void ChangeLabelColors(Text text1, Text text2, int count){
		switch(count){
		case 0:
			text1.color = low;
			text2.color = low;
			break;
		case 1:
			text1.color = low;
			text2.color = low;
			break;
		case 2:
			text1.color = medium;
			text2.color = medium;
			break;
		case 3:
			text1.color = high;
			text2.color = high;
			break;
		case 4:
			text1.color = high;
			text2.color = high;
			break;
		case 5:
			text1.color = high;
			text2.color = high;
			break;
		default:
			Debug.LogError("Invalid count for chart");
			break;
		}
	}
}
