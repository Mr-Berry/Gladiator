using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_script : MonoBehaviour {

	#region External Variables
	public int m_minDamage = 1;
	public int m_maxDamage = 2;
	public bool m_isPlayerOwned = true;
	public PlayerController_script m_player;
	public EnemyMovement_script m_enemy;
	#endregion

	#region Internal Variables
	public List<Transform> m_targets = new List<Transform>(); 
	#endregion

	#region Standard Methods

	private void OnTriggerEnter(Collider other) {
		Health_script opponentHealth = other.GetComponent<Health_script>();
		if (!m_targets.Contains(other.transform)) {
			m_targets.Add(other.transform);
			if (opponentHealth != null) {
				if (m_isPlayerOwned) {
					if (m_player.m_isAttacking) {
						EnemyMovement_script opponentMovement = other.GetComponent<EnemyMovement_script>();
						opponentHealth.TakeDamage(Random.Range(m_minDamage,m_maxDamage));
						opponentMovement.GetStunned();
					}
				} else {
					if (m_enemy.m_isAttacking) {
						opponentHealth.TakeDamage(Random.Range(m_minDamage,m_maxDamage));
					}
				}
			}
		}
	}

	#endregion

	#region Custom Methods

	public void ResetTargets() {
		m_targets.Clear();
	}

#endregion

}
