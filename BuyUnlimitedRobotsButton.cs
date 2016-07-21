using UnityEngine;
using System.Collections;
using Soomla.Store;

public class BuyUnlimitedRobotsButton : MonoBehaviour {

	//private VirtualGood vg = SpringItAssets.UNLIMITED_ROBOTS;

	public void BuyUnlimitedRobots(){
		StoreInventory.BuyItem ("unlimited_robots");
	}
}
