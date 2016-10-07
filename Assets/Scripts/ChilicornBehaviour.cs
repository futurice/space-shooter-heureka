using UnityEngine;
using System.Collections;

public class ChilicornBehaviour : MonoBehaviour
{
	[SerializeField]
	private int _hp = 5;

	private void OnTriggerEnter (Collider other)
	{
		if (other.CompareTag ("projectile"))
		{
			_hp--;
			int sourceId = other.GetComponent<ProjectileBehaviour>().SourceId;

			if (_hp <= 0)
			{
				ScoreManager.Instance.addPoints (sourceId, GameConstants.POINTS_FOR_CHILICORN);
				BonusSpawnerBehaviour bonusSpawner = GetComponent<BonusSpawnerBehaviour> ();

				if (bonusSpawner)
				{
					bonusSpawner.spawnBonusOnDestroy (this.gameObject.transform.position);	
				}

				GameManager.Instance.DestroyWithExplosion (this.gameObject, sourceId);
			}

			GameManager.Instance.DestroyWithExplosion (other.gameObject, sourceId);
		}
		else if (other.CompareTag ("spaceship"))
		{
			SpaceShipController spaceShip = other.GetComponent<SpaceShipController> ();
			PlayerController playerController = spaceShip.Player;
			GameManager.Instance.DestroyWithExplosion (playerController.gameObject, playerController.Id);
		}
		else if (other.CompareTag ("asteroid"))
		{
			GameManager.Instance.DestroyAsteroid (other.gameObject);
		}
	}
}
