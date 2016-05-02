﻿using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class PurchasingManager : Singleton<PurchasingManager>, IStoreListener {

	private static IStoreController m_StoreController;			// Reference to the Purchasing system.
	private static IExtensionProvider m_StoreExtensionProvider;	// Reference to store-specific Purchasing subsystems.

	public ProductPageUIController productPageUIController;		// Window to purchase ads
	public IAPStatusPageUIController statusPageUIController;	// Status window of purchase

	// Product identifiers for all products capable of being purchased: "convenience" general identifiers for use with Purchasing, and their store-specific identifier counterparts 
	// for use with and outside of Unity Purchasing. Define store-specific identifiers also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)
	private static string kProductIDPro = "com.lifeguardgames.foodallergy.iap.pro";				// General handle for the non-consumable product.
	private static string kProductNameApplePro = "com.LifeGuardGames.FoodAllergy.IAP.Pro";		// Apple App Store identifier for the non-consumable product.
	private static string kProductNameGooglePlayPro = "com.lifeguardgames.foodallergy.iap.pro";	// Google Play Store identifier for the non-consumable product.

	void Start() {
		// If we haven't set up the Unity Purchasing reference
		if(m_StoreController == null) {
			// Begin to configure our connection to Purchasing
			InitializePurchasing();
		}
	}

	public void InitializePurchasing() {
		Debug.Log("====INITIALIZING");
		// If we have already connected to Purchasing ...
		if(IsInitialized()) {
			Debug.Log("====ALREADY INITIALIZED");
			// ... we are done here.
			return;
		}

		// Create a builder, first passing in a suite of Unity provided stores.
		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

		// Add a product to sell / restore by way of its identifier, associating the general identifier with its store-specific identifiers.
		builder.AddProduct(kProductIDPro, ProductType.NonConsumable, new IDs() { { kProductNameApplePro, AppleAppStore.Name }, { kProductNameGooglePlayPro, GooglePlay.Name }, });// And finish adding the subscription product.
		UnityPurchasing.Initialize(this, builder);
	}
	
	private bool IsInitialized() {
		// Only say we are initialized if both the Purchasing references are set.
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}
	
	public void BuyNonConsumable() {
		// Buy the non-consumable product using its general identifier. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
		BuyProductID(kProductIDPro);
	}
	
	void BuyProductID(string productId) {
		Debug.Log("====BUYING PRODUCT PREPARE");
		// If the stores throw an unexpected exception, use try..catch to protect my logic here.
		try {
			// If Purchasing has been initialized ...
			if(IsInitialized()) {
				Debug.Log("====BUYING PRODUCT");
				// ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
				Product product = m_StoreController.products.WithID(productId);

				// If the look up found a product for this device's store and that product is ready to be sold ... 
				if(product != null && product.availableToPurchase) {
					Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
					m_StoreController.InitiatePurchase(product);
				}
				// Otherwise ...
				else {
					// ... report the product look-up failure situation  
					Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
				}
			}
			// Otherwise ...
			else {
				// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
				Debug.Log("BuyProductID FAIL. Not initialized.");
			}
		}
		// Complete the unexpected exception handling ...
		catch(Exception e) {
			// ... by reporting any unexpected exception for later diagnosis.
			Debug.Log("BuyProductID: FAIL. Exception during purchase. " + e);
		}
	}
	
	// Restore purchases previously made by this customer. Some platforms automatically restore purchases. Apple currently requires explicit purchase restoration for IAP.
	public void RestorePurchases() {
		Debug.Log("====RESTORING PURCHASE");
		// If Purchasing has not yet been set up ...
		if(!IsInitialized()) {
			// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
			Debug.Log("RestorePurchases FAIL. Not initialized.");
			return;
		}

		// If we are running on an Apple device ... 
		if(Application.platform == RuntimePlatform.IPhonePlayer ||
			Application.platform == RuntimePlatform.OSXPlayer) {
			// ... begin restoring purchases
			Debug.Log("RestorePurchases started ...");

			// Fetch the Apple store-specific subsystem.
			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
			// Begin the asynchronous process of restoring purchases. Expect a confirmation response in the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
			apple.RestoreTransactions((result) => {
				// The first phase of restoration. If no more responses are received on ProcessPurchase then no purchases are available to be restored.
				Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
			});
		}
		// Otherwise ...
		else {
			// We are not running on an Apple device. No work is necessary to restore purchases.
			Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}
	
	//  
	// --- IStoreListener
	//
	public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
		Debug.Log("====INITIALIZED");
		// Purchasing has succeeded initializing. Collect our Purchasing references.
		Debug.Log("OnInitialized: PASS");

		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;

		// Save the localized price to DataManager
		DataManager.Instance.PriceStringAux = m_StoreController.products.WithID(kProductIDPro).metadata.localizedPriceString;
	}
	
	public void OnInitializeFailed(InitializationFailureReason error) {
		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}
	
	// NOTE: Android calls this on game start automatically if you have a purchase to redeem
	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) {
		Debug.Log("====PROCESSING PURCHASE");
		// Or ... a non-consumable product has been purchased by this user.
		if(String.Equals(args.purchasedProduct.definition.id, kProductIDPro, StringComparison.Ordinal)) {
			Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

			UnlockMoreCrates();
			productPageUIController.HidePanel();
			Debug.Log("====SHOWING STATUS TRUE");
			statusPageUIController.ShowPanel(true);
		}
		// Or ... an unknown product has been purchased by this user. Fill in additional products here.
		else {
			Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));

			productPageUIController.HidePanel();
			Debug.Log("====SHOWING STATUS FALSE");
			statusPageUIController.ShowPanel(false);
		}
		Debug.Log("====DONE PROCESSING PURCHASE");

		// Return a flag indicating wither this product has completely been received, 
		// or if the application needs to be reminded of this purchase at next app launch.
		// Is useful when saving purchased products to the cloud, and when that save is delayed.
		return PurchaseProcessingResult.Complete;
	}
	
	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {
		// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing this reason with the user.
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));

		productPageUIController.HidePanel();
		statusPageUIController.ShowPanel(false);
	}

	// Called from beacon gameobject
	public void ShowProductPage() {
		StartManager.Instance.ChallengeMenuEntranceUIController.ToggleClickable(false);
		StartManager.Instance.dinerEntranceUIController.ToggleClickable(false);
		StartManager.Instance.shopEntranceUIController.ToggleClickable(false);
		productPageUIController.ShowPanel();
	}

	// Localized to native price, cached in datamanager
	public string GetPriceButtonText() {
		return DataManager.Instance.PriceStringAux == "" ? "Buy" : DataManager.Instance.PriceStringAux;
    }

	public void BuyPro() {
		BuyProductID(kProductIDPro);
	}

	// Called on success or restore
	private void UnlockMoreCrates() {
		Debug.Log("====UNLOCKING MORE CRATES");
		DataManager.Instance.GameData.DayTracker.IsMoreCrates = true;

		// Do other things here, remove UI - if it is the correct scene
		if(LoadLevelManager.Instance.GetCurrentSceneName() == SceneUtils.START) {
			// Destroy this for now
			Destroy(StartManager.Instance.beaconNode.transform.GetChild(0).gameObject);
		}
	}
}

