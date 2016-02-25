using UnityEngine;
using System.Collections;
using System;

public class BehavBlackoutNotifyLeave : Behav {

	public BehavBlackoutNotifyLeave() {

	}

	public override void Reason() {
	}

	public override void Act() {
		if(self.satisfaction == 0 || self.isAnnoyed) {
			self.DestroyOrder();
			CustomerAnimationCotrollerBlackOut animBlackout = self.customerAnim as CustomerAnimationCotrollerBlackOut;
			animBlackout.BlackOut();
		}
		else {
			// Otherwise leave normally
			var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[10]);
			Behav leave = (Behav)Activator.CreateInstance(type);
			leave.self = self;
			leave.Act();
			leave = null;
		}
	}
}
