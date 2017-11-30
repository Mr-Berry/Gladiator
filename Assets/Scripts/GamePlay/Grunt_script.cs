using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt_script : EnemyMovement_script {

	#region External Variables
	public Weapon_script m_weapon;
	#endregion

	#region Internal Variables
	private int m_gruntGold = 20;
	#endregion

	#region Standard Methods

	private void Start() {
		m_goldValue = m_gruntGold;
	}

	private void Update() {
		if (!m_isDead) {
			HandleAnimations();
			if (!m_isStunned && !m_isKnockedDown) {
				CheckIfMoving();
				Vector3 target = m_target.position;
				target.y = transform.position.y;
				if (m_canRotate) {
					LookAtTarget(target);
				}
				if (m_canMove) {
					if (m_canWalk && !m_isAttacking) {
						MoveToTarget(target);
					}
					if (!m_isWalking && m_sensor.m_hasTarget) {
						m_animator.SetBool("isAttacking", true);
						m_canWalk = false;
					}
				}
			}
		}
	}
	#endregion

	#region Custom Methods
	private void HandleAnimations() {
		AnimatorStateInfo currentBaseLayerState = m_animator.GetCurrentAnimatorStateInfo(0);
		HandleAttacking(currentBaseLayerState);
		HandleStun(currentBaseLayerState);
	}

	private void HandleAttacking(AnimatorStateInfo State) {
		if (State.fullPathHash == m_attackState) {
			m_isAttacking = true;
			if (m_canWalk) {
				m_canWalk = false;
			}
		} else if (m_isAttacking) {
			m_animator.SetBool("isAttacking", false);
			m_isAttacking = false;
			m_weapon.ResetTargets();
			m_canWalk = true;
		}		
	}

	private void HandleStun(AnimatorStateInfo State) {
		if (State.fullPathHash == m_stunnedState) {
			m_canWalk = false;
			m_isStunned = true;
			m_agent.enabled = false;
			m_animator.applyRootMotion = false;
		} else if (State.fullPathHash == m_knockedDownState) {

			m_isKnockedDown = true;
			m_animator.SetBool("isStunned", false);			
			m_isStunned = false;
		} else if (State.fullPathHash == m_getUpState) {
			
		} else if (m_isStunned || m_isKnockedDown) {
			m_animator.applyRootMotion = true;
			m_animator.SetBool("isStunned", false);
			m_animator.SetBool("isKnockedDown", false);			
			m_isStunned = false;
			m_isKnockedDown = false;
			m_canWalk = true;
			m_agent.enabled = true;
			m_stunCount = 0;
		}	
	}

	public void SetTarget (Transform target) {
		m_target = target;
		m_weapon.m_player = target.GetComponent<PlayerController_script>();
	}

	#endregion

}
