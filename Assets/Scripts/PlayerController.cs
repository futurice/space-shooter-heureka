using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {

	[SerializeField]
	private bool _isJoystick = true;

	[SerializeField]
	private float _speed = 10.0f;

	[SerializeField]
	private float _rotationSpeed = 5.0f;

	[SerializeField]
	private float _maxSpeedFactor = 1.5f;
	[SerializeField]
	private float _accelerationFactor = 1.1f;

	public Boundary _boundary;

	enum InputKey {
		Horizontal,
		Vertical,
		Fire
	}

	private float getInput(InputKey key) {
		switch (key) {
		case InputKey.Horizontal: return _isJoystick ? Input.GetAxis ("JoystickHorizontal") : Input.GetAxis ("Horizontal");
		case InputKey.Vertical: return _isJoystick ? Input.GetAxis ("JoystickVertical") : Input.GetAxis ("Vertical");
		//case InputKey.Fire: return _isJoystick ? Input.GetAxis ("JoystickHorizontal") : Input.GetAxis ("Horizontal");
		default: return 0.0f;
		}
	}

	void FixedUpdate() {
		float horizontal = getInput(InputKey.Horizontal);//Input.GetAxis ("JoystickHorizontal");
		float vertical = getInput(InputKey.Vertical);//Input.GetAxis ("JoystickVertical");

		Rigidbody rb = GetComponent<Rigidbody> ();
		//Vector3 movement = new Vector3 (horizontal, 0.0f, vertical);
		//TODO fix: move towards rotation head
		Vector3 movement = new Vector3 (0.0f, 0.0f, vertical);

		rb.transform.Rotate(new Vector3(0.0f, _rotationSpeed * horizontal, 0.0f));
		rb.transform.eulerAngles = new Vector3(0.0f, rb.transform.rotation.eulerAngles.y, 0.0f);

		float currentSpeed = _speed;
		/*//accelerate if running straigt, otherwise slow down
		if (horizontal < 0.00001) {
			currentSpeed += _accelerationFactor * Time.deltaTime * currentSpeed;
		}
		else {
			currentSpeed += (1 / _accelerationFactor) * Time.deltaTime * currentSpeed;
		}*/

		//rb.velocity = rb.rotation.eulerAngles * _speed * movement;
		//Vector3 direction = rb.transform.forward * new Vector3(1, 
		rb.transform.position += transform.forward * Time.deltaTime * currentSpeed * vertical;
		rb.transform.position = Vector3.Scale(new Vector3(1.0f, 0.0f, 1.0f), rb.transform.position);

		rb.position = new Vector3(
			Mathf.Clamp (rb.position.x, _boundary.xMin, _boundary.xMax), 
			0.0f, 
			Mathf.Clamp (rb.position.z, _boundary.zMin, _boundary.zMax));

	}
}
