using UnityEngine;
using UnityEngine.EventSystems;

public class EpiPenGameSlot : MonoBehaviour, IDropHandler {
	public bool isFinalSlot = true;		//Final slot vs Aux slot
	public int slotNumber;

	#region IDropHandler implementation
	public void OnDrop(PointerEventData eventData) {
		if(EpiPenGameToken.itemBeingDragged != null && transform.childCount == 0) {
			EpiPenGameToken panel = EpiPenGameToken.itemBeingDragged.GetComponent<EpiPenGameToken>();
			panel.transform.SetParent(transform);
			panel.transform.localPosition = Vector3.zero;

			if(isFinalSlot) {
				EpiPenGameManager.Instance.submittedAnswers.Add(slotNumber, panel.order);
			}
		}
	}
	#endregion
}
