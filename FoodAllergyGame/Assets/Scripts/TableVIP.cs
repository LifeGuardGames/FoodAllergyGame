using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TableVIP : Table {

	// Use this for initialization
	void Start () {
		Init();
	}
	public override void Init() {
		if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName(SceneUtils.DECO)) {
			base.Init();
			node = Pathfinding.Instance.NodeVIP;
			CustomerUIController customerUI = this.GetComponent<CustomerUIController>();
			customerUI.ToggleWait(false);
			customerUI.ToggleStar(false);
			customerUI.ToggleAllergyAttack(false);
			this.GetComponent<BoxCollider>().enabled = true;
		}
	}

	public override void CustomerLeaving() {
		base.CustomerLeaving();
		CustomerUIController customerUI = this.GetComponent<CustomerUIController>();
		customerUI.satisfaction1.gameObject.SetActive(false);
		customerUI.satisfaction2.gameObject.SetActive(false);
		customerUI.satisfaction3.gameObject.SetActive(false);
		customerUI.satisfaction4.gameObject.SetActive(false);
		customerUI.ToggleStar(false);
		customerUI.ToggleAllergyAttack(false);
		customerUI.ToggleWait(false);
	}

	public override void TableSmashed() {
		base.TableSmashed();
		CustomerUIController customerUI = this.GetComponent<CustomerUIController>();
		customerUI.satisfaction1.gameObject.SetActive(false);
		customerUI.satisfaction2.gameObject.SetActive(false);
		customerUI.satisfaction3.gameObject.SetActive(false);
		customerUI.satisfaction4.gameObject.SetActive(false);
		customerUI.ToggleWait(false);
		customerUI.ToggleStar(false);
		customerUI.ToggleAllergyAttack(false);
	}
}
