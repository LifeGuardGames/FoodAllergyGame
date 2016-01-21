using UnityEngine;
using System.Collections;
using System;

public class BehavWaitingInLine : CustomerComponent {


	public BehavWaitingInLine() {

	}

	public override void Reason() {
		RestaurantManager.Instance.lineCount--;
		RestaurantManager.Instance.CustomerLineSelectHighlightOff();
		Waiter.Instance.CurrentLineCustomer = null;
		AudioManager.Instance.PlayClip("CustomerSeated");
		//sitting down
		self.transform.SetParent(RestaurantManager.Instance.GetTable(self.tableNum).Seat);
		self.transform.localPosition = Vector3.zero;
		if(RestaurantManager.Instance.GetTable(self.tableNum).tableType == Table.TableType.VIP) {    // TODO connect this with logic rather than number
			RestaurantManager.Instance.VIPUses++;
			self.customerUI = self.gameObject.transform.GetComponentInParent<CustomerUIController>();
			//customerUI.satisfaction1.gameObject.SetActive(true);
			//customerUI.satisfaction2.gameObject.SetActive(true);
			//customerUI.satisfaction3.gameObject.SetActive(true);
			self.timer /= RestaurantManager.Instance.GetTable(self.tableNum).VIPMultiplier;
			self.SetSatisfaction(4);
			AudioManager.Instance.PlayClip("VIPEnter");

		}
		// begin reading menu
		self.customerAnim.SetReadingMenu();

		self.StartCoroutine("ReadMenu");
		// TODO-SOUND Reading menu here
		self.StopCoroutine("SatisfactionTimer");

		// Table connection setup
		self.gameObject.GetComponentInParent<Table>().currentCustomerID = self.customerID;
		self.GetComponent<BoxCollider>().enabled = false;
		RestaurantManager.Instance.lineController.FillInLine();
		var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[0]);
		CustomerComponent read = (CustomerComponent)Activator.CreateInstance(type);
		read.self = self;
		read.Act();
		//BehavReadingMenu read = new BehavReadingMenu(self);
		self.currBehav = read;
		read = null;
	}

	public override void Act() {
		self.state = CustomerStates.InLine;
	}
}
