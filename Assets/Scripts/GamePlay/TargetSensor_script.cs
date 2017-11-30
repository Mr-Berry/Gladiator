using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSensor_script : MonoBehaviour {

	#region External Variables
	public bool m_hasTarget = false;
	public Transform m_target = null;
	#endregion

	#region Standard Methods
	
	private void OnTriggerEnter(Collider other) {
		if (m_target == null) {
			CheckIfDead(other);
		} else {
			GetCloserEnemy(other);
		}
		m_hasTarget = true;
	}

	private void OnTriggerExit(Collider other) {
		if (other.transform == m_target) {
			m_target = null;
			m_hasTarget = false;
		}
	}

	#endregion

	#region Custom Methods

	private void CheckIfDead(Collider other) {
		Health_script m_health = other.GetComponent<Health_script>();
		if (m_health != null) {
			if (!m_health.m_isDead) {
				m_target = other.transform;
			}
		}
	}

	private void GetCloserEnemy(Collider newTarget) {
		if (GetDistance(newTarget.transform.position) < GetDistance(m_target.position)) {
			CheckIfDead(newTarget);
		}
	}

	private float GetDistance(Vector3 location) {
		return (location - transform.position).magnitude;
	}
	#endregion

}
