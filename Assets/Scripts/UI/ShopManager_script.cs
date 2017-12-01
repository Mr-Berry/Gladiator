using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager_script : MonoBehaviour {

	#region External Variables
		public GameObject[] m_weapons;
		public GameObject[] m_armor;
		public GameObject[] m_shield;
	#endregion

	#region Internal Variables
	#endregion

	#region Standard Methods
	#endregion

	#region Custom Methods
	private bool CanAfford(int cost) {
		bool retval = false;
		if (GameManager.m_playerGold >= cost) {
			retval = true;
		}
		return retval;
	}
	#endregion

}
