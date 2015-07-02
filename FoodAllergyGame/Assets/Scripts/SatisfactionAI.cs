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
		score = _satisfaction *5;
		satisfaction += _satisfaction;
		SatisfactionLevel(score);
		return _satisfaction* 3.476f;
	}
	// Calcualtes the satisfaction level
	private void SatisfactionLevel(int _satisfaction){
		// using a random number to add some unpredictability into the system
		satisfactionLevel += (_satisfaction/numOfCustomers) + UnityEngine.Random.Range(0,4);
	}

	public float GetSatisfaction(){
		return satisfactionLevel;
	}

	public int GetScore(){
		return score;
	}

	public float AvgSatifaction(){
		return satisfaction/ numOfCustomers;
	}

	public void AddCustomer(){
		numOfCustomers++;
	}

	public int GetMissingCustomers(){
		return missingCustomers;
	}
}
