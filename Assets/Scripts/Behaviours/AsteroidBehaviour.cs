﻿using UnityEngine;
using System.Collections;

public class AsteroidBehaviour : MonoBehaviour {
	[SerializeField]
	private float _spinMagnitude = 5.0f;


	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * _spinMagnitude;
		//place outside of bounds and calc some constant velocity, or do it in the game manager
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "projectile") {
			//destroyMe();
			GameManager.Instance.destroyAsteroid(this.gameObject);
			Destroy(other.gameObject);
		}
	}

	public virtual void destroyMe() {
		//not very good way, we could just have the code here then...
		BonusSpawnerBehaviour bonusSpawner = GetComponent<BonusSpawnerBehaviour>();
		if (bonusSpawner) {
			bonusSpawner.spawnBonusOnDestroy(this.gameObject.transform.position);	
		}

		GameManager.Instance.destroyWithExplosion(this.gameObject);

	}
}