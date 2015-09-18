using UnityEngine;
using System.Collections;

public class KitchenLoader : DecoLoader {
	protected override void DecoInit(){
		isDebugEnableDeco = true;	// Always on
		debugDecoID = Constants.GetDebugConstant<string>("KitchenID");
		decoType = DecoTypes.Kitchen;
	}
}
