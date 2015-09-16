using UnityEngine;
using System.Collections;

public class VIPLoader : DecoLoader {
	protected override void DecoInit(){
		isDebugEnableDeco = Constants.GetDebugConstant<bool>("VIPOn");
		debugDecoID = Constants.GetDebugConstant<string>("VIPID");
	}
}
