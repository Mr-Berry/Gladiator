using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour {

	#region External Variables
	public static GameManager Instance { get{return m_instance;}}
	public GameObject[] m_canvases;
	public bool m_playMode;
	public static int m_playerGold = 0;
	public bool m_canExitToMenu = false;
	public PoolManager_script m_pool;
	public Transform m_waveTextObject;
	public Text m_goldText;
	public Text m_loseText;
	public Image m_loseBG;
	public GameObject[] m_postGameStats;
	public Text[] m_postGameStatsTexts;
	#endregion

	#region Internal Variables
	private static GameManager m_instance = null;
	private Text[] m_waveTexts;
	private int m_currentCanvas = 0;
	private int m_currentWave = 0;
	private int m_bestWave = 0;
	private int m_enemiesKilled = 0;
	private int m_goldGained = 0;
	#endregion

	#region Standard Methods

	void Awake () {
		m_instance = this;
		InitPlayerPrefs();
		if (m_playMode) {
			InitPlayerGold();
			m_waveTexts = m_waveTextObject.GetComponentsInChildren<Text>();
		}
	}

	private void Update() {
		if (m_playMode) {
			if (m_pool.m_waveEnemies.Count == 0) {
				m_currentWave++;
				m_pool.SetupEnemies(m_currentWave);
				m_pool.ResetPlayer();
				m_waveTexts[0].text = "Wave " + m_currentWave;
				m_waveTexts[1].text = "Wave " + m_currentWave;
				StartCoroutine(ShowWaveNumber());
			}
		}
	}
	#endregion

	#region Custom Methods
		
	public void StartGame() {
		SceneManager.LoadScene(1,LoadSceneMode.Single);
	}

	public void LoadMenu() {
		if (m_canExitToMenu) {
			Time.timeScale = 1;
			SceneManager.LoadScene(0,LoadSceneMode.Single);
		}
	}

	public void QuitGame() {
		Application.Quit();
	}

	public void SwitchCanvas() {
		m_canvases[m_currentCanvas].SetActive(false);
		m_currentCanvas = (m_currentCanvas+1)%2;
		m_canvases[m_currentCanvas].SetActive(true);
	}

	public void GameOver() {

	}

	IEnumerator ShowWaveNumber() {
		m_waveTextObject.gameObject.SetActive(true);
		for(float t = 0; t < 1; t += Time.deltaTime) {
			m_waveTextObject.localScale = new Vector3(t,t,t);
			yield return null;
		}
		StartCoroutine(FadeWaveNumber());
	}

	IEnumerator FadeWaveNumber() {
		Color orig1 = m_waveTexts[0].color;
		Color orig2 = m_waveTexts[1].color;
		orig1.a = 0;
		orig2.a = 0;
		for (float t = 0; t < 1; t += Time.deltaTime) {
			yield return null;
		}		
		for (float t = 1; t > 0; t -= Time.deltaTime) {
			m_waveTexts[0].color = Color.Lerp(m_waveTexts[0].color,orig1,0.1f);
			m_waveTexts[1].color = Color.Lerp(m_waveTexts[1].color,orig2,0.1f);	
			yield return null;
		}
		orig1.a = 255;
		orig2.a = 255;
		m_waveTexts[0].color = orig1;
		m_waveTexts[1].color = orig2;
		m_waveTextObject.gameObject.SetActive(false);
		m_pool.ActivateEnemies();
	}

	private void SetGoldText() {
		m_goldText.text = m_playerGold.ToString();
	}

	public void AddGold(int gold) {
		m_goldGained += gold;
		StartCoroutine(AddGoldSlowly(gold));
		m_enemiesKilled++;
	}

	public void EndGame() {
		Invoke("ShowEndGameScreen",4);
	}

	IEnumerator AddGoldSlowly(int gold) {
		for (int i = 0; i < gold*3; i++) {
			if (i%3 == 0) {
				m_playerGold++;
				SetGoldText();
			}
			yield return null;
		}
	}

	private void InitPlayerGold() {
		SetGoldText();
	}

	private void ShowEndGameScreen() {
		Time.timeScale = 0;
		StartCoroutine(SetEndGameScreen());
	}

	private void SetEndGameStats() {
		int i = 0;
		while (i < m_postGameStatsTexts.Length) {
			switch (i) {
				case 0:
					m_postGameStatsTexts[i].text = m_currentWave.ToString();
				break;
				case 1:
					if (m_currentWave > m_bestWave) {
						m_bestWave = m_currentWave;
						PlayerPrefs.SetInt("BestWave",m_bestWave);
					}
					m_postGameStatsTexts[i].text = m_bestWave.ToString();
				break;
				case 2:
					m_postGameStatsTexts[i].text = m_enemiesKilled.ToString();
				break;
				case 3:
					m_postGameStatsTexts[i].text = m_goldGained.ToString();
					PlayerPrefs.SetInt("Gold", m_playerGold);
				break;
				default:
				break;
			}
			i++;
		}
	}

	IEnumerator SetEndGameScreen() {
		m_loseBG.gameObject.SetActive(true);
		m_loseText.gameObject.SetActive(true);
		float BG_Alpha = m_loseBG.color.a;
		float Text_Alpha = m_loseText.color.a;
		for (float t = 0; t < 2; t += Time.fixedDeltaTime) {
			Color newColor = m_loseBG.color;
			newColor.a = t*BG_Alpha/2f;
			m_loseBG.color = newColor;
			newColor = m_loseText.color;
			newColor.a = t*Text_Alpha/2f;
			m_loseText.color = newColor;
			yield return null;
		}
		SetEndGameStats();
		StartCoroutine(ShowStats(0));
	}

	IEnumerator ShowStats(int i) {
		yield return new WaitForSecondsRealtime(1);
		// play sound and show object
		switch (i) {
			case 0:
				m_postGameStats[i].SetActive(true);
				StartCoroutine(ShowStats(1));
			break;
			case 1:
				m_postGameStats[i].SetActive(true);
				StartCoroutine(ShowStats(2));
			break;
			case 2:
				m_postGameStats[i].SetActive(true);
				StartCoroutine(ShowStats(3));
			break;
			case 3:
				m_postGameStats[i].SetActive(true);
				StartCoroutine(ShowStats(4));
			break;
			default:
				m_canExitToMenu = true;
			break;
		}
	}

	private void InitPlayerPrefs() {
		if (PlayerPrefs.HasKey("BestWave")) {
			m_bestWave = PlayerPrefs.GetInt("BestWave");
		} else {
			PlayerPrefs.SetInt("BestWave", m_bestWave);
		}
		if (PlayerPrefs.HasKey("Gold")) {
			m_playerGold = PlayerPrefs.GetInt("Gold");
		} else {
			PlayerPrefs.SetInt("Gold", m_playerGold);
		}
	}

	#endregion
}
