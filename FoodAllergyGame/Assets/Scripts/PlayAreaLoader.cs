using UnityEngine;
using System.Collections;

public class PlayAreaLoader : DecoLoader {
	protected override void DecoInit(){
		isDebugEnableDeco = Constants.GetDebugConstant<bool>("PlayAreaOn");
		debugDecoID = Constants.GetDebugConstant<string>("PlayAreaID");
		decoType = DecoTypes.PlayArea;
	}
}
