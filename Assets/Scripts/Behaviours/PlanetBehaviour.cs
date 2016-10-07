using UnityEngine;
using System.Collections;

public class PlanetBehaviour : MonoBehaviour
{
	[SerializeField]
	private float _rotationSpeed 	= 5.0f;
	[SerializeField]
	private bool	_rotate 		= true;

	private void Update ()
	{
		if (_rotate)
		{
			this.gameObject.transform.Rotate (Vector3.up, _rotationSpeed);
		}
	}

	private void OnTriggerEnter (Collider other)
	{
		if (other.CompareTag ("projectile"))
		{
			GameManager.Instance.DestroyWithExplosion (other.gameObject, other.gameObject.GetComponent <ProjectileBehaviour> ().SourceId);
		}
		else if (other.CompareTag ("spaceship"))
		{
			SpaceShipController spaceShip = other.GetComponent<SpaceShipController> ();
			PlayerController playerController = spaceShip.Player;
			GameManager.Instance.DestroyWithExplosion (playerController.gameObject, playerController.Id);
		}
	}
}
