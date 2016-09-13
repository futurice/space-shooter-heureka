using UnityEngine;
using System.Collections;

public class GameManager: Singleton<GameManager> {

	[SerializeField]
	private GameObject _explosionPrefab;

	//TODO create players back once they're destroyed

	public void destroyWithExplosion(GameObject obj) {
		GameObject explosion = Instantiate(_explosionPrefab) as GameObject;
		explosion.transform.position = obj.transform.position;
		float duration = explosion.GetComponent<ParticleSystem>().duration;
		Destroy(explosion, duration);

		Destroy(obj);
	}
}
