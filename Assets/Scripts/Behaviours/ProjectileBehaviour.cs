using UnityEngine;
using System.Collections;

public class ProjectileBehaviour : MonoBehaviour {

	[SerializeField]
	private float _speed = 50.0f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		this.gameObject.transform.position += transform.forward * Time.deltaTime * _speed;
	}
}
