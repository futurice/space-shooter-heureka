using UnityEngine;
using System.Collections;

public class Done_DestroyByBoundary : MonoBehaviour
{
	private void OnTriggerExit (Collider other) 
	{
		Destroy(other.gameObject);
	}
}