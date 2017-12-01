using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

enum m_enemyTypes { GRUNT, WARRIOR, ARCHER}

public class PoolManager_script : MonoBehaviour {

	#region External Variables
	[HideInInspector]
	public List<GameObject> m_waveEnemies = new List<GameObject>();
	public static PoolManager_script Instance;
	public Transform m_enemyTarget;
	public Transform m_spawnPointContainer;
	public PooledEnemy[] m_enemyClass;
	[Serializable]
	public class PooledEnemy {
		public GameObject m_enemyPrefab;
		public int m_numInPool = 3;
	}
	#endregion

	#region Internal Variables
	private GameObject[] m_grunts;
	private GameObject[] m_warriors;
	private GameObject[] m_archers;
	private Transform[] m_spawnPoints;
	#endregion


	#region Standard Methods
	private void Awake() {
		Instance = this;
		InitPool();
		GetSpawns();
	}
	#endregion

	#region Custom Methods
	private void InitPool() {
		m_grunts = new GameObject[m_enemyClass[0].m_numInPool];


		for (int i = 0; i < m_enemyClass.Length; i++) {
			for (int j = 0; j < m_enemyClass[i].m_numInPool; j++) {
				switch (i) {
					case (int)m_enemyTypes.GRUNT:
						m_grunts[j] = Instantiate(m_enemyClass[i].m_enemyPrefab);
						m_grunts[j].GetComponent<EnemyMovement_script>().m_target = m_enemyTarget;
						m_grunts[j].SetActive(false);
					break;
					case (int)m_enemyTypes.WARRIOR:
						m_warriors[j] = Instantiate(m_enemyClass[i].m_enemyPrefab);
						m_warriors[j].SetActive(false);
						Debug.Log("spawned warrior");					
					break;
					case (int)m_enemyTypes.ARCHER:
						m_archers[j] = Instantiate(m_enemyClass[i].m_enemyPrefab);
						m_archers[j].SetActive(false);
						Debug.Log("spawned archer");
					break;
					default:
						Debug.Log("no class assigned");
					break;
				}
			}
		}
	}

	public void SetupEnemies(int waveNumber) {
		switch(waveNumber) {
			case 1:
				AddEnemies(2,0,0);
			break;
			case 2:
				AddEnemies(4,0,0);
			break;
			case 3:
				AddEnemies(6,0,0);
			break;
			case 4:
				AddEnemies(8,0,0);
			break;
			case 5:
				AddEnemies(10,0,0);
			break;
			case 6:
				AddEnemies(12,0,0);
			break;
			case 7:
				AddEnemies(14,0,0);
			break;
			default:
				Debug.Log("Unknown waveNumber");
			break;
		}	
	}

	private void GetSpawns() {
		Transform[] potentialSpawns = m_spawnPointContainer.GetComponentsInChildren<Transform>();
		m_spawnPoints = new Transform[ (potentialSpawns.Length - 1) ];
		for (int i = 1; i < potentialSpawns.Length; ++i ) {
 			m_spawnPoints[i - 1] = potentialSpawns[i];
		}
	}

	private void AddEnemies(int numGrunts, int numWarriors, int numArchers) {
		for (int i = 0; i < numGrunts; i++) {
			m_waveEnemies.Add(m_grunts[i]);
		}
		for (int i = 0; i < numWarriors; i++) {
			m_waveEnemies.Add(m_warriors[i]);
		}
		for (int i = 0; i < numArchers; i++) {
			m_waveEnemies.Add(m_archers[i]);
		}
		SpawnEnemies();
	}

	private void SpawnEnemies() {
		for (int i = 0; i < m_waveEnemies.Count; i++) {
			m_waveEnemies[i].transform.position = m_spawnPoints[i].position;
			m_waveEnemies[i].GetComponent<EnemyMovement_script>().m_canWalk = false;
			m_waveEnemies[i].SetActive(true);
		}
	}

	public void ResetPlayer() {
		PlayerController_script player = m_enemyTarget.GetComponent<PlayerController_script>();
		player.ResetPosition();
		player.m_gameStarted = false;
	}

	public void ActivateEnemies() {
		for (int i = 0; i < m_waveEnemies.Count; i++) {
			m_waveEnemies[i].GetComponent<EnemyMovement_script>().m_canMove = true;
			m_waveEnemies[i].GetComponent<EnemyMovement_script>().m_canWalk = true;
		}
		m_enemyTarget.GetComponent<PlayerController_script>().m_gameStarted = true;	
	}

	#endregion
}
