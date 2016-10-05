using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager: Singleton<GameManager>, Timeoutable.TimeoutListener {

	[SerializeField]
	private GameObject _explosionPrefab;

	[SerializeField]
	private List<GameObject> _spaceShipPrefabs;

	[SerializeField]
	private Transform _homePlanet;

	[SerializeField]
	private Transform _gameArea;

	[SerializeField]
	private GameObject _collectablePrefab;

	[SerializeField]
	private GameObject _asteroidPrefab;
	private List<GameObject> _asteroids = new List<GameObject>();

	private float _secsUntilNextCollectable = 0.0f;
	private bool _warningGiven = false;

	private Dictionary<int, PlayerState> _playerShips = new Dictionary<int, PlayerState>();

	private bool _isGameActive = false;

  	private class PlayerState {
		public PlayerState(GameObject obj) {
			_ship = obj;
		}
		public GameObject _ship;
		public float _idleTime = 0.0f;
	}

	void Start() {
		string[] joysticks = Input.GetJoystickNames();
		foreach (string j in joysticks) {
			Debug.Log("Found joystick " + j);
		}

		for (int n = 0; n < GameConstants.NUMBER_OF_ASTEROIDS; n++) {
			createAsteroid();
		}

	}

	public void startNewRound () {
		//TODO enumerate min and max
		for (int id = 1; id < GameConstants.NUMBER_OF_PLAYERS + 1; id++) {
			createPlayer(id);
		}

		initCollectableTimeout();
		_warningGiven = false;
		_isGameActive = true;
		AudioManager.Instance.speak("Board your ships.");
	}

	public void stopRound() {
		//TODO modify state so players cannot re-create themselves
		destroyAll();
		_isGameActive = false;
		AudioManager.Instance.speak("Time's up.");
	}

	private void initCollectableTimeout() {
		//Rate is loosely controlled by the number of active ships
		float timeout = 10.0f;
		if (_playerShips.Count < 4) {
			timeout = 10.0f;
		}
		else {
			timeout = 5.0f;
		}
		_secsUntilNextCollectable = Mathf.Clamp(timeout * Random.value, 1.0f, timeout);
	}

	void Update() {
		if (!_isGameActive) {
			return;
		}

		//TODO remove direct button listening from this class... abstract it somewhere and just get events, etc
		for (int id = 1; id < GameConstants.NUMBER_OF_PLAYERS + 1; id++) {
			if (!_playerShips.ContainsKey(id)) {
				GameConstants.PlayerKeys keys = GameConstants.getPlayerKeys(id);
				bool pressed = Input.GetButtonDown(keys.SpawnBtn);
				if (pressed) {
					Debug.Log(string.Format ("Spawnbutton {0} pressed for ship ID {1}", keys.SpawnBtn, id));

					createPlayer(id);
				}

			}
		}

		_secsUntilNextCollectable -= Time.deltaTime;
		if (_secsUntilNextCollectable <= 0.0f) {
			//These should also be cleaned up.. Add logic to CollectableBehaviour or here
			createCollectable();
			initCollectableTimeout();
		}

		if (!_warningGiven) {
			float secsLeft = 30.0f;
			if (SessionManager.Instance.gameSessionLeft() < secsLeft) {
				AudioManager.Instance.speak("You have thirty seconds");
				_warningGiven = true;
			}
		}
	}

	/**
	 * Note! Create and Destroy spaceship instances ONLY through this class!
	 * This singleton instance keeps the track who is spawned, when and where
	 * */

	public void destroyWithExplosion(GameObject obj) {
		destroyWithExplosion(obj, true, true);
	}

	public void destroyWithExplosion(GameObject obj, bool scores, bool sounds) {
		animateExplosion(obj.transform);
		Destroy(obj);

		//TODO add tags to constants
		if (obj.tag == "spaceship" ) {
			int id = obj.GetComponent<PlayerController>().Id;
			if (_playerShips.Remove(id)) {
				Debug.Log(string.Format("player {0} died.", id));
				if (scores) {
					ScoreManager.Instance.addPoints(id, GameConstants.POINTS_FOR_DYING);
				}
				if (sounds) {
					InsultManager.Instance.playerDied(id);
					AudioManager.Instance.playClip(AudioManager.AppAudioClip.Explosion);
				}
			}
		}
	}

	private void destroyAll() {
		foreach(PlayerState s in _playerShips.Values) {
			GameObject obj = s._ship;
			animateExplosion(obj.transform);
			Destroy(obj);
		}
		_playerShips.Clear();
		AudioManager.Instance.playClip(AudioManager.AppAudioClip.Explosion);
	}

	private void animateExplosion(Transform t) {
		GameObject explosion = Instantiate(_explosionPrefab) as GameObject;
		explosion.transform.position = t.position;
		float duration = explosion.GetComponent<ParticleSystem>().duration;
		Destroy(explosion, duration);
	}


	public void destroyAsteroid(GameObject obj) {
		Debug.Log("destroy asteroid");
		if (_asteroids.Remove(obj)) {
			//create new to replace old one
			Debug.Log("Creating a new asteroid to replace old");
			createAsteroid();
		}
		else {
			Debug.LogWarning("Destroying asteroid that's not on the list!");
		}
		AsteroidBehaviour behaviour = obj.GetComponent<AsteroidBehaviour>();
		behaviour.destroyMe();
	}

	private void createPlayer(int id) {
		int index =  id % _spaceShipPrefabs.Count;
		GameObject prefab = _spaceShipPrefabs[index];
		GameObject ship = Instantiate(prefab) as GameObject;
		_playerShips[id] = new PlayerState(ship);

		//init game controller keys. See "Project Settings > Input" where the id mapping is
		GameConstants.PlayerKeys keys = GameConstants.getPlayerKeys(id);
		PlayerController ctrl = ship.GetComponent<PlayerController>();
		ctrl.setPlayerKeys(keys);
		ctrl.Id = id;
		ctrl.PlayerInformation = PlayerInformationManager.Instance.GetPlayerInformation (id);
		ScoreManager.Instance.addPoints (id, 0);

		ship.GetComponent<WeaponLauncher>().setFireKeyCode(keys.FireBtn);

		//set initial position to unique place around the home planet
		Vector3 directionOnUnitCircle = GameConstants.getPlayerStartDirection(id);
		Vector3 startPos = _homePlanet.position + 1.2f * _homePlanet.localScale.magnitude * directionOnUnitCircle; 
		ship.transform.position = startPos;

		ctrl.addTimeoutListener(this);
	}

	public void timeoutElapsed(Timeoutable t) {
		//TODO refactor tags out, use method accesesor for this feature
		if (t.tag == "spaceship") {
			//Invoked by the Controllers after a timeout
			destroyWithExplosion(t.gameObject, false, false);
		}
		else {
			Destroy(t.gameObject);
		}
	}


	private void createCollectable() {
		GameObject collectable = Instantiate(_collectablePrefab) as GameObject;
		float randomX = (Random.value - 0.5f) * _gameArea.localScale.x;
		float randomZ = (Random.value - 0.5f) * _gameArea.localScale.z;

		collectable.transform.position = new Vector3(randomX, 0.0f, randomZ); 

		collectable.GetComponent<CollectableBehaviour>().addTimeoutListener(this);
	}

	private void createAsteroid() {
		//TODO some animation
		GameObject asteroid = Instantiate(_asteroidPrefab) as GameObject;
		if (Random.value > 0.2f) {
			asteroid.AddComponent<AsteroidBehaviour>();
		}
		else {
			asteroid.AddComponent<FallingAsteroidBehaviour>();
		}
		//TODO make sure it doesn't overlap with the planet
		float randomX = (Random.value - 0.5f) * _gameArea.localScale.x;
		float randomZ = (Random.value - 0.5f) * _gameArea.localScale.z;
		asteroid.transform.position = new Vector3(randomX, 0.0f, randomZ); 

		float scaleFactor = Random.Range(0.5f, 1.0f);
		asteroid.transform.localScale = scaleFactor * asteroid.transform.localScale;
		_asteroids.Add(asteroid);
	}
	

}
