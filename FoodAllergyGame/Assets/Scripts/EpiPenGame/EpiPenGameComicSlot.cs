using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EpiPenGameComicSlot : MonoBehaviour, IDropHandler {

	public int slotNumber;

	#region IDropHandler implementation
	public void OnDrop(PointerEventData eventData) {
		
		EpiPenGamePanel panel = EpiPenGamePanel.itemBeingDragged.GetComponent<EpiPenGamePanel>();
		Debug.Log(panel.gameObject.name);
		EpiPenGameManager.Instance.submittedAnswers.Add(slotNumber, panel.order);
		panel.gameObject.transform.SetParent(this.transform);
	}
	#endregion
}
