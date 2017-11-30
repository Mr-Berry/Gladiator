using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera_script : MonoBehaviour {

	#region External Variables
	public Transform m_waypointsContainer;
	public float m_speed = 5;
	#endregion

	#region Internal Variables
	private Transform[] m_waypoints;
	private Rigidbody m_rb;
	private Vector3 m_targetRotation;
	private int m_currentWP = 0;
	private const int LENGTHERROR = 10;
	#endregion

	#region Standard Methods

	private void Awake() {
		m_targetRotation = transform.rotation.eulerAngles;
		GetWaypoints();
	}

	private void Update() {
		UpdateCurrentWP();
		Move();
		RotateToWP();
	}

	#endregion

	#region Custom Methods
	private void GetWaypoints() {
		Transform[] potentialWaypoints = m_waypointsContainer.GetComponentsInChildren<Transform>();
		m_waypoints = new Transform[ (potentialWaypoints.Length - 1) ];
		for (int i = 1; i < potentialWaypoints.Length; ++i ) {
 			m_waypoints[i - 1] = potentialWaypoints[i];
		}
	}

	private void UpdateCurrentWP() {
		if (Mathf.Abs((transform.position - m_waypoints[m_currentWP].position).magnitude) < LENGTHERROR) {
			if (m_currentWP < m_waypoints.Length - 1) {
				m_currentWP++;
			} else {
				m_currentWP = 0;
			}
			m_targetRotation = m_waypoints[m_currentWP].rotation.eulerAngles;
		}
	}

	private void Move() {
		Vector3 direction = (m_waypoints[m_currentWP].position - transform.position).normalized;
		transform.Translate(transform.InverseTransformVector(direction) * m_speed * Time.deltaTime);
	}

	private void RotateToWP() {
		if (transform.rotation.eulerAngles != m_targetRotation) {
			if (Mathf.Abs((transform.rotation.eulerAngles - m_targetRotation).magnitude) < 2) {
				transform.rotation = Quaternion.Euler(m_targetRotation);
			} else {
				Vector3 rotateBy = m_targetRotation - transform.rotation.eulerAngles;
				rotateBy.x = Mathf.Clamp(rotateBy.x,-0.5f,0.5f);
				rotateBy.y = Mathf.Clamp(rotateBy.y,-0.5f,0.5f);
				rotateBy.z = Mathf.Clamp(rotateBy.z,-0.5f,0.5f);
				transform.Rotate(rotateBy);
			}
		}
	}

	#endregion
}
