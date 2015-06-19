using UnityEngine;
using System.Collections;

public class CustomerAnimController : MonoBehaviour {
	public Animator customerAnimator;
	
	public void SetSatisfaction(int satisfaction){
		customerAnimator.SetInteger("Satisfaction", satisfaction);
	}

	public void SetEating(bool isEating){
		customerAnimator.SetBool("IsEating", isEating);
	}

	public void SetReadingMenu(bool isReadingMenu){
		customerAnimator.SetBool("IsReadingMenu", isReadingMenu);
	}
}
