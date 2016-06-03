using UnityEngine;
using UnityEngine.SceneManagement;

public class TableVIP : Table {
	public Animator VIPTableAnimator;
	public SpriteRenderer topSprite;
	public SpriteRenderer baseSprite;
	public Canvas statusCanvas;
	public ParticleSystem activeParticle;
	public CustomerUIController customerUI;

	void Start () {
		Init();
	}

	public override void Init() {
		base.Init();
		if(SceneManager.GetActiveScene().name == SceneUtils.RESTAURANT) {
			node = Pathfinding.Instance.NodeVIP;
			customerUI.ToggleWait(false);
			customerUI.ToggleStar(false);
			customerUI.ToggleAllergyAttack(false);
			this.GetComponent<BoxCollider>().enabled = true;

			SetBaseSortingOrder(VIPLoader.baseSortingOrder);
		}
	}

	public override void CustomerLeaving() {
		base.CustomerLeaving();
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
		CustomerUIController customerUI = this.GetComponentInChildren<CustomerUIController>();
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
			if(activeParticle != null) {
				activeParticle.Play();
			}
		}
		else {
			if(activeParticle != null) {
				activeParticle.Stop();
			}
		}
		
	}

	public override void SetBaseSortingOrder(int _baseSortingOrder) {
		baseSortingOrder = _baseSortingOrder;
		baseSprite.sortingOrder = _baseSortingOrder + 1;
		topSprite.sortingOrder = _baseSortingOrder + 2;

		if(activeParticle != null) {
			Renderer[] renderers = activeParticle.GetComponentsInChildren<Renderer>(true);
			foreach(Renderer r in renderers) {
				if(r.gameObject.GetComponent<ParticleSystem>() != null) {
					r.sortingOrder = _baseSortingOrder + 3;
                }
			}
		}

        uiCanvas.sortingOrder = _baseSortingOrder + 3;
		statusCanvas.sortingOrder = _baseSortingOrder + 100;
        tableHighlight.sortingOrder = _baseSortingOrder + 5;
	}
}
