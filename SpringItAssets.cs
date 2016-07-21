using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

public class SpringItAssets : IStoreAssets {

	public int GetVersion() {
		return 0;
	}
	
	public VirtualCurrency[] GetCurrencies() {
		return new VirtualCurrency[]{};
	}

	public VirtualGood[] GetGoods() {
		return new VirtualGood[] {TWENTYROBOTS_GOOD,UNLIMITED_ROBOTS};
	}
	
	public VirtualCurrencyPack[] GetCurrencyPacks() {
		return new VirtualCurrencyPack[] {};
	}

	public VirtualCategory[] GetCategories() {
		return new VirtualCategory[]{GENERAL_CATEGORY};
	}

	public const string UNLIMITED_ROBOTS_PRODUCT_ID = "unlimited_robots";
	public const string TWENTYROBOTS_PRODUCT_ID = "twenty_robots";

	public static LifetimeVG UNLIMITED_ROBOTS = new LifetimeVG(
		"Unlimited Robots",
		"Unlock Unlimited Robots for Spring It",
		"unlimited_robots",
		new PurchaseWithMarket(UNLIMITED_ROBOTS_PRODUCT_ID, 5.00)
	 );

	public static VirtualGood TWENTYROBOTS_GOOD = new SingleUseVG(
		"Twenty Robots",                                       		// name
		"Player get 5 per day,  this allows them to buy extra", // description
		"twenty_robots",                                       		// item id
		new PurchaseWithMarket(TWENTYROBOTS_PRODUCT_ID, 1.00)
	); // the way this virtual good is purchased

	public static VirtualCategory GENERAL_CATEGORY = new VirtualCategory(
		"General", new List<string>(new string[] { TWENTYROBOTS_PRODUCT_ID, UNLIMITED_ROBOTS_PRODUCT_ID })
		);
}
