using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PurchasingManager : Singleton<PurchasingManager>, IStoreListener {

	private static IStoreController m_StoreController;			// Reference to the Purchasing system.
	private static IExtensionProvider m_StoreExtensionProvider;	// Reference to store-specific Purchasing subsystems.
	
	// Product identifiers for all products capable of being purchased: "convenience" general identifiers for use with Purchasing, and their store-specific identifier counterparts 
	// for use with and outside of Unity Purchasing. Define store-specific identifiers also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)
	private static string kProductIDPro = "com.lifeguardgames.foodallergy.iap.pro";				// General handle for the non-consumable product.
	private static string kProductNameApplePro = "com.LifeGuardGames.FoodAllergy.IAP.Pro";		// Apple App Store identifier for the non-consumable product.
	private static string kProductNameGooglePlayPro = "com.lifeguardgames.foodallergyandroid.iap.pro";  // Google Play Store identifier for the non-consumable product.

	private static string kProductIDSpookyTable = "com.lifeguardgames.foodallergy.iap.spooky.table";
	private static string kProductIDAppleSpookyTable = "com.LifeGuardGames.FoodAllergy.IAP.Spooky.Table";
	private static string kProductIDGooglePlaySpookyTable = "com.lifeguardgames.foodallergyandroid.iap.spooky.table";

	private static string kProductIDSpookyKitchen = "com.lifeguardgames.foodallergy.iap.spooky.kitchen";
	private static string kProductIDAppleSpookyKitchen = "com.LifeGuardGames.FoodAllergy.IAP.Spooky.Kitchen";
	private static string kProductIDGooglePlaySpookyKitchen = "com.lifeguardgames.foodallergyandroid.iap.spooky.kitchen";

	private static string kProductIDSpookyFloor = "com.lifeguardgames.foodallergy.iap.spooky.floor";
	private static string kProductIDAppleSpookyFloor = "com.LifeGuardGames.FoodAllergy.IAP.Spooky.Floor";
	private static string kProductIDGooglePlaySpookyFloor = "com.lifeguardgames.foodallergyandroid.iap.spooky.floor";

	private static string kProductIDSloppyTable = "com.lifeguardgames.foodallergy.iap.sloppy.table";
	private static string kProductIDAppleSloppyTable = "com.LifeGuardGames.FoodAllergy.IAP.Sloppy.Table";
	private static string kProductIDGooglePlaySloppyTable = "com.lifeguardgames.foodallergyandroid.iap.sloppy.table";

	private static string kProductIDSloppyKitchen = "com.lifeguardgames.foodallergy.iap.sloppy.kitchen";
	private static string kProductIDAppleSloppyKitchen = "com.LifeGuardGames.FoodAllergy.IAP.Sloppy.Kitchen";
	private static string kProductIDGooglePlaySloppyKitchen = "com.lifeguardgames.foodallergyandroid.iap.sloppy.kitchen";

	private static string kProductIDSloppyFloor = "com.lifeguardgames.foodallergy.iap.sloppy.floor";
	private static string kProductIDAppleSloppyFloor = "com.LifeGuardGames.FoodAllergy.IAP.Sloppy.Floor";
	private static string kProductIDGooglePlaySloppyFloor = "com.lifeguardgames.foodallergyandroid.iap.sloppy.floor";

	private static string kProductIDSeaTable = "com.lifeguardgames.foodallergy.iap.sea.table";
	private static string kProductIDAppleSeaTable = "com.LifeGuardGames.FoodAllergy.IAP.Sea.Table";
	private static string kProductIDGooglePlaySeaTable = "com.lifeguardgames.foodallergyandroid.iap.sea.table";

	private static string kProductIDSeaKitchen = "com.lifeguardgames.foodallergy.iap.sea.kitchen";
	private static string kProductIDAppleSeaKitchen = "com.LifeGuardGames.FoodAllergy.IAP.Sea.Kitchen";
	private static string kProductIDGooglePlaySeaKitchen = "com.lifeguardgames.foodallergyandroid.iap.sea.kitchen";

	private static string kProductIDSeaFloor = "com.lifeguardgames.foodallergy.iap.sea.floor";
	private static string kProductIDAppleSeaFloor = "com.LifeGuardGames.FoodAllergy.IAP.Sea.Floor";
	private static string kProductIDGooglePlaySeaFloor = "com.lifeguardgames.foodallergyandroid.iap.sea.floor";

	private static string kProductIDPlushieTable = "com.lifeguardgames.foodallergy.iap.plushie.table";
	private static string kProductIDApplePlushieTable = "com.LifeGuardGames.FoodAllergy.IAP.Plushie.Table";
	private static string kProductIDGooglePlayPlushieTable = "com.lifeguardgames.foodallergyandroid.iap.plushie.table";

	private static string kProductIDPlushieKitchen = "com.lifeguardgames.foodallergy.iap.plushie.kitchen";
	private static string kProductIDApplePlushieKitchen = "com.LifeGuardGames.FoodAllergy.IAP.Plushie.Kitchen";
	private static string kProductIDGooglePlayPlushieKitchen = "com.lifeguardgames.foodallergyandroid.iap.plushie.kitchen";

	private static string kProductIDPlushieFloor = "com.lifeguardgames.foodallergy.iap.plushie.floor";
	private static string kProductIDApplePlushieFloor = "com.LifeGuardGames.FoodAllergy.IAP.Plushie.Floor";
	private static string kProductIDGooglePlayPlushieFloor = "com.lifeguardgames.foodallergyandroid.iap.plushie.floor";

	public List<string> productList;
	public string currentDecoId;


	void Start() {
		productList = new List<string>();
		productList.Add(kProductIDPlushieFloor);
		productList.Add(kProductIDPlushieKitchen);
		productList.Add(kProductIDPlushieTable);
		productList.Add(kProductIDSeaFloor);
		productList.Add(kProductIDSeaKitchen);
		productList.Add(kProductIDSeaTable);
		productList.Add(kProductIDSloppyFloor);
		productList.Add(kProductIDSloppyKitchen);
		productList.Add(kProductIDSloppyTable);
		productList.Add(kProductIDSpookyFloor);
		productList.Add(kProductIDSpookyKitchen);
		productList.Add(kProductIDSpookyTable);
		// If we haven't set up the Unity Purchasing reference
		if(m_StoreController == null) {
			// Begin to configure our connection to Purchasing
			InitializePurchasing();
		}
	}

	public void InitializePurchasing() {
		// If we have already connected to Purchasing ...
		if(IsInitialized()) {
			// ... we are done here.
			return;
		}

		// Create a builder, first passing in a suite of Unity provided stores.
		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
		// Add a product to sell / restore by way of its identifier, associating the general identifier with its store-specific identifiers.
		builder.AddProduct(kProductIDPro, ProductType.NonConsumable, new IDs() { { kProductNameApplePro, AppleAppStore.Name }, { kProductNameGooglePlayPro, GooglePlay.Name }, });// And finish adding the subscription product.
		builder.AddProduct(kProductIDSpookyTable, ProductType.NonConsumable, new IDs() { { kProductIDAppleSpookyTable, AppleAppStore.Name }, { kProductIDGooglePlaySpookyTable, GooglePlay.Name }, });
		builder.AddProduct(kProductIDSpookyKitchen, ProductType.NonConsumable, new IDs() { { kProductIDAppleSpookyKitchen, AppleAppStore.Name }, { kProductIDGooglePlaySpookyKitchen, GooglePlay.Name }, });
		builder.AddProduct(kProductIDSpookyFloor, ProductType.NonConsumable, new IDs() { { kProductIDAppleSpookyFloor, AppleAppStore.Name }, { kProductIDGooglePlaySpookyFloor, GooglePlay.Name }, });
		builder.AddProduct(kProductIDSloppyTable, ProductType.NonConsumable, new IDs() { { kProductIDAppleSloppyTable, AppleAppStore.Name }, { kProductIDGooglePlaySloppyTable, GooglePlay.Name }, });
		builder.AddProduct(kProductIDSloppyKitchen, ProductType.NonConsumable, new IDs() { { kProductIDAppleSloppyKitchen, AppleAppStore.Name }, { kProductIDGooglePlaySloppyKitchen, GooglePlay.Name }, });
		builder.AddProduct(kProductIDSloppyFloor, ProductType.NonConsumable, new IDs() { { kProductIDAppleSloppyFloor, AppleAppStore.Name }, { kProductIDGooglePlaySloppyFloor, GooglePlay.Name }, });
		builder.AddProduct(kProductIDSeaTable, ProductType.NonConsumable, new IDs() { { kProductIDAppleSeaTable, AppleAppStore.Name }, { kProductIDGooglePlaySeaTable, GooglePlay.Name }, });
		builder.AddProduct(kProductIDSeaKitchen, ProductType.NonConsumable, new IDs() { { kProductIDAppleSeaKitchen, AppleAppStore.Name }, { kProductIDGooglePlaySeaKitchen, GooglePlay.Name }, });
		builder.AddProduct(kProductIDSeaFloor, ProductType.NonConsumable, new IDs() { { kProductIDAppleSeaFloor, AppleAppStore.Name }, { kProductIDGooglePlaySeaFloor, GooglePlay.Name }, });
		builder.AddProduct(kProductIDPlushieTable, ProductType.NonConsumable, new IDs() { { kProductIDApplePlushieTable, AppleAppStore.Name }, { kProductIDGooglePlayPlushieTable, GooglePlay.Name }, });
		builder.AddProduct(kProductIDPlushieKitchen, ProductType.NonConsumable, new IDs() { { kProductIDApplePlushieKitchen, AppleAppStore.Name }, { kProductIDGooglePlayPlushieKitchen, GooglePlay.Name }, });
		builder.AddProduct(kProductIDPlushieFloor, ProductType.NonConsumable, new IDs() { { kProductIDApplePlushieFloor, AppleAppStore.Name }, { kProductIDGooglePlayPlushieFloor, GooglePlay.Name }, });
		UnityPurchasing.Initialize(this, builder);
	}
	
	private bool IsInitialized() {
		// Only say we are initialized if both the Purchasing references are set.
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}

	// --- IStoreListener
	public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
		// Purchasing has succeeded initializing. Collect our Purchasing references.
		Debug.Log("OnInitialized: PASS");

		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;
		// Save the localized price to DataManager
		DataManager.Instance.BaseDecoPriceStringAux = m_StoreController.products.WithID(kProductIDSpookyTable).metadata.localizedPriceString;
		//make sure datamanger knows what has been purchased
		foreach(Product prod in m_StoreController.products.all) {
			if(prod.hasReceipt) {
				currentDecoId = DataLoaderDecoItem.GetDecoIDFromProdID(prod.definition.id);
				DataManager.Instance.GameData.Decoration.BoughtDeco.Add(currentDecoId, "");
			}
		}
		
	}

	public void OnInitializeFailed(InitializationFailureReason error) {
		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}

	/*
	public void BuyNonConsumable() {
		// Buy the non-consumable product using its general identifier. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
		BuyProductID(kProductIDPro);
	}
	*/

	public void BuyStardustSet(int setNumber) {
		switch(setNumber) {
			case 1:
				BuyProductID(kProductIDAppleSpookyTable);
				break;
			case 2:
				BuyProductID(kProductIDAppleSpookyKitchen);
				break;
			case 3:
				BuyProductID(kProductIDAppleSpookyFloor);
				break;
		}
	}
	
	void BuyProductID(string productId) {
		// If the stores throw an unexpected exception, use try..catch to protect my logic here.
		try {
			// If Purchasing has been initialized ...
			if(IsInitialized()) {
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
					PurchaseFailedUI();
				}
			}
			// Otherwise ...
			else {
				// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
				Debug.Log("BuyProductID FAIL. Not initialized.");
				PurchaseFailedUI();
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
		// If Purchasing has not yet been set up ...
		if(!IsInitialized()) {
			// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
			Debug.Log("RestorePurchases FAIL. Not initialized.");
			PurchaseFailedUI();
			return;
		}

		// If we are running on an Apple device ... 
		if(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer) {
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
			PurchaseFailedUI();
		}
	}
	
	// NOTE: Android calls this on game start automatically if you have a purchase to redeem
	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) {
		// Or ... a non-consumable product has been purchased by this user.
		// TODO Commenting this out, old feature
		/*if(String.Equals(args.purchasedProduct.definition.id, kProductIDPro, StringComparison.Ordinal)) {
			Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
			Amplitude.Instance.logRevenue(Double.Parse(DataManager.Instance.PriceStringAux));
			StartManager.Instance.UnlockMoreCratesAndShowUI();
        }
		else*/
		if(productList.Contains(args.purchasedProduct.definition.id)) {
			Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
			string price = DataManager.Instance.BaseDecoPriceStringAux;
			price = price.Trim('$');
            Amplitude.Instance.logRevenue(double.Parse(price));
			//DataManager.Instance.GameData.DayTracker.IAPCurrency++;
			if(DecoManager.Instance != null) {
				currentDecoId = DataLoaderDecoItem.GetDecoIDFromProdID(args.purchasedProduct.definition.id);
				DecoManager.Instance.PurchaseCompleteDeco(currentDecoId);
			}
		}
		// Or ... an unknown product has been purchased by this user. Fill in additional products here.
		else {
			Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
			PurchaseFailedUI();
		}
		// Return a flag indicating wither this product has completely been received, 
		// or if the application needs to be reminded of this purchase at next app launch.
		// Is useful when saving purchased products to the cloud, and when that save is delayed.
		return PurchaseProcessingResult.Complete;
	}
	
	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {
		// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing this reason with the user.
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
		PurchaseFailedUI();
    }

	// Get the purchase failed UI depending on what scene it is
	public void PurchaseFailedUI() {
		if(SceneManager.GetActiveScene().name == SceneUtils.START) {
			//StartManager.Instance.PurchaseFailed();
		}
		else if(SceneManager.GetActiveScene().name == SceneUtils.DECO) {
			DecoManager.Instance.PurchaseFailed();
		}
	}

	public void BuyIapDeco(string id) {
		currentDecoId = id;
		switch(id) {
			case "TableSpooky":
				BuyProductID(kProductIDSpookyTable);
				break;
			case "KitchenSpooky":
				BuyProductID(kProductIDSpookyKitchen);
				break;
			case "FloorSpooky":
				BuyProductID(kProductIDSpookyFloor);
				break;
			case "TableSloppy":
				BuyProductID(kProductIDSloppyTable);
				break;
			case "KitchenSloppy":
				BuyProductID(kProductIDSloppyKitchen);
				break;
			case "FloorSloppy":
				BuyProductID(kProductIDSloppyFloor);
				break;
			case "TableSea":
				BuyProductID(kProductIDSeaTable);
				break;
			case "KitchenSea":
				BuyProductID(kProductIDSeaKitchen);
				break;
			case "FloorSea":
				BuyProductID(kProductIDSeaFloor);
				break;
			case "TablePlushie":
				BuyProductID(kProductIDPlushieTable);
				break;
			case "KitchenPlushie":
				BuyProductID(kProductIDPlushieKitchen);
				break;
			case "FloorPlushie":
				BuyProductID(kProductIDPlushieFloor);
				break;

		}
	}


	// Localized to native price, cached in datamanager
	/*
	public string GetPriceButtonText() {
		return DataManager.Instance.PriceStringAux == "" ? "Buy" : DataManager.Instance.PriceStringAux;
    }

	public void BuyPro() {
		BuyProductID(kProductIDPro);
	}
	*/
}