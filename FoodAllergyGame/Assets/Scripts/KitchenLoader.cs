public class KitchenLoader : DecoLoader {
	public static int baseSortingOrder = 120;

	protected override void DecoInit(){
		isDebugEnableDeco = true;	// Always on
		debugDecoID = Constants.GetDebugConstant<string>("KitchenID");
		decoType = DecoTypes.Kitchen;
	}
}
