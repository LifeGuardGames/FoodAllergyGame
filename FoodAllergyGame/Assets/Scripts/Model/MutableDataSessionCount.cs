using System;
using System.Collections;

public class MutableDataSessionCount {
	public DateTime start;
	public int sessionCount;

	public MutableDataSessionCount() {
		start = new DateTime();
		sessionCount = 0;
	}
}
