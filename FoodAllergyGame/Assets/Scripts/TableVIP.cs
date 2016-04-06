﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class TableVIP : Table {
	public Animator VIPTableAnimator;
	public SpriteRenderer topSprite;
	public SpriteRenderer baseSprite;
	public Canvas statusCanvas;

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

			SetBaseSortingOrder(VIPLoader.baseSortingOrder);
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

		TableActiveToggle(false);
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

	// Toggles the sprite states of the table, initally from BehavWaitingInLine
	public void TableActiveToggle(bool isActive) {
		VIPTableAnimator.SetBool("InUse", isActive);
		if(isActive) {
			AudioManager.Instance.PlayClip("VIPEnter");
		}
	}

	public override void SetBaseSortingOrder(int _baseSortingOrder) {
		baseSortingOrder = _baseSortingOrder;
		baseSprite.sortingOrder = _baseSortingOrder + 1;
		topSprite.sortingOrder = _baseSortingOrder + 2;
        uiCanvas.sortingOrder = _baseSortingOrder + 3;
		statusCanvas.sortingOrder = _baseSortingOrder + 4;
        tableHighlight.sortingOrder = _baseSortingOrder + 5;
	}
}
