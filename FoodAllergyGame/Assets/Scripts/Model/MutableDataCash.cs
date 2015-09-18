using System.Collections;

public class MutableDataCash {
	public int TotalCash {get; set;}
	public int CurrentCash {get; set;}

	public MutableDataCash(){
		TotalCash = 0;
		CurrentCash = 0;
	}

	public void SaveCash(int dayCashNet, int dayCashRevenue){
		CurrentCash += dayCashNet;
		TotalCash += dayCashRevenue;
	}
}
