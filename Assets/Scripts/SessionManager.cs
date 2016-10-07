using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SessionManager : Singleton<SessionManager> {

	[SerializeField]
	private GameObject _introPoster;
    [SerializeField]
    private GameObject _titleBanner;

	[Header("Instructions")]
	[SerializeField]
	private GameObject _instructionsContainer;
	[SerializeField]
	private CanvasGroup _instructionsCanvasGroup;

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

	GameState NextState
	{
		get
		{
			switch (_curState)
			{
				case GameState.Idle: 			return GameState.Intro;
                case GameState.Intro: 			return GameState.Instructions;
                case GameState.Instructions: 	return GameState.Round1;
				case GameState.Round1: 			return GameState.Round1Cleanup;
				case GameState.Round1Cleanup: 	return GameState.MidScores;
	            case GameState.MidScores: 		return GameState.Round2;
				case GameState.Round2: 			return GameState.Round2Cleanup;
				case GameState.Round2Cleanup: 	return GameState.FinalScores;
	            case GameState.FinalScores: 	return GameState.Idle;
			}

			return GameState.Idle;
		}
	}

	private float StateLength
	{
		get
		{
			return GetStateLength (_curState);
		}
	}

	private GameState _curState = GameState.Idle;

	GameState CurrentState
	{
		get
		{
			return _curState;
		}

		set
		{
			_curState = value;
		}
	}

	private float _timer = 0.0f;
	private int _sessionId = 0;//TODO This should be initialized from some persistent store, now always starts from 0 

	private void Start ()
	{
		InsultManager.Instance.readAllSpeeches();
		_curState = GameState.Idle;
		ShowIntroPoster(true);
    }

	private float GetStateLength (GameState state)
	{
		switch (state)
		{
		case GameState.Idle: 			return -1.0f;
		case GameState.Intro: 			return 25.0f;
        case GameState.Instructions: 	return 55.0f;
		case GameState.Round1: 			return 180.0f;
		case GameState.Round1Cleanup: 	return 5.0f;
		case GameState.MidScores: 		return 15.0f;
		case GameState.Round2: 			return 180.0f;
		case GameState.Round2Cleanup: 	return 5.0f;
		case GameState.FinalScores: 	return 120.0f;
		}

		return -1.0f;
	}

	public float gameSessionLeft ()
	{
		return StateLength - _timer;
	}

	private void InitGame ()
	{
		_sessionId++;
		ScoreManager.Instance.clearScores();
	}

	private void ShowIntroPoster (bool show)
	{
		if (_introPoster != null)
		{
			_introPoster.SetActive (show);
		}
	}

    private void ShowTitleBanner (bool show)
    {
        if (_titleBanner != null)
        {
            _titleBanner.SetActive (show);
        }
    }

	private void ShowInstructions (bool show)
	{
		if (_instructionsContainer != null)
		{
			if (show)
			{
				_instructionsCanvasGroup.alpha = 0.0f;
				_instructionsCanvasGroup.DOFade (1.0f, 4.0f)
					.SetDelay (8.0f)
					.SetEase (Ease.Linear);
			}
			else
			{
				_instructionsCanvasGroup.alpha = 0.0f;
			}

			_instructionsContainer.SetActive (show);
		}
	}

	private void GotoNextState ()
	{
		GameState newState = NextState;

		if (newState == GameState.Idle)
		{
			//we're just waiting this to start
		}
		else if (newState == GameState.Intro)
		{
            AudioManager.Instance.playClip(AudioManager.AppAudioClip.IntroFanfare);
            _titleBanner.GetComponent<IntroBannerBehaviour>().doAnimation();
        }
		else if (newState == GameState.Instructions)
		{
			InsultManager.Instance.tellInstructions();
		}
		else if (newState == GameState.Round1 || newState == GameState.Round2)
		{
			InitGame ();
			GameManager.Instance.StartNewRound (GetStateLength (newState));
		}
		else if (newState == GameState.Round1Cleanup || newState == GameState.Round2Cleanup)
		{
			GameManager.Instance.StopRound ();
        }

		// Show high scores if the game state is MidScores or FinalScores, otherwise Hide HighScores
		if (newState == GameState.MidScores || newState == GameState.FinalScores)
		{
			ScoreManager.Instance.saveSession(_sessionId);
			InsultManager.Instance.tellHighscores (ScoreManager.Instance.ScoresSorted, newState == GameState.MidScores);
			HighScoreListManager.Instance.ShowHighScores ();
		}
		else
		{
			HighScoreListManager.Instance.HideHighScores ();
		}
	
		ShowInstructions (newState == GameState.Instructions);
		ShowIntroPoster (newState == GameState.Idle);
        ShowTitleBanner (newState == GameState.Intro);

		Debug.Log("Setting New State: " + newState);
		CurrentState = newState;
		_timer = 0.0f;
	}
	
	private void Update ()
	{

        if (Input.GetKey("escape")) {
			Debug.LogWarning("Quitting application");
			Application.Quit();
		}

		if (Input.GetKeyDown(KeyCode.Return)) {
			Debug.LogWarning("Moving on to next phase by force");
			GotoNextState();
		}
		else {
			_timer += Time.deltaTime;
			
			if (CurrentState != GameState.Idle && _timer >= StateLength) {
				Debug.LogWarning("Moving on to next phase by timeout");
	            GotoNextState();
	        }
		}
	}
}
