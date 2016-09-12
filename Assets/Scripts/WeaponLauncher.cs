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
			// TODO destroy once out of scope
			weapon.transform.localPosition = this.transform.localPosition;
			weapon.transform.localRotation = this.transform.rotation;
		}


	}
}
