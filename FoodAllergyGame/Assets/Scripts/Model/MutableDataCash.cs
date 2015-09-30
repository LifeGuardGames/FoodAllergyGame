using System.Collections;

public class MutableDataCash {
	public int TotalCash {get; set;}
	public int CurrentCash {get; set;}

	public MutableDataCash(){
		TotalCash = 1000;
		CurrentCash = 1000;
	}

	public void SaveCash(int dayCashNet, int dayCashRevenue){
		CurrentCash += dayCashNet;
		TotalCash += dayCashRevenue;
	}
}
