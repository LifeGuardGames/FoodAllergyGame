using System.Collections;

public class MutableDataTutorial{
	public bool IsComicViewed { get; set; }
	public bool IsTutorial1Done { get; set; }
	public bool IsMedicTut1Done { get; set; }
	public bool IsMedicTut2Done { get; set; }
	public bool IsDecoFingerTutDone { get;set; }
	public bool IsMenuPlanningFingerTutDone { get; set; }	// Finger dragging tutorial for menuplanning
	public bool IsEpiPenGameTutorialDone { get; set; }
	public int MissedMedic { get; set; }

	public MutableDataTutorial(){
		IsComicViewed = false;
		IsTutorial1Done = DataManager.Instance.IsDebug ? Constants.GetDebugConstant<bool>("IsTut1Done") : false;
		IsMedicTut1Done = false;
		IsMedicTut2Done = false;
		IsDecoFingerTutDone = DataManager.Instance.IsDebug ? Constants.GetDebugConstant<bool>("IsDecoFingerTutDone") : false;
		IsMenuPlanningFingerTutDone = false;
		IsEpiPenGameTutorialDone = false;
    }
}
