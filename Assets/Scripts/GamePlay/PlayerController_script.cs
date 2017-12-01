using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController_script : MonoBehaviour {

	#region External Variables
	public float m_Speed;
	public bool m_canRotate = true;
	public bool m_canMove = true;
	public bool m_isAttacking = false;
	public bool m_isBlocking = false;
	public bool m_gameStarted = false;
	public TargetSensor_script m_sensor;
	public Weapon_script[] m_weapons;
	public GameObject[] m_shields;
	[HideInInspector]
	public Animation_script m_animController;
	[HideInInspector]
	public Weapon_script m_weapon;
	[HideInInspector]
	public Health_script m_health;
	public Armor[] m_armors;
	[Serializable]
	public class Armor {
		public GameObject[] m_armorPieces;
	}
	#endregion

	#region Internal Variables
	private Transform m_origPos;
	private float m_blockSpeed;
	private float m_runSpeed;
	private int m_currentWeapon = 0;
	private int m_currentShield = 0;
	private int m_currentArmor = 0;
	private bool m_comboAttack = false;
	#endregion

	#region Standard Methods

	private void Awake() {
//		PlayerPrefs.DeleteAll();
		m_origPos = transform;
		m_animController = GetComponent<Animation_script>();
		m_health = GetComponent<Health_script>();
		m_runSpeed = m_Speed;
		m_blockSpeed = m_Speed/2f;
		InitWeapon();
		InitShield();
		InitArmor();
	}

	private void Update() {
		if (!m_health.m_isDead && m_gameStarted) {
			if (m_canRotate) {
				RotateToFaceMouse();
			}
			HandleInput();
		}
	}
	#endregion

	#region Custom Methods
	private void RotateToFaceMouse() {
		if (m_canRotate) {
			Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			float length = (transform.position - Camera.main.transform.position).magnitude;
			Vector3 direction = mouseRay.origin + mouseRay.direction * length;
			direction.y = transform.position.y;
			transform.LookAt(direction);
		}
	}

	private void HandleInput() {
		if (m_canMove) {
			MovementInput();
		}
		if (!m_isBlocking) {
			AttackInput();
		}
		if (!m_isAttacking) {
			DefendInput();
		} else {
			if (m_sensor.m_target != null) {
				Vector3 target = m_sensor.m_target.position;
				target.y = transform.position.y;
				transform.LookAt(target);
			}			
		}
	}

	private void MovementInput() {
		float moveX = Input.GetAxis("Horizontal");
		float moveZ = Input.GetAxis("Vertical");
		Vector3 movement = (new Vector3(moveX,0,moveZ)) * m_Speed * Time.deltaTime;
		movement = transform.InverseTransformVector(movement);
		transform.Translate(movement);
		m_animController.SetSpeed(movement.z);
		m_animController.SetDirection(movement.x);
		if (movement.magnitude > 0.05) {
			m_animController.SetWalking(true);
		} else {
			m_animController.SetWalking(false);
		}
	}

	private void AttackInput() {
		if (Input.GetAxis("Attack") != 0 && !m_comboAttack) {
			m_animController.SetAttacking();
			m_isAttacking = true;
			m_comboAttack = true;
			m_canRotate = false;
		} else if (Input.GetAxis("Attack") == 0 && m_comboAttack) {
			m_comboAttack = false;
		}
	}

	private void DefendInput() {
		if (Input.GetAxis("Block") != 0) {
			m_animController.SetBlocking(true);
			m_isBlocking = true;
			m_Speed = m_blockSpeed;
		} else if (Input.GetAxis("Block") == 0) {
			m_animController.SetBlocking(false);
			m_isBlocking = false;
			m_Speed = m_runSpeed;
		}
	}

	private void InitWeapon() {
		for (int i = 0; i < m_weapons.Length; i++) {
			m_weapons[i].gameObject.SetActive(false);
		}
		if (PlayerPrefs.HasKey("Weapon")) {
		 	m_currentWeapon = PlayerPrefs.GetInt("Weapon");
		} else {
		 	PlayerPrefs.SetInt("Weapon",m_currentWeapon);
		}
		m_weapons[m_currentWeapon].gameObject.SetActive(true);
		m_weapon = m_weapons[m_currentWeapon];
	}

	private void InitShield() {
		for (int i = 0; i < m_shields.Length; i++) {
			m_shields[i].SetActive(false);
		}
		if (PlayerPrefs.HasKey("Shield")) {
		 	m_currentShield = PlayerPrefs.GetInt("Shield");
		} else {
		 	PlayerPrefs.SetInt("Shield",m_currentShield);
		}
		m_shields[m_currentShield].SetActive(true);
		m_health.m_shieldFactor -= m_currentShield*0.1f;
	}

	private void InitArmor() {
		if (PlayerPrefs.HasKey("Armor")) {
		 	m_currentArmor = PlayerPrefs.GetInt("Armor");
		} else {
		 	PlayerPrefs.SetInt("Armor",m_currentArmor);
		}	
		for (int i = 0; i < m_currentArmor; i++) {
			for (int j = 0; j < m_armors[i].m_armorPieces.Length; j++) {
				m_armors[i].m_armorPieces[j].SetActive(true);
			}
		}
		m_health.m_armorFactor -= m_currentArmor*0.1f;
	}

	public void ResetPosition() {
		transform.position = m_origPos.position;
		transform.rotation = m_origPos.rotation;
		GetComponent<Animator>().Rebind();
	}

	#endregion

}
