using UnityEngine;
using System.Collections;

public class OrbitManager : Singleton<OrbitManager>
{
	private OrbitController[] _orbits;

	private OrbitController[] Orbits
	{
		get
		{
			if (_orbits == null)
			{
				_orbits = GetComponentsInChildren<OrbitController> (true);
			}

			return _orbits;
		}
	}

	// Returns a random orbit from the orbits
	public Vector3[] GetOrbit ()
	{
		int numOrbits = Orbits.Length;

		if (numOrbits > 0)
		{
			return Orbits[UnityEngine.Random.Range (0, numOrbits)].OrbitPath;
		}

		return null;
	}
}
