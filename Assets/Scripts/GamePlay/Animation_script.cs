using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_script : MonoBehaviour {

	#region External Variables
	#endregion

	#region Internal Variables
	private const int NUMATTACKS = 2;
	private int m_attackState;
	private int m_attackState1;
	private int m_attackState2;
	private int m_stunnedFrontState;
	private int m_stunnedBackState;
	private int m_attackCombo = 0;
	private bool m_incrementAttack = false;
	private int m_deathHash = Animator.StringToHash("isDead");
	private int m_blockingHash = Animator.StringToHash("isBlocking");
	private int m_attackingHash = Animator.StringToHash("AttackState");
	private Animator m_animController;
	private PlayerController_script m_controller;
	#endregion

	#region Standard Methods
	private void Awake() {
		m_animController = GetComponent<Animator>();
		m_controller = GetComponent<PlayerController_script>();
		m_attackState = Animator.StringToHash("AttackLayer.Attack");
		m_attackState1 = Animator.StringToHash("AttackLayer.Attack1");
		m_attackState2 = Animator.StringToHash("AttackLayer.Attack2");
		m_stunnedFrontState = Animator.StringToHash("BaseLayer.StunnedFromFront");
		m_stunnedBackState = Animator.StringToHash("BaseLayer.StunnedFromBack");	
	}

	private void Update() {
		AnimatorStateInfo currentBaseLayerState = m_animController.GetCurrentAnimatorStateInfo(0);
		AnimatorStateInfo currentAttackLayerState = m_animController.GetCurrentAnimatorStateInfo(1);
		HandleAttacking(currentAttackLayerState ,currentBaseLayerState);
		HandleStun(currentBaseLayerState);
	}
	#endregion

	#region Custom Methods

		private void HandleAttacking(AnimatorStateInfo currentAttackLayerState, AnimatorStateInfo currentBaseLayerState) {
			if (currentAttackLayerState.fullPathHash == m_attackState) {
				m_controller.m_canRotate = false;
				m_controller.m_canMove = false;
				m_attackCombo = 1;
				if (m_incrementAttack) {
					m_animController.SetInteger(m_attackingHash, 2);
					m_incrementAttack = false;
					m_controller.m_weapon.ResetTargets();		
				}
			} else if (currentAttackLayerState.fullPathHash == m_attackState1) {
				if (m_attackCombo == 1) {
					m_controller.m_canPlayAttackSound = true;
				}
				m_attackCombo = 2;
				if (m_incrementAttack) {
					m_animController.SetInteger(m_attackingHash, 3);
					m_incrementAttack = false;
					m_controller.m_weapon.ResetTargets();				
				}
			} else if (currentAttackLayerState.fullPathHash == m_attackState2) {
				if (m_attackCombo == 2) {
					m_controller.m_canPlayAttackSound = true;
				}
				m_attackCombo = 3;
				m_animController.SetInteger(m_attackingHash, 0);
				m_incrementAttack = false;
			} else {
				if (m_incrementAttack) {
					m_animController.SetInteger(m_attackingHash, 1);
					m_incrementAttack = false;	
					m_controller.m_weapon.ResetTargets();
					m_controller.m_canPlayAttackSound = true;
				} else {
					m_animController.SetInteger(m_attackingHash, 0);
					m_controller.m_canRotate = true;
					m_controller.m_canMove = true;
					m_controller.m_isAttacking = false;
					m_controller.m_weapon.ResetTargets();
					m_attackCombo = 0;
				}
			}
		}

		private void HandleStun(AnimatorStateInfo currentBaseLayerState) {
			if (currentBaseLayerState.fullPathHash == m_stunnedFrontState) {

			} else if (currentBaseLayerState.fullPathHash == m_stunnedBackState) {

			} else {
				m_animController.SetInteger("Stunned", 0);
			}
		}

		private void HandleBlocking(AnimatorStateInfo currentUpperTorsoState) {

		}

		public void SetAttacking() {
			m_incrementAttack = true;
		}

		public void SetDead() {
			m_animController.SetBool(m_deathHash, true);
			m_controller.m_isBlocking = false;
		}

		public void SetStunnedFront() {
			m_animController.SetInteger("Stunned", 1);
		}

		public void SetStunnedBack() {
			m_animController.SetInteger("Stunned", 2);
		}

		public void SetSpeed(float speed) {
			m_animController.SetFloat("Speed", speed);			
		}

		public void SetWalking(bool isWalking) {
			m_animController.SetBool("isWalking", isWalking);
		}

		public void SetDirection(float direction) {
			m_animController.SetFloat("Direction", direction);
		}

		public void SetBlocking(bool isBlocking) {
			m_animController.SetBool(m_blockingHash, isBlocking);
		}

		public void SetImpact() {
			m_animController.SetTrigger("Impact");
		}
	#endregion
}
