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

	private Dictionary<int, GameObject> _playerShips = new Dictionary<int, GameObject>();

	void Start () {
		//TODO enumerate min and max
		for (int id = 1; id < GameConstants.NUMBER_OF_PLAYERS + 1; id++) {
			createPlayer(id);
		}
	}

	void Update() {
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
		//TODO also blow up idle spaceships with no movement after N secs.
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
		_playerShips[id] = ship;

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

}
