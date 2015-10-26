using System.Collections;
using System.Collections.Generic;

public class MutableDataDecoration{
	public bool IsFirstTimeEntrance;
	public Dictionary<DecoTypes, string> ActiveDeco;
	public Dictionary<string, string> BoughtDeco;
	public List<DecoTypes> DecoTut;
	public List<string> DecoTutQueue;

	public MutableDataDecoration(){
		DecoTutQueue = new List<string>();
		IsFirstTimeEntrance = true;

		DecoTut = new List<DecoTypes>();
		DecoTut.Add(DecoTypes.Floor);
		DecoTut.Add(DecoTypes.Bathroom);
		DecoTut.Add(DecoTypes.Kitchen);
		DecoTut.Add(DecoTypes.Table);

		BoughtDeco = new Dictionary<string, string>();
		// Add default decos
		BoughtDeco.Add("Floor00", "");
		BoughtDeco.Add("Table00", "");
		BoughtDeco.Add("Kitchen00", "");

		ActiveDeco = new Dictionary<DecoTypes, string>();
		// Add default decos
		ActiveDeco.Add(DecoTypes.Floor, "Floor00");
		ActiveDeco.Add(DecoTypes.Table, "Table00");
		ActiveDeco.Add(DecoTypes.Kitchen, "Kitchen00");
	}
}
