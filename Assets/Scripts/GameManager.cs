using UnityEngine;
using System.Collections;

public class GameManager: Singleton<GameManager> {

	[SerializeField]
	private GameObject _explosionPrefab;

	[SerializeField]
	private GameObject _spaceShipPrefab;

	//TODO create players back once they're destroyed

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

		GameConstants.PlayerKeys keys = GameConstants.getPlayerKeys(id);
		PlayerController ctrl = ship.GetComponent<PlayerController>();
		ctrl.setPlayerKeys(keys);
		ctrl.Id = id;

		ship.GetComponent<WeaponLauncher>().setFireKeyCode(keys.FireBtn);

		//TODO pre-define start positions
		Vector3 startPos = Random.onUnitSphere;
		startPos.y = 0.0f;
		ship.transform.position = startPos;

	}

}
