using System.Collections;
using System.Collections.Generic;

public class MutableDataDecoration{
	public Dictionary<DecoTypes, string> currDiner;
	public Dictionary <string, string> BoughtDeco;

	public MutableDataDecoration(){
		currDiner = new Dictionary<DecoTypes, string>();
		BoughtDeco = new Dictionary<string, string>();
		currDiner.Add(DecoTypes.Floor, "Floor00");
		currDiner.Add(DecoTypes.Table, "Table00");
		currDiner.Add(DecoTypes.Kitchen, "Kitchen00");
		currDiner.Add(DecoTypes.FlyThru, "FlyThru00");
		currDiner.Add(DecoTypes.PlayArea, "PlayArea00");
		currDiner.Add(DecoTypes.Microwave, "Microwave00");
		currDiner.Add(DecoTypes.VIP, "VIP00");
	}
}
