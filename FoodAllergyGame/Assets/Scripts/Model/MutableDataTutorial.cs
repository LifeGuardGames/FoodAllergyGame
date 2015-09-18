using System.Collections;

public class MutableDataTutorial{

	public bool IsTutorial1Done {get; set;}
	public bool IsTutorial3Done {get; set;}		// Tutorial 2 is part of tutorial 1 flow, so skip
	public bool IsMedicTuT1Done {get; set;}
	public bool IsMedicTuT2Done {get; set;}
	public bool IsDecoTuTDone {get;set;}

	public MutableDataTutorial(){
		IsTutorial1Done = Constants.GetDebugConstant<bool>("IsTut1Done");
		IsTutorial3Done = Constants.GetDebugConstant<bool>("IsTut3Done");
		IsMedicTuT1Done = false;
		IsMedicTuT2Done = false;
		IsDecoTuTDone =  false;
	}
}
