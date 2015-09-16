using UnityEngine;
using System.Collections;

public class MicrowaveLoader : DecoLoader {
	protected override void DecoInit(){
		isDebugEnableDeco = Constants.GetDebugConstant<bool>("MicrowaveOn");
		debugDecoID = Constants.GetDebugConstant<string>("MicrowaveID");
	}
}
