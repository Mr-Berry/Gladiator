using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_script : MonoBehaviour {

	#region External Variables
	public short m_maxHP = 100;
	public bool m_isPlayer = false;
	//lower armor factor means lower damage taken
	public float m_armorFactor = 1;
	public float m_shieldFactor = 0.5f;
	public bool m_isDead = false;
	public Slider m_healthBar;
	public Image m_healthBarFill;
	public AudioClip[] m_deathClips;
	#endregion

	#region  Internal Variables
	private short m_currentHP;
	private Animation_script m_animation;
	private PlayerController_script m_player;
	private EnemyMovement_script m_enemy;
	private AudioSource m_audio;
	#endregion

	#region Standard Methods
	private void Awake() {
		m_audio = GetComponent<AudioSource>();
		if (m_isPlayer) {
			m_animation = GetComponent<Animation_script>();
			m_player = GetComponent<PlayerController_script>();
		} else {
			m_enemy = GetComponent<EnemyMovement_script>();
		}
		m_currentHP = m_maxHP;
		m_isDead = false;
	}
	#endregion

	#region Custom Methods
	private void Die() {
		m_audio.clip = m_deathClips[Random.Range(0,m_deathClips.Length)];
		m_audio.Play();
		m_isDead = true;
		if (m_isPlayer) {
			m_animation.SetDead();
			GameManager.Instance.EndGame();
		} else {
			m_enemy.Die();
		}
	}
	public void TakeDamage(int damage) {
		if (m_isPlayer) {
			StartCoroutine(SetHealthBar((float)m_currentHP/(float)m_maxHP));
			StartCoroutine(Flash());
			if (m_player.m_isBlocking) {
				m_currentHP -= (short)((damage * m_armorFactor)*m_shieldFactor);
			} else {
				m_currentHP -= (short)(damage * m_armorFactor);
			}

		} else {
			m_currentHP -= (short)(damage * m_armorFactor );			
		}
		if (m_currentHP <= 0) {
			m_currentHP = 0;
			Die();
		}
		Debug.Log(name + "'s HP = " + m_currentHP);
	}

	public void ResetHealth() {
		m_isDead = false;
		m_currentHP = m_maxHP;
	}

	IEnumerator Flash() {
		Color original = m_healthBarFill.color;
		m_healthBarFill.color = new Color32(255,00,00,255);
		for (float t = 0; t < 0.1; t += Time.deltaTime) {
			yield return null;
		}
		m_healthBarFill.color = original;
	}

	IEnumerator SetHealthBar(float newHealth) {
		while (m_healthBar.value > newHealth) {
			m_healthBar.value -= 0.005f;
			yield return null;
		}
	}
	#endregion

}
