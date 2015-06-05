using UnityEngine;
using System.Collections;

public class SatisfactionManager : MonoBehaviour {

	public int score;
	public int numOfCustomers;

	// takes a satisfaction and transfers it into score
	public void SatisfactionToScore(int Satisfaction){
		numOfCustomers++;
		score += Satisfaction * 100 ;
	}


}
