using System.Collections;

public class ChallengeAI  {
	private int score;
	public int Score {
		get { return score; }
	}

	private int numOfCustomers;
	public int NumOfCustomers {
		get { return numOfCustomers; }
	}


	private int missingCustomers;
	public int MissingCustomers {
		get { return missingCustomers; }
	}


	private int negativeCash;
	public int NegativeCash {
		get { return negativeCash; }
	}

	private float totalSatisfaction;

	public void AddNegativeCash(int amount) {
		negativeCash += amount;
	}

	// Calculates the money given to the player once a customer leaves
	public int CalculateBill(int customerSatisfaction, int priceMultiplier) {
		if(customerSatisfaction <= 0) {
			missingCustomers++;
		}
			if(customerSatisfaction < 0) {
				customerSatisfaction = 0;
			}

			totalSatisfaction += customerSatisfaction;
			
			return (int)(customerSatisfaction * 3.476f * priceMultiplier);
	}


	public float AvgSatisfaction() {
		if(totalSatisfaction / numOfCustomers > 3) {
			return 3;
		}
		else {
			return totalSatisfaction / numOfCustomers;
		}
	}

	public int ScoreIt() {
		score = RestaurantManagerChallenge.Instance.DayEarnedCash -(( 50 * missingCustomers)+ negativeCash);
		return score;
	}

	public void AddCustomer() {
		numOfCustomers++;
	}

}
