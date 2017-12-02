using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyMovement_script : MonoBehaviour {

	#region External Variables
	public Transform m_target;
	public TargetSensor_script m_sensor;
	public bool m_isAttacking = false;
	public bool m_canRotate = true;
	public bool m_canWalk = true;
	public bool m_canMove = true;
	public bool m_isWalking = true;
	public bool m_isStunned = false;
	public bool m_isKnockedDown = false;
	public bool m_isDead = false;
	#endregion

	#region Internal Variables
	protected NavMeshAgent m_agent;
	protected Animator m_animator;
	protected Rigidbody m_rb;
	protected int m_stunCount = 0;
	protected int m_attackState;
	protected int m_stunnedState;
	protected int m_knockedDownState;
	protected int m_getUpState;
	protected int m_goldValue = 0;
	private bool m_canBeStunned = true;
	private int m_stunResetTimer = 3;
	#endregion

	#region Standard Methods

	private void Awake() {
		m_agent = GetComponent<NavMeshAgent>();
		m_animator = GetComponent<Animator>();
		m_rb = GetComponent<Rigidbody>();
		m_attackState = Animator.StringToHash("BaseLayer.Attack");
		m_stunnedState = Animator.StringToHash("BaseLayer.Stunned");
		m_knockedDownState = Animator.StringToHash("BaseLayer.KnockedDown");
		m_getUpState = Animator.StringToHash("BaseLayer.GetUp");
	}
	#endregion

	#region Custom Methods

	protected void CheckIfMoving() {
		if (m_agent.remainingDistance <= m_agent.stoppingDistance + 0.5) {
			m_isWalking = false;
			m_animator.SetBool("isWalking",false);
		} else {
			m_isWalking = true;
			m_animator.SetBool("isWalking",true);
			m_animator.SetFloat("Speed", m_agent.speed);		
		}
	}

	protected void LookAtTarget(Vector3 target) {
		transform.LookAt(target);
	}

	protected void MoveToTarget(Vector3 target) {
		m_agent.SetDestination(target);
	}

	protected void MoveSideways() {

	}

	public void GetStunned() {
		if (m_canBeStunned) {
			if (m_isAttacking) {
				m_isAttacking = false;
			}
			m_stunCount = (m_stunCount+1)%3;
			switch (m_stunCount) {
				case 0:
					//nothing here
				break;
				case 1:
					HitReact();
				break;
				case 2:
					FallBackward();
				break;
				default:
					Debug.LogError("default stun state");
				break;
			}
		}
	}

	public void HitReact() {
		m_animator.SetBool("isStunned",true);
	}

	public void GetParried() {
		m_animator.SetTrigger("Parried");
	}

	public void FallBackward() {
		m_animator.SetBool("isKnockedDown",true);
		StartCoroutine(ResetStun());		
	}

	public void Die() {
		m_animator.SetBool("isDead",true);
		m_isDead = true;
		m_agent.enabled = false;
		StartCoroutine(DeactivateEnemy());
	}

	public void ResetEnemy() {	
		m_animator.Rebind();
		m_isAttacking = false;
		m_stunCount = 0;
		m_agent.enabled = true;
		m_canRotate = true;
		m_canMove = false;
		if (m_isDead) {
			GameManager.Instance.AddGold(m_goldValue);
			m_isDead = false;
		}
		GetComponent<Health_script>().ResetHealth();
		PoolManager_script.Instance.m_waveEnemies.Remove(this.gameObject);
		gameObject.SetActive(false);
	}

	IEnumerator ResetStun() {
		for (float t = 0; t < m_stunResetTimer; t += Time.deltaTime) {
			yield return null;
		}		
		m_canBeStunned = false;
		for (float t = 0; t < m_stunResetTimer; t += Time.deltaTime) {
			yield return null;
		}
		m_canBeStunned = true;
	}

	IEnumerator DeactivateEnemy(){
		for (float t = 0; t < 2; t += Time.deltaTime) {
			yield return null;
		}
		for (float t = 0; t < 2; t += Time.deltaTime) {
			transform.Translate(0,-0.05f,0);
			yield return null;
		}
		ResetEnemy();
	}
	#endregion

}
