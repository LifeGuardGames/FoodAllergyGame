using UnityEngine;
using System.Collections;

public class BathroomLoader : DecoLoader {
	protected override void DecoInit(){
		isDebugEnableDeco = Constants.GetDebugConstant<bool>("BathroomOn");
		debugDecoID = Constants.GetDebugConstant<string>("BathroomID");
	}
}
