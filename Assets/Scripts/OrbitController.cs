using UnityEngine;
using System.Collections;

public class OrbitController : MonoBehaviour
{
	private Vector3[] _orbitPath;

	public Vector3[] OrbitPath
	{
		get
		{
			if (_orbitPath == null)
			{
				_orbitPath = new Vector3[transform.childCount];
				int i = 0;

				foreach (Transform child in transform)
				{
					_orbitPath[i++] = child.position;
				}
			}

			return _orbitPath;
		}
	}
}
