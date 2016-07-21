using UnityEngine;
using System.Collections;
using Soomla.Store;

public class RestoreStore : MonoBehaviour {

	public void ReStore(){
		SoomlaStore.RestoreTransactions();
	}
}
