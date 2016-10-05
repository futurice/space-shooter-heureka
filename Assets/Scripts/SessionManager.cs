using UnityEngine;
using System.Collections;

public class SessionManager : Singleton<SessionManager> {

	[SerializeField]
	GameObject _introPoster;

	enum GameState {
		Idle,
		Intro,
		Instructions,
		Round1,
		Round1Cleanup,
		MidScores,
		Round2,
		Round2Cleanup,
		FinalScores
	}

	GameState NextState {
		get {
			switch (_curState) {
			case GameState.Idle: return GameState.Intro;
			case GameState.Intro: return GameState.Instructions;
			case GameState.Instructions: return GameState.Round1;
			case GameState.Round1: return GameState.Round1Cleanup;
			case GameState.Round1Cleanup: return GameState.MidScores;
            case GameState.MidScores: return GameState.Round2;
			case GameState.Round2: return GameState.Round2Cleanup;
			case GameState.Round2Cleanup: return GameState.FinalScores;
            case GameState.FinalScores: return GameState.Idle;
			}
			return GameState.Idle;
		}
	}

	float StateLength {
		get {
			switch (_curState) {
			case GameState.Idle: return -1.0f;
			case GameState.Intro: return 5.0f;
			case GameState.Instructions: return 65.0f;
			case GameState.Round1: return 180.0f;
			case GameState.Round1Cleanup: return 5.0f;
            case GameState.MidScores: return 15.0f;
			case GameState.Round2: return 180.0f;
			case GameState.Round2Cleanup: return 5.0f;
			case GameState.FinalScores: return 120.0f;
			}
			return -1.0f;
		}
	}
	private GameState _curState = GameState.Idle;
	GameState CurrentState {
		get { return _curState; }
		set { _curState = value; }
	}

	private float _timer = 0.0f;
	private int _sessionId = 0;//TODO This should be initialized from some persistent store, now always starts from 0 

	void Start () {
		InsultManager.Instance.readAllSpeeches();
		_curState = GameState.Idle;
		showIntroPoster(true);
    }

	public float gameSessionLeft() {
		return StateLength - _timer;
	}

	void initGame() {
		_sessionId++;
		ScoreManager.Instance.clearScores();
	}

	private void showIntroPoster(bool show) {
		if (_introPoster != null) {
			_introPoster.SetActive(show);
		}
	}

	void gotoNextState() {
		GameState newState = NextState;

		if (newState == GameState.Idle) {
			//we're just waiting this to start
		}
		else if (newState == GameState.Intro) {
			//InsultManager.Instance.tellIntro();
			//TODO possibly show some video? or just play some music, etc
		}
		else if (newState == GameState.Instructions) {
			InsultManager.Instance.tellInstructions();
		}
		else if (newState == GameState.Round1 || newState == GameState.Round2) {
			initGame();
			GameManager.Instance.startNewRound();
		}
		else if (newState == GameState.Round1Cleanup || newState == GameState.Round2Cleanup) {
			GameManager.Instance.stopRound();
        }

		// Show high scores if the game state is MidScores or FinalScores, otherwise Hide HighScores
		if (newState == GameState.MidScores || newState == GameState.FinalScores) {
			ScoreManager.Instance.saveSession(_sessionId);
			InsultManager.Instance.tellHighscores (ScoreManager.Instance.ScoresSorted, newState == GameState.MidScores);
			HighScoreListManager.Instance.ShowHighScores ();
		}
		else
		{
			HighScoreListManager.Instance.HideHighScores ();
		}
	
		showIntroPoster(newState == GameState.Idle);

		Debug.Log("Setting New State: " + newState);
		CurrentState = newState;
		_timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update() {

        if (Input.GetKey("escape")) {
			Debug.LogWarning("Quitting application");
			Application.Quit();
		}

		if (Input.GetKeyDown(KeyCode.Return)) {
			Debug.LogWarning("Moving on to next phase by force");
			gotoNextState();
		}
		else {
			_timer += Time.deltaTime;
			
			if (CurrentState != GameState.Idle && _timer >= StateLength) {
				Debug.LogWarning("Moving on to next phase by timeout");
	            gotoNextState();
	        }
		}
	}
}
