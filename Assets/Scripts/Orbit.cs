using UnityEngine;
using System.Collections;

public class Orbit
{
	public Vector3[] Path
	{
		get;
		protected set;
	}

	public float Duration
	{
		get;
		protected set;
	}

	public Orbit (Vector3[] path, float duration)
	{
		this.Path = path;
		this.Duration = duration;
	}
}
