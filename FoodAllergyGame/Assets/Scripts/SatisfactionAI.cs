using System.Collections;

public class SatisfactionAI{
	private int modifiedSatisfaction;
	public int Score{
		get{ return modifiedSatisfaction; }
	}

	private int numOfCustomers;
	public int NumOfCustomers{
		get{ return numOfCustomers; }
	}

	private float difficultyLevel;
	public float DifficultyLevel{
		get{ return difficultyLevel; }
	}

	private int missingCustomers;
	public int MissingCustomers{
		get{ return missingCustomers; }
	}

	private float totalSatisfaction;

	// Calculates the money given to the player once a customer leaves
	public float CalculateCheck(int incomingSatisfaction){
		if(incomingSatisfaction <= 0){
			missingCustomers++;
		}
		modifiedSatisfaction = incomingSatisfaction * 5;
		if(incomingSatisfaction < 0)
		{
			incomingSatisfaction = 0;
		}

		CalculateDifficultyLevel(modifiedSatisfaction);

		totalSatisfaction += incomingSatisfaction;
		return incomingSatisfaction * 3.476f;
	}

	// Calculates the difficulty level
	private void CalculateDifficultyLevel(int modifiedSatisfaction){
		// using a random number to add some unpredictability into the system
		difficultyLevel += (modifiedSatisfaction/numOfCustomers) + UnityEngine.Random.Range(0,4);
	}

	public float AvgSatifaction(){
		return totalSatisfaction / numOfCustomers;
	}

	public void AddCustomer(){
		numOfCustomers++;
	}
}
