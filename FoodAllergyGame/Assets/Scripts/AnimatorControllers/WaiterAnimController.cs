using UnityEngine;
using System.Collections;

public class WaiterAnimController : MonoBehaviour {
	public Animator waiterAnimator;

	public void SetMoving(bool isMoving){
		waiterAnimator.SetBool("IsMoving", isMoving);
	}
}
