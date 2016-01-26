using System.Collections;

public class SatisfactionAI{
	private float modifiedSatisfaction;
	public float Score{
		get{ return modifiedSatisfaction; }
	}

	private int numOfCustomers;
	public int NumOfCustomers{
		get{ return numOfCustomers; }
	}

	private float difficultyLevel = 10;
	public float DifficultyLevel{
		get{ return difficultyLevel; }
	}

	private int missingCustomers;
	public int MissingCustomers{
		get{ return missingCustomers; }
	}

	private float totalSatisfaction;

	// Calculates the money given to the player once a customer leaves
	public int CalculateBill(int customerSatisfaction, int priceMultiplier, float time, bool isModifiesDifficulty){
		if(customerSatisfaction <= 0) {
			missingCustomers++;
		}
		if(isModifiesDifficulty) {
			modifiedSatisfaction = time;
			if(customerSatisfaction < 0) {
				customerSatisfaction = 0;
			}

			CalculateDifficultyLevel(modifiedSatisfaction);

			totalSatisfaction += customerSatisfaction;
			return (int)(customerSatisfaction * 3.476f * priceMultiplier);
	 	}
		return 0;
	}

	// Calculates the difficulty level
	private void CalculateDifficultyLevel(float time){
		// using a random number to add some unpredictability into the system
		if(RestaurantManager.Instance.isTutorial){
//			difficultyLevel = 10;
		}
		else{
			difficultyLevel = (difficultyLevel + (time-4.0f))/2 ;
		}
	}

	public float AvgSatisfaction(){
		if(totalSatisfaction / numOfCustomers > 3) {
			return 3;
		}
		else {
			return totalSatisfaction / numOfCustomers;
		}
	}

	public void AddCustomer(){
		numOfCustomers++;
	}
}
