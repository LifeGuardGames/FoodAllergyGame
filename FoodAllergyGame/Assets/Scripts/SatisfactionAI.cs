using System.Collections;

public class SatisfactionAI{
	private int score;
	private int numOfCustomers;
	private float satisfactionLevel;
	private int missingCustomers;
	private float satisfaction;

	// Calculates the money given to the player once a customer leaves
	public float CalculateCheck(int _satisfaction){
		if(_satisfaction <= 0){
			missingCustomers++;
		}
		UnityEngine.Debug.Log (_satisfaction);
		score = _satisfaction *5;
		satisfaction += _satisfaction;
		SatisfactionLevel(score);
		return _satisfaction* 3.476f;
	}
	// Calcualtes the satisfaction level
	private void SatisfactionLevel(int _satisfaction){
		satisfactionLevel += (_satisfaction/numOfCustomers) + UnityEngine.Random.Range(0,4);
	}

	public float GetSatisfaction(){
		return satisfactionLevel;
	}

	public int GetScore(){
		return score;
	}
	public float AvgSatifaction(){
		UnityEngine.Debug.Log(satisfaction);
		UnityEngine.Debug.Log(numOfCustomers);
		return satisfaction/ numOfCustomers;
	}

	public void AddCustomer(){
		numOfCustomers++;
	}

	public int GetMissingCustomers(){
		return missingCustomers;
	}
}
