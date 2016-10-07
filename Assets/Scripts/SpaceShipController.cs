using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpaceShipController : MonoBehaviour
{
	[SerializeField]
	private GameObject _engines;

	public PlayerController Player
	{
		get;
		protected set;
	}

	public void Init (PlayerController player)
	{
		this.Player = player;
	}

	public void SetEnginesOn (bool active)
	{
		if (_engines != null && _engines.activeSelf != active)
		{
			_engines.SetActive (active);
		}
	}
}