using UnityEngine;
using System;

public class BehavWaitingInLine : Behav {
	public BehavWaitingInLine() {

	}

	public override void Reason() {
		RestaurantManager.Instance.lineCount--;
		RestaurantManager.Instance.CustomerLineSelectHighlightOff();
		Waiter.Instance.CurrentLineCustomer = null;
		AudioManager.Instance.PlayClip("CustomerSeated");

		//sitting down
		Table selfTable = RestaurantManager.Instance.GetTable(self.tableNum);
        self.transform.SetParent(selfTable.Seat);
		self.SetBaseSortingOrder(selfTable.BaseSortingOrder);
		self.transform.localPosition = Vector3.zero;
		if(selfTable.tableType == Table.TableType.VIP) {
			RestaurantManager.Instance.VIPUses++;
			self.customerUI = RestaurantManager.Instance.GetTable(self.tableNum).GetComponent<TableVIP>().customerUI;
			self.timer /= selfTable.VIPMultiplier;
			self.SetSatisfaction(4);
			TableVIP vipTable = (TableVIP)selfTable;
			vipTable.TableActiveToggle(true);
		}
		// begin reading menu
		self.customerAnim.SetReadingMenu();

		// TODO-SOUND Reading menu here
		self.StopCoroutine("SatisfactionTimer");

		// Table connection setup
		self.gameObject.GetComponentInParent<Table>().currentCustomerID = self.customerID;
		self.GetComponent<BoxCollider>().enabled = false;
		RestaurantManager.Instance.lineController.FillInLine();
		var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[1]);
		Behav read = (Behav)Activator.CreateInstance(type);
		read.self = self;
		self.currBehav = read;
		read.Act();
		//BehavReadingMenu read = new BehavReadingMenu(self);
		read = null;
	}

	public override void Act() {
		self.state = CustomerStates.InLine;
	}
}
