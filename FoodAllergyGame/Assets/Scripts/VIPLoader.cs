public class VIPLoader : DecoLoader {
	public static int baseSortingOrder = 150;

	protected override void DecoInit(){
		isDebugEnableDeco = Constants.GetDebugConstant<bool>("VIPOn");
		debugDecoID = Constants.GetDebugConstant<string>("VIPID");
		decoType = DecoTypes.VIP;
	}
}
