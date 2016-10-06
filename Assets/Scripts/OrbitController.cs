using UnityEngine;
using System.Collections;

public class OrbitController : MonoBehaviour
{
	[SerializeField]
	private float 		_duration = 10.0f;
	private Orbit 		_orbit;

	public Orbit Orbit
	{
		get
		{
			if (_orbit == null)
			{
				Vector3[] orbitPath = new Vector3[transform.childCount];
				int i = 0;

				foreach (Transform child in transform)
				{
					orbitPath[i++] = child.position;
				}

				_orbit = new Orbit (orbitPath, _duration);
			}

			return _orbit;
		}
	}
}
