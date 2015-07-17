using System.Collections;

public class MutableDataCash {
	public int TotalCash {get; set;}
	public int CurrentCash {get; set;}
	public int Tier {get;set;}

	public MutableDataCash(){
		TotalCash = 0;
		CurrentCash = 0;
	}
}
