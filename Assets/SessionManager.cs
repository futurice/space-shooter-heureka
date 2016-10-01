using UnityEngine;
using System.Collections;

public class SessionManager : Singleton<SessionManager> {

	enum GameState {
		Idle,
		Intro,
		Instructions,
		Round1,
		MidScores,
		Round2,
		FinalScores
	}

	GameState NextState {
		get {
			switch (_curState) {
			case GameState.Idle: return GameState.Intro;
			case GameState.Intro: return GameState.Instructions;
			case GameState.Instructions: return GameState.Round1;
			case GameState.Round1: return GameState.MidScores;
			case GameState.MidScores: return GameState.Round2;
			case GameState.Round2: return GameState.FinalScores;
			case GameState.FinalScores: return GameState.Intro;
			}
			return GameState.Idle;
		}
	}

	float StateLength {
		get {
			switch (_curState) {
			case GameState.Idle: return -1.0f;
			case GameState.Intro: return 10.0f;
			case GameState.Instructions: return 30.0f;
			case GameState.Round1: return 150.0f;
			case GameState.MidScores: return 20.0f;
			case GameState.Round2: return 150.0f;
			case GameState.FinalScores: return 20.0f;
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
	private int _sessionId = 0;

	void Start () {
		initSession();
		InsultManager.Instance.tellIntro();
	}

	void initSession() {
		InsultManager.Instance.readInsults();
		_sessionId++;
	}

	void gotoNextState() {
		GameState newState = NextState;

		if (newState == GameState.Idle) {
			//we're just waiting this to start
			initSession();
		}
		if (newState == GameState.Intro) {
			InsultManager.Instance.tellIntro();
			//TODO possibly show some video?
		}
		else if (newState == GameState.Instructions) {
			InsultManager.Instance.tellInstructions();
		}
		else if (newState == GameState.Round1 || newState == GameState.Round2) {
			GameManager.Instance.startNewRound();
		}
		else if (newState == GameState.MidScores) {
			//TODO show scores
			GameManager.Instance.stopRound();
		}
		else if (newState == GameState.FinalScores) {
			//TODO show scores
			GameManager.Instance.stopRound();
			ScoreManager.Instance.saveSession(_sessionId);
		}
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
			Debug.LogWarning("Moving on to next phase");
			gotoNextState();
			return;
		}

		_timer += Time.deltaTime;
	}
}
