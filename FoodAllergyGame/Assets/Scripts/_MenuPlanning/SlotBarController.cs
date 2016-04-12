using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SlotBarController : MonoBehaviour {

	public GameObject slotPrefab;
	public GridLayoutGroup gridLayout;
	public RectTransform gridImageTransform;

	public Sprite slotOnSprite;
	public Sprite slotOffSprite;

	private List<GameObject> slotList;		// List to keep track of the actual image slots
	private List<bool> slotListBoolAux;		// Aux to keep track of active or not for slotList

	public void Init(int totalSlots) {
		foreach(Transform child in gridLayout.transform) {
			Destroy(child);
		}

		// Calculate the bar size given grid layout perperties
		int padding = gridLayout.padding.left;
		float spacing = gridLayout.spacing.x;
		float cellWidth = gridLayout.cellSize.x;
		gridImageTransform.sizeDelta = new Vector2((padding * 2) + (cellWidth * totalSlots) + (spacing * (totalSlots - 1)),
													gridImageTransform.sizeDelta.y);

		slotList = new List<GameObject>();
		slotListBoolAux = new List<bool>();
		for(int i = 0; i < totalSlots; i++) {
			GameObject slot = GameObjectUtils.AddChildGUI(gridLayout.gameObject, slotPrefab);
			slot.name = "SlotBarSlot" + i;
			slotList.Add(slot);
			slotListBoolAux.Add(false);
        }
    }

	// Only returns true if activation is allowed
	public bool ActivateSlots(int slotCount) {
		for(int i = 0; i < slotList.Count; i++) {
			if(slotListBoolAux[i] == false) {					// Find the first instance of false
				if(i + slotCount > slotList.Count) {			// Overflow
					Debug.LogWarning("Slot Overflow");
					return false;
				}
				else {
					for(int j = i; j < i + slotCount; j++) {    // Check if slots are positioned nicely
						if(slotListBoolAux[j] == true) {	
							Debug.LogError("Invalid slot scattering");
							return false;
						}
					}
					for(int j = i; j < i + slotCount; j++) {	// Now actually change it
						slotListBoolAux[j] = true;
						slotList[j].GetComponent<Animation>().Play();
						slotList[j].GetComponent<Image>().sprite = slotOnSprite;
					}
					return true;
				}
			}
		}
		Debug.LogWarning("Cannot find any available slots");
		return false;
	}

	// Only returns true if deactivation is allowed
	public bool DeactivateSlots(int slotCount) {
		for(int i = slotList.Count - 1; i >= 0; i--) {
			if(slotListBoolAux[i] == true) {					// Find the first instance of true
				if(i - slotCount + 1 < 0) {
					Debug.LogError("Slot Underflow");
					return false;
				}
				else {
					for(int j = i; j > i - slotCount; j--) {	// Check if slots are positioned nicely
						if(slotListBoolAux[j] == false) {
							Debug.LogError("Invalid slot scattering");
							return false;
						}
					}
					for(int j = i; j > i - slotCount; j--) {   // Now actually change it
						slotListBoolAux[j] = false;
						slotList[j].GetComponent<Image>().sprite = slotOffSprite;
					}
					return true;
				}
			}
		}
		Debug.LogError("Cannot find any available slots");
		return false;
	}

	public bool IsSlotsFull() {
		return (!slotListBoolAux.Contains(false)) ? true : false;
	}

	//void OnGUI() {
	//	if(GUI.Button(new Rect(300, 100, 100, 100), "Reset")) {
	//		Init(12);
	//	}
	//	if(GUI.Button(new Rect(400, 100, 100, 100), "+2")) {
	//		ActivateSlots(2);
	//	}
	//	if(GUI.Button(new Rect(500, 100, 100, 100), "+1")) {
	//		ActivateSlots(1);
	//	}
	//	if(GUI.Button(new Rect(600, 100, 100, 100), "-2")) {
	//		DeactivateSlots(2);
	//	}
	//	if(GUI.Button(new Rect(700, 100, 100, 100), "-1")) {
	//		DeactivateSlots(1);
	//	}
	//}
}
