using UnityEngine;
using System.Collections;

public class WeaponLauncher : MonoBehaviour {
	[SerializeField]
	GameObject _prefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		bool pressed = Input.GetButtonDown("JoystickFire");
		if (pressed) {
			GameObject weapon = Instantiate(_prefab) as GameObject;

			weapon.transform.localPosition = this.transform.localPosition;
			weapon.transform.localRotation = this.transform.rotation;
		}


	}
}
