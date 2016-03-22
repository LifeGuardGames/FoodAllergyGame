using System.Collections;
using System.Collections.Generic;

public class MutableDataDecoration{
	public bool IsFirstTimeEntrance;
	public Dictionary<DecoTypes, string> ActiveDeco;
	public Dictionary<string, string> BoughtDeco;
	public List<string> NewDeco;

	public MutableDataDecoration(){
		IsFirstTimeEntrance = true;

		// Add default decos
		BoughtDeco = new Dictionary<string, string>();
		BoughtDeco.Add("FloorBasicAqua", "");
		BoughtDeco.Add("TableBasicAqua", "");
		BoughtDeco.Add("KitchenBasicAqua", "");

		// Add default decos
		ActiveDeco = new Dictionary<DecoTypes, string>();
		ActiveDeco.Add(DecoTypes.Floor, "FloorBasicAqua");
		ActiveDeco.Add(DecoTypes.Table, "TableBasicAqua");
		ActiveDeco.Add(DecoTypes.Kitchen, "KitchenBasicAqua");

		NewDeco = new List<string>();
	}
}
