using UnityEngine;
using System.Collections;

public class AsteroidFieldBehaviour : MonoBehaviour
{
	[SerializeField]
	private float _rotationSpeed = 0.5f;

	private void FixedUpdate ()
	{
		Vector3 rotation = transform.localRotation.eulerAngles;
		rotation.y += _rotationSpeed;
		transform.localRotation = Quaternion.Euler (rotation);
	}
}
