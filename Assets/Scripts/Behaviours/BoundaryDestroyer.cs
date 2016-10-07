using UnityEngine;
using System.Collections;

public class BoundaryDestroyer : MonoBehaviour
{
	private void OnTriggerExit (Collider other)
	{
		if (other.CompareTag ("asteroid"))
		{
			GameManager.Instance.DestroyAsteroid (other.gameObject);
		}
		else
		{
			Destroy(other.gameObject);
		}
	}
}
