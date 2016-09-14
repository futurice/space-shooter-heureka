using UnityEngine;
using System.Collections;

public class WeaponLauncher : MonoBehaviour {
	[SerializeField]
	GameObject _prefab;

	[SerializeField]
	string _keyCode = "JoystickFire";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		bool pressed = Input.GetButtonDown(_keyCode);
		if (pressed) {
			GameObject weapon = Instantiate(_prefab) as GameObject;
			// TODO don't destroy the shooting spaceship, this is now possible
			MeshCollider shipCollider = GetComponent<MeshCollider>();

			Vector3 shipDirection = this.transform.forward;
			//let's try to position the projectile so it's ahead. Should take the max speed of the ship into account..
			weapon.transform.localPosition = this.transform.localPosition + 3 * shipCollider.bounds.extents.z * shipDirection;
			weapon.transform.localRotation = this.transform.rotation;
		}


	}
}
