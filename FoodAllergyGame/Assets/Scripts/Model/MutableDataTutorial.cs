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
	public bool IsSpeDecoTutDone { get; set; }
	public bool IsTrashCanTutDone { get; set; }
	public bool IsPlayAreaTutDone { get; set; }
	public bool IsFlyThruTutDone { get; set; }
	public bool IsBlackOutTutDone { get; set; }
	public bool IsGossiperTutDone { get; set; }

	public MutableDataTutorial(){
		IsComicViewed = false;
		IsTutorial1Done = DataManager.Instance.IsDebug ? Constants.GetDebugConstant<bool>("IsTut1Done") : false;
		IsMedicTut1Done = false;
		IsMedicTut2Done = false;
		IsDecoFingerTutDone = DataManager.Instance.IsDebug ? Constants.GetDebugConstant<bool>("IsDecoFingerTutDone") : false;
		IsMenuPlanningFingerTutDone = false;
		IsEpiPenGameTutorialDone = false;
		IsSpeDecoTutDone = false;
		IsTrashCanTutDone = false;
		IsPlayAreaTutDone = false;
		IsFlyThruTutDone = false;
		IsBlackOutTutDone = false;
		IsGossiperTutDone = false;
    }
}
