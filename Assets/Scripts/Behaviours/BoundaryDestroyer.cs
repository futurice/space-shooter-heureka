using UnityEngine;
using System.Collections;

public class BoundaryDestroyer : MonoBehaviour {
	void OnTriggerExit(Collider other) {
		Destroy(other.gameObject);
	}
}
