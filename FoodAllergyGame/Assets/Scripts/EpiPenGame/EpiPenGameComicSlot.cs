using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EpiPenGameComicSlot : MonoBehaviour, IDropHandler {

	public int slotNumber;

	#region IDropHandler implementation
	public void OnDrop(PointerEventData eventData) {
		Debug.Log("rdhbjnfgbkj");
		EpiPenGamePanel panel = EpiPenGamePanel.itemBeingDragged.GetComponent<EpiPenGamePanel>();
		EpiPenGameManager.Instance.submittedAnswers.Add(slotNumber, panel.order);
		panel.gameObject.transform.SetParent(this.transform);
	}
	#endregion
}
