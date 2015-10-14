using System.Collections;

public class MutableDataTutorial{

	public bool IsTutorial1Done {get; set;}
	public bool IsTutorial3Done {get; set;}		// Tutorial 2 is part of tutorial 1 flow, so skip
	public bool IsMedicTut1Done {get; set;}
	public bool IsMedicTut2Done {get; set;}
	public bool IsDecoTutDone {get;set;}
	public bool IsSpecialDecoTutDone {get;set;}

	public MutableDataTutorial(){
		IsTutorial1Done = DataManager.Instance.IsDebug ? Constants.GetDebugConstant<bool>("IsTut1Done") : false;
		IsTutorial3Done = DataManager.Instance.IsDebug ? Constants.GetDebugConstant<bool>("IsTut3Done") : false;
		IsMedicTut1Done = false;
		IsMedicTut2Done = false;
		IsDecoTutDone =  false;
		IsSpecialDecoTutDone = false;
	}
}
