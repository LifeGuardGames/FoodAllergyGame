using UnityEngine;
using System.Collections;

public class FlyThruLoader : DecoLoader {
	protected override void DecoInit(){
		isDebugEnableDeco = Constants.GetDebugConstant<bool>("FlyThruOn");
		debugDecoID = Constants.GetDebugConstant<string>("FlyThruID");
		decoType = DecoTypes.FlyThru;
	}
}
