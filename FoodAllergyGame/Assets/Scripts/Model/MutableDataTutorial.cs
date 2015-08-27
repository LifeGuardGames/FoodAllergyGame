using System.Collections;

public class MutableDataTutorial{

	public bool IsTutorial1Done {get; set;}
	public bool IsTutorial2Done {get; set;}
	public bool IsMedicTuT1Done{get; set;}
	public bool IsMedicTuT2Done{get; set;}

	public MutableDataTutorial(){
		Constants.GetDebugConstant<bool>("IsTut1Done");
		Constants.GetDebugConstant<bool>("IsTut2Done");
		IsMedicTuT1Done = false;
		IsMedicTuT2Done = false;
	}
}
