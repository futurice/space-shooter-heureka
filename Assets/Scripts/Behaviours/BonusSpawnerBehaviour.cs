using UnityEngine;
using System.Collections;

/**
 * This behaviour will possibly spawn bonus items.
 * */
public class BonusSpawnerBehaviour : MonoBehaviour
{
	[SerializeField]
	private GameObject 	_bonusPrefab;
	[SerializeField]
	private float 		_chance 	= 0.5f;

	private bool 		_hasBonus 	= true;

	private void Start ()
	{
		_hasBonus = Random.value > 1.0f - _chance;
	}

	public bool spawnBonusOnDestroy (Vector3 spot)
	{
		if (_hasBonus)
		{
			GameObject bonus = Instantiate(_bonusPrefab) as GameObject;
			bonus.transform.position = spot;
			CollectableBehaviour collectable = bonus.GetComponent<CollectableBehaviour>();

			if (collectable)
			{
				//not quite loosely coupled this is.. TODO better
				collectable.ReceivableType = CollectableBehaviour.ReceivedCollectable.Bonus;

			}

			return true;
		}

		return false;
	}

}
