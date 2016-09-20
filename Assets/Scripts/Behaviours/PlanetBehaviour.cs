using UnityEngine;
using System.Collections;

public class PlanetBehaviour : MonoBehaviour {
	[SerializeField]
	private float _rotationSpeed = 5.0f;

	private Vector3 _direction = new Vector3(0, 0, 0);
	// Use this for initialization
	void Start () {
		_direction = Random.insideUnitSphere;
	}
	
	// Update is called once per frame
	void Update () {
		//Rigidbody rb = GetComponent<Rigidbody> ();
		//rb.transform.Rotate(_rotationSpeed * _direction);
		this.gameObject.transform.Rotate(_rotationSpeed * _direction);

	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "projectile") {
			GameManager.Instance.destroyWithExplosion(other.gameObject);
			//Destroy(other.gameObject);
		}
		else if (other.tag == "spaceship") {
			GameManager.Instance.destroyWithExplosion(other.gameObject);
		}
	}
}
