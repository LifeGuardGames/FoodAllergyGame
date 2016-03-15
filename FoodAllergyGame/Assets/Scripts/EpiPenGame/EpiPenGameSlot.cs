using UnityEngine;
using UnityEngine.EventSystems;

public class EpiPenGameSlot : MonoBehaviour, IDropHandler {
	public bool isFinalSlot = true;		//Final slot vs Aux slot
	public int slotNumber;

	public EpiPenGameToken GetToken() {
		return GetComponentInChildren<EpiPenGameToken>();
	}

	// Used for resetting the game
	public void ClearToken() {
		EpiPenGameToken token = GetToken();
		if(token != null) {
			Destroy(token.gameObject);
		}
	}

	// Sets token as this slot's child
	public void SetToken(EpiPenGameToken token) {
		token.transform.SetParent(transform);
		token.transform.localPosition = Vector3.zero;

		RectTransform rect = token.GetComponent<RectTransform>();
		rect.offsetMin = new Vector2(10, 10);
		rect.offsetMax = new Vector2(-10, -10);
	}

	#region IDropHandler implementation
	public void OnDrop(PointerEventData eventData) {
		if(EpiPenGameToken.itemBeingDragged != null &&
			((isFinalSlot && transform.childCount == 1) || (!isFinalSlot && transform.childCount == 0))) {

			SetToken(EpiPenGameToken.itemBeingDragged.GetComponent<EpiPenGameToken>());
		}
	}
	#endregion
}
