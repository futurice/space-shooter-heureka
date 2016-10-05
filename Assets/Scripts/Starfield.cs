using UnityEngine;
using System.Collections;

public class Starfield : MonoBehaviour 
{
	[SerializeField]
	private Material 	_starsMaterial;
	[SerializeField]
	private float		_speed					= 1.0f;
	[SerializeField]
	private float 		_smallStarsDistance  	= 5000;
	[SerializeField]
	private float 		_mediumStarsDistance 	= 2500;
	[SerializeField]
	private float 		_bigStarsDistance    	= 1000;

	private Vector2 	_position				= Vector2.zero;

	private void LateUpdate ()
	{
		_position.x += _speed;

		_starsMaterial.SetTextureOffset("_SmallStars" , new Vector2(_position.x / _smallStarsDistance, 	_position.y / _smallStarsDistance));
		_starsMaterial.SetTextureOffset("_MediumStars", new Vector2(_position.x / _mediumStarsDistance, _position.y / _mediumStarsDistance));
		_starsMaterial.SetTextureOffset("_BigStars"   , new Vector2(_position.x / _bigStarsDistance, 	_position.y / _bigStarsDistance));
	}
}