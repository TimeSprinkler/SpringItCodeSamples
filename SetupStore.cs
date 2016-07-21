using UnityEngine;
using System.Collections;
using Soomla.Store;
using System.Collections.Generic;
using System;
using Soomla;


public class SetupStore : MonoBehaviour {

	void Awake(){

	}

	void Start(){
		StoreEvents.OnSoomlaStoreInitialized += SoomlaStoreInitialized;
		StoreEvents.OnCurrencyBalanceChanged += CurrencyBalanceChanged;
		StoreEvents.OnUnexpectedStoreError += UnexpectedStoreError;
		StoreEvents.OnItemPurchased += CheckItemPurchased;
		//StoreEvents.OnMarketPurchase += CheckMarketItemPurchased;

		SoomlaStore.Initialize(new SpringItAssets());
	}

	void OnDestroy(){
		StoreEvents.OnSoomlaStoreInitialized -= SoomlaStoreInitialized;
		StoreEvents.OnCurrencyBalanceChanged -= CurrencyBalanceChanged;
		StoreEvents.OnUnexpectedStoreError -= UnexpectedStoreError;
		StoreEvents.OnItemPurchased -= CheckItemPurchased;
		//StoreEvents.OnMarketPurchase -= CheckMarketItemPurchased;
	}

	void SoomlaStoreInitialized(){

	}

	void CurrencyBalanceChanged(VirtualCurrency virtualCurrency, int balance, int amountAdded){
		Debug.LogWarning ("This should not be called because we do not have virtual currency");
	}

	void UnexpectedStoreError(int errorCode){
		SoomlaUtils.LogError ("ExampleEventHandler", "error with code: " + errorCode);
	}

	void CheckItemPurchased(PurchasableVirtualItem pvi, string payload){

		if (pvi.ID == SpringItAssets.TWENTYROBOTS_PRODUCT_ID) {
			PlayerData.instance.mNumberOfRobots += 20;
		}else if (pvi.ID == SpringItAssets.UNLIMITED_ROBOTS_PRODUCT_ID) {
				
			PlayerData.instance.mUnlimitedRobotsUnlocked = true;
			PlayerData.instance.mNumberOfRobots = 999;				
		}
	}

	//void CheckMarketItemPurchased(PurchasableVirtualItem pvi, string payload, Dictionary<string, string> extra){



	//}
}
