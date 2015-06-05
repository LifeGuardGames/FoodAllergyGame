using System.Collections;

public class SatisfactionAI{
	private int score;
	private int numOfCustomers;
	private float satisfactionLevel;

	// calculates the money given to the player once a customer leaves
	public float CalculateCheck(int satisfaction){
		numOfCustomers++;
		score = satisfaction *5;
		SatisfactionLevel(score);
		return satisfaction* 3.476f;
	}
	//calcualtes the satisfaction level
	private void SatisfactionLevel(int satisfaction){
		satisfactionLevel += (satisfaction/numOfCustomers) + UnityEngine.Random.Range(0,4);
	}

	public float GetSatisfaction(){
		return satisfactionLevel;
	}

	public int GetScore(){
		return score;
	}
}
