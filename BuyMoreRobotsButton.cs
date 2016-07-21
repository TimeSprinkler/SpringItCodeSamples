using UnityEngine;
using System.Collections;
using Soomla.Store;

public class BuyMoreRobotsButton : MonoBehaviour {

	private VirtualGood vg = SpringItAssets.TWENTYROBOTS_GOOD;

	public void BuyTwentyRobots(){
		StoreInventory.BuyItem(vg.ItemId);
	}
}
