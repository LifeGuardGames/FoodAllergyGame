using System.Collections;
using System.Collections.Generic;

public class MutableDataDecoration{
	public bool IsFirstTimeEntrance;
	public Dictionary<DecoTypes, string> ActiveDeco;
	public Dictionary<string, string> BoughtDeco;

	public MutableDataDecoration(){
		IsFirstTimeEntrance = true;

		// Add default decos
		BoughtDeco = new Dictionary<string, string>();
		BoughtDeco.Add("Floor00", "");
		BoughtDeco.Add("Table00", "");
		BoughtDeco.Add("Kitchen00", "");

		// Add default decos
		ActiveDeco = new Dictionary<DecoTypes, string>();
		ActiveDeco.Add(DecoTypes.Floor, "Floor00");
		ActiveDeco.Add(DecoTypes.Table, "Table00");
		ActiveDeco.Add(DecoTypes.Kitchen, "Kitchen00");
	}
}
