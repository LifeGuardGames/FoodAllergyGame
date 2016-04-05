public class FlyThruLoader : DecoLoader {
	public static int baseSortingOrder = 90;

	protected override void DecoInit(){
		isDebugEnableDeco = Constants.GetDebugConstant<bool>("FlyThruOn");
		debugDecoID = Constants.GetDebugConstant<string>("FlyThruID");
		decoType = DecoTypes.FlyThru;
	}
}
