using UnityEngine;
using System.Collections;

public class SelectedMenuController : MonoBehaviour {

	public GameObject selectedSlotPrefab;
	public GameObject selectedSlotGrid;

	public void Init(int slots){
		for(int i = 0; i < slots; i++){
			GameObject slot = GameObjectUtils.AddChildGUI(selectedSlotGrid, selectedSlotPrefab);
			slot.name = "SelectedSlot" + i.ToString();
		}
	}
}
