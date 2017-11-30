using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_script : MonoBehaviour {

	#region External Variables
	public PlayerController_script m_player;
	#endregion

	#region Internal Variables
	private bool m_isPlayer = false;
	private const float SLOWTIMEDURATION = 0.1f;
	#endregion

	#region Standard Methods
	private void Awake() {
		if (m_player != null) {
			m_isPlayer = true;
		}
	}

	private void OnTriggerEnter(Collider other) {
		Debug.Log("shield Collision");
		if (m_isPlayer) {
			if (m_player.m_isBlocking) {
				Weapon_script enemy = other.GetComponent<Weapon_script>();
				if (enemy != null) {
					Debug.Log("here");
					if (enemy.m_enemy.m_isAttacking) {
						Debug.Log("enemy parried");
						m_player.m_health.TakeDamage(Random.Range(enemy.m_minDamage, enemy.m_maxDamage));
						enemy.m_enemy.GetParried();
						enemy.m_enemy.m_isAttacking = false;
						StartCoroutine(SlowTime());	
						m_player.m_animController.SetImpact();
					}
				}
			}
		}
	}
	#endregion

	#region Custom Methods
	IEnumerator SlowTime() {
		Time.timeScale = 0.1f;
		for (float t = 0; t < SLOWTIMEDURATION; t += Time.deltaTime) {
			yield return null;
		}
		Time.timeScale = 1;
	}
	#endregion

}
