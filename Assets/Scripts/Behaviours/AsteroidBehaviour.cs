using UnityEngine;
using System.Collections;

public class AsteroidBehaviour : MonoBehaviour {
	[SerializeField]
	protected float _spinMagnitude = 5.0f;

	// Use this for initialization
	void Start ()
	{
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * _spinMagnitude;
		//place outside of bounds and calc some constant velocity, or do it in the game manager
	}

	private void OnTriggerEnter (Collider other)
	{
		if (other.CompareTag ("projectile"))
		{
			int sourceId = other.GetComponent<ProjectileBehaviour>().SourceId;
			ScoreManager.Instance.addPoints (sourceId, GameConstants.POINTS_FOR_ASTEROID);
			GameManager.Instance.DestroyAsteroid (this.gameObject, sourceId);
			Destroy (other.gameObject);
		}
		else
		{
            //some refactoring would be in place..
			if (other.CompareTag ("spaceship"))
			{
				SpaceShipController spaceShip = other.GetComponent<SpaceShipController> ();
				PlayerController playerController = spaceShip.Player;

				if (playerController.IsEnlargened)
				{
	                int sourceId = playerController.Id;
	                ScoreManager.Instance.addPoints (sourceId, GameConstants.POINTS_FOR_ASTEROID);
	                GameManager.Instance.DestroyAsteroid (this.gameObject);
				}
				else
				{
					GetComponent<Rigidbody> ().angularVelocity = Random.insideUnitSphere * _spinMagnitude;
				}
            }
            else
			{
                //give it a little spin
                GetComponent<Rigidbody> ().angularVelocity = Random.insideUnitSphere * _spinMagnitude;
            }		
        }
	}

	public virtual void DestroyMe (int playerId)
	{
		//not very good way, we could just have the code here then...
		BonusSpawnerBehaviour bonusSpawner = GetComponent<BonusSpawnerBehaviour>();

		if (bonusSpawner)
		{
			bonusSpawner.spawnBonusOnDestroy (this.gameObject.transform.position);	
		}

		GameManager.Instance.DestroyWithExplosion (this.gameObject, playerId);
	}
}
