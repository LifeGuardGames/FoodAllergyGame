using System.Collections;
using System.Collections.Generic;

public class MutableDataDecoration{
	public Dictionary<DecoTypes, string> ActiveDeco;
	public Dictionary <string, string> BoughtDeco;

	public MutableDataDecoration(){
		BoughtDeco = new Dictionary<string, string>();
		// Add default decos
		BoughtDeco.Add("Floor00", "");
		BoughtDeco.Add("Table00", "");
		BoughtDeco.Add("Kitchen00", "");
		BoughtDeco.Add("PlayArea00", "");

		ActiveDeco = new Dictionary<DecoTypes, string>();
		// Add default decos
		ActiveDeco.Add(DecoTypes.Floor, "Floor00");
		ActiveDeco.Add(DecoTypes.Table, "Table00");
		ActiveDeco.Add(DecoTypes.Kitchen, "Kitchen00");

		// TODO Turn this off in live
		ActiveDeco.Add(DecoTypes.FlyThru, "FlyThru00");
		ActiveDeco.Add(DecoTypes.PlayArea, "PlayArea00");
		ActiveDeco.Add(DecoTypes.Microwave, "Microwave00");
		ActiveDeco.Add(DecoTypes.VIP, "VIP00");
	}
}
