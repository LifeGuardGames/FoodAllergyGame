using System.Collections;
using System.Collections.Generic;

public class MutableDataDecoration{
	public Dictionary<DecoTypes, string> ActiveDeco;
	public Dictionary <string, string> BoughtDeco;

	public MutableDataDecoration(){
		BoughtDeco = new Dictionary<string, string>();
		ActiveDeco = new Dictionary<DecoTypes, string>();
		ActiveDeco.Add(DecoTypes.Floor, "Floor00");
		ActiveDeco.Add(DecoTypes.Table, "Table00");
		ActiveDeco.Add(DecoTypes.Kitchen, "Kitchen00");
		ActiveDeco.Add(DecoTypes.FlyThru, "FlyThru00");
		ActiveDeco.Add(DecoTypes.PlayArea, "PlayArea00");
		ActiveDeco.Add(DecoTypes.Microwave, "Microwave00");
		ActiveDeco.Add(DecoTypes.VIP, "VIP00");
	}
}
