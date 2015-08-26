using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MutableDataStartObject{

	public Dictionary <string,bool> shouldAnimate;

	public MutableDataStartObject(){
		shouldAnimate = new Dictionary<string, bool>();
	}
}
