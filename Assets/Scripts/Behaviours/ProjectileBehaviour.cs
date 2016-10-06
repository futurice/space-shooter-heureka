using UnityEngine;
using System.Collections;

public class ProjectileBehaviour : MonoBehaviour
{
	[SerializeField]
	private float 			_speed = 50.0f;
	[SerializeField]
	private TrailRenderer 	_projectileTrail;

	private int _sourceId;

	public int SourceId
	{
		get
		{
			return _sourceId;
		}
	}

	public void Init (int playerId, Color playerColor)
	{
		_sourceId = playerId;

		// Set the projectile trail material - need to copy so we don't change
		// all the projectile colors (sharing the same material)
		Color projectileColor = playerColor;
		projectileColor.a = 0.5f;

		Material material = new Material (_projectileTrail.material);
		material.SetColor ("_TintColor", projectileColor);
		_projectileTrail.material = material;
	}

	private void FixedUpdate ()
	{
		this.gameObject.transform.position += transform.forward * Time.fixedDeltaTime * _speed;
	}
}
