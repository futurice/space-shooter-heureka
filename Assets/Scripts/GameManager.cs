using UnityEngine;
using System.Collections;

public class GameManager: Singleton<GameManager> {

	[SerializeField]
	private GameObject _explosionPrefab;

	[SerializeField]
	private GameObject _spaceShipPrefab;

	[SerializeField]
	private Transform _homePlanet;

	private const float CREATE_TIMEOUT = 2.0f;

	void Start () {
		//TODO enumerate min and max
		for (int id = 1; id < GameConstants.NUMBER_OF_PLAYERS + 1; id++) {
			createPlayer(id);
		}
	}


	public void destroyWithExplosion(GameObject obj) {
		GameObject explosion = Instantiate(_explosionPrefab) as GameObject;
		explosion.transform.position = obj.transform.position;
		float duration = explosion.GetComponent<ParticleSystem>().duration;
		Destroy(explosion, duration);

		Destroy(obj);

		//TODO add to constants
		if (obj.tag == "spaceship" ) {
			//TODO add delay, also minor random?
			//createPlayer(obj.GetComponent<PlayerController>().Id);
			//Invoke("createPlayer", CREATE_TIMEOUT);
		}
	}

	void createPlayer(int id) {
		GameObject ship = Instantiate(_spaceShipPrefab) as GameObject;

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
