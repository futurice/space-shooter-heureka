using UnityEngine;
using System.Collections;

public class AsteroidBehaviour : MonoBehaviour {
	[SerializeField]
	private float _spinMagnitude = 5.0f;


	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * _spinMagnitude;
		//place outside of bounds and calc some constant velocity, or do it in the game manager
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "projectile") {
			GameManager.Instance.destroyWithExplosion(this.gameObject);
			Destroy(other.gameObject);
		}
	}
}
