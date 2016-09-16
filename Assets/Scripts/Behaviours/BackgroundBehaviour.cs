using UnityEngine;
using System.Collections;

public class BackgroundBehaviour : MonoBehaviour {

	public float _rotationSpeed = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.gameObject.transform.Rotate(0.0f, 0.0f, _rotationSpeed * Time.deltaTime);
	}
}
