using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager: Singleton<GameManager> {

	[SerializeField]
	private GameObject _explosionPrefab;

	[SerializeField]
	private GameObject _spaceShipPrefab;

	[SerializeField]
	private Transform _homePlanet;

	[SerializeField]
	private Transform _gameArea;

	[SerializeField]
	private GameObject _collectablePrefab;

	private float _secsUntilNextCollectable = 0.0f;

	private Dictionary<int, PlayerState> _playerShips = new Dictionary<int, PlayerState>();

  	private class PlayerState {
		public PlayerState(GameObject obj) {
			_ship = obj;
		}
		public GameObject _ship;
		public float _idleTime = 0.0f;
	}


	void Start () {
		//TODO enumerate min and max
		for (int id = 1; id < GameConstants.NUMBER_OF_PLAYERS + 1; id++) {
			createPlayer(id);
		}
		initCollectableTimeout();
	}

	private void initCollectableTimeout() {
		//TODO: Rate should probably be controlled by the number of active ships?
		_secsUntilNextCollectable = Mathf.Clamp(10.0f * Random.value, 1.0f, 10.0f);
	
	}

	void Update() {
		if (Input.GetKey("escape")) {
			Debug.LogWarning("Quitting application");
			Application.Quit();
        }

		//TODO remove direct button listening from this class... abstract it somewhere and just get events, etc
		for (int id = 1; id < GameConstants.NUMBER_OF_PLAYERS + 1; id++) {
			if (!_playerShips.ContainsKey(id)) {
				GameConstants.PlayerKeys keys = GameConstants.getPlayerKeys(id);

				bool pressed = Input.GetButtonDown(keys.SpawnBtn);
				if (pressed) {
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
		//TODO also blow up idle spaceships with no movement after N secs.
		checkIdleTimeouts();
	}

	/**
	 * Note! Create and Destroy spaceship instances ONLY through this class!
	 * This singleton instance keeps the track who is spawned, when and where
	 * */

	public void destroyWithExplosion(GameObject obj) {
		GameObject explosion = Instantiate(_explosionPrefab) as GameObject;
		explosion.transform.position = obj.transform.position;
		float duration = explosion.GetComponent<ParticleSystem>().duration;
		Destroy(explosion, duration);

		Destroy(obj);

		//TODO add tags to constants
		if (obj.tag == "spaceship" ) {
			int id = obj.GetComponent<PlayerController>().Id;
			_playerShips.Remove(id);
		}
	}

	private void createPlayer(int id) {
		GameObject ship = Instantiate(_spaceShipPrefab) as GameObject;
		_playerShips[id] = new PlayerState(ship);

		//init game controller keys. See "Project Settings > Input" where the id mapping is
		GameConstants.PlayerKeys keys = GameConstants.getPlayerKeys(id);
		PlayerController ctrl = ship.GetComponent<PlayerController>();
		ctrl.setPlayerKeys(keys);
		ctrl.Id = id;

		ship.GetComponent<WeaponLauncher>().setFireKeyCode(keys.FireBtn);

		//set initial position to unique place around the home planet
		Vector3 directionOnUnitCircle = GameConstants.getPlayerStartDirection(id);
		Vector3 startPos = _homePlanet.position + 1.2f * _homePlanet.localScale.magnitude * directionOnUnitCircle; 
		ship.transform.position = startPos;


	}

	private void createCollectable() {
		GameObject collectable = Instantiate(_collectablePrefab) as GameObject;
		float randomX = (Random.value - 0.5f) * _gameArea.localScale.x;
		float randomZ = (Random.value - 0.5f) * _gameArea.localScale.z;

		collectable.transform.position = new Vector3(randomX, 0.0f, randomZ); 
	}

	private void checkIdleTimeouts() {
		List<GameObject> destroyUs = new List<GameObject>();

		foreach (PlayerState state in _playerShips.Values) {
			PlayerController ctrl = state._ship.GetComponent<PlayerController>();
			if (ctrl.hasInput()) {
				state._idleTime = 0.0f;
			}
			else {
				state._idleTime += Time.deltaTime;
				if (state._idleTime > GameConstants.PLAYER_IDLE_TIMEOUT) {
					destroyUs.Add(state._ship);
				}
			}
		}

		foreach(GameObject killMe in destroyUs) {
			destroyWithExplosion(killMe);
		}
	}
}
