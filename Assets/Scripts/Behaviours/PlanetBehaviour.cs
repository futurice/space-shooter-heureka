using UnityEngine;
using System.Collections;

public class PlanetBehaviour : MonoBehaviour
{
	[SerializeField]
	private float _rotationSpeed = 5.0f;
	private Vector3 _direction = Vector3.zero;

	private void Start ()
	{
		_direction = Random.insideUnitSphere;
	}
	
	private void Update ()
	{
		//Rigidbody rb = GetComponent<Rigidbody> ();
		//rb.transform.Rotate(_rotationSpeed * _direction);
		this.gameObject.transform.Rotate(_rotationSpeed * _direction);

	}

	private void OnTriggerEnter (Collider other)
	{
		if (other.tag == "projectile")
		{
			GameManager.Instance.DestroyWithExplosion (other.gameObject, other.gameObject.GetComponent <ProjectileBehaviour> ().SourceId);
		}
		else if (other.tag == "spaceship")
		{
			GameManager.Instance.DestroyWithExplosion (other.gameObject, other.gameObject.GetComponent<PlayerController> ().Id);
		}
	}
}
