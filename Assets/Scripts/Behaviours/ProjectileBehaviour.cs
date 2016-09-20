using UnityEngine;
using System.Collections;

public class ProjectileBehaviour : MonoBehaviour {

	[SerializeField]
	private float _speed = 50.0f;

	private GameObject _source;
	public GameObject Source {
		set { _source = value; }
		get { return _source; }
	}

	// Update is called once per frame
	void Update () {
		this.gameObject.transform.position += transform.forward * Time.deltaTime * _speed;
	}
}
