using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager_script : MonoBehaviour {

	#region External Variables
	public GameObject[] m_weapons;
	public GameObject[] m_armor;
	public GameObject[] m_shield;
	public Text[] m_prices;
	public Text m_goldText;
	#endregion

	#region Internal Variables
	private int m_armorCost = 100;
	private int m_weaponCost = 150;
	private int m_shieldCost = 50;
	private int m_currentWeapon = 0;
	private int m_currentShield = 0;
	private int m_currentArmor = 0;
	#endregion

	#region Standard Methods
	private void Start() {
		InitPlayerPrefs();
		InitImages();
		m_goldText.text = GameManager.m_playerGold.ToString();
	}
	#endregion

	#region Custom Methods
	private bool CanAfford(int cost) {
		bool retval = false;
		if (GameManager.m_playerGold >= cost) {
			retval = true;
		}
		return retval;
	}

	private void InitImages() {
		InitArmor();
		InitShield();
		InitWeapon();
	}

	private void InitPlayerPrefs() {
		if (PlayerPrefs.HasKey("Weapon")) {
			m_currentWeapon = PlayerPrefs.GetInt("Weapon");
		}
		if (PlayerPrefs.HasKey("Shield")) {
			m_currentShield = PlayerPrefs.GetInt("Shield");
		}
		if (PlayerPrefs.HasKey("Armor")) {
			m_currentArmor = PlayerPrefs.GetInt("Armor");
		}
	}

	private void InitArmor() {
		if (m_currentArmor < m_armor.Length) {
			m_armor[m_currentArmor].SetActive(true);
		} else {
			m_armor[m_currentArmor-1].SetActive(true);
		}
		m_armorCost += m_currentArmor * m_armorCost;
		m_prices[1].text = m_armorCost.ToString();
	}

	private void InitWeapon() {
		if (m_currentWeapon < m_weapons.Length) {
			m_weapons[m_currentWeapon].SetActive(true);
		} else {
			m_weapons[m_currentWeapon-1].SetActive(true);
		}
		m_weaponCost += m_currentWeapon * m_weaponCost;
		m_prices[0].text = m_weaponCost.ToString();	
	}

	private void InitShield() {
		if (m_currentShield < m_shield.Length) {
			m_shield[m_currentShield].SetActive(true);
		} else {
			m_shield[m_currentShield-1].SetActive(true);
		}
		m_shieldCost += m_currentShield * m_shieldCost;
		m_prices[2].text = m_shieldCost.ToString();	
	}

	public void PurchaseArmor() {
		if (CanAfford(m_armorCost)) {
			if (m_currentArmor < 4) {
				GameManager.m_playerGold -= m_armorCost;
				m_armor[m_currentArmor].SetActive(false);				
				m_currentArmor++;
				if (m_currentArmor < 4) {
					m_armor[m_currentArmor].SetActive(true);
				}
				m_armorCost += m_currentArmor * m_armorCost;
				m_prices[1].text = m_armorCost.ToString();
				PlayerPrefs.SetInt("Armor",m_currentArmor);	
				PlayerPrefs.SetInt("Gold", GameManager.m_playerGold);	
				m_goldText.text = GameManager.m_playerGold.ToString();				
			}
		}
	}

	public void PurchaseWeapon()  {
		if (CanAfford(m_weaponCost)) {
			if (m_currentWeapon < 3) {
				GameManager.m_playerGold -= m_weaponCost;
				m_weapons[m_currentWeapon].SetActive(false);				
				m_currentWeapon++;
				if (m_currentWeapon < 3) {
					m_weapons[m_currentWeapon].SetActive(true);
				}
				m_weaponCost += m_currentWeapon * m_weaponCost;
				m_prices[0].text = m_weaponCost.ToString();
				PlayerPrefs.SetInt("Weapon",m_currentWeapon);	
				PlayerPrefs.SetInt("Gold", GameManager.m_playerGold);	
				m_goldText.text = GameManager.m_playerGold.ToString();			
			}
		}
	}

	public void PurchaseShield() {
		if (CanAfford(m_shieldCost)) {
			if (m_currentShield < 3) {
				GameManager.m_playerGold -= m_shieldCost;
				m_shield[m_currentShield].SetActive(false);				
				m_currentShield++;
				if (m_currentShield < 3) {
					m_shield[m_currentShield].SetActive(true);
				}
				m_shieldCost += m_currentShield * m_shieldCost;
				m_prices[2].text = m_shieldCost.ToString();
				PlayerPrefs.SetInt("Shield",m_currentShield);
				PlayerPrefs.SetInt("Gold", GameManager.m_playerGold);	
				m_goldText.text = GameManager.m_playerGold.ToString();				
			}
		}
	}
	
	#endregion

}
