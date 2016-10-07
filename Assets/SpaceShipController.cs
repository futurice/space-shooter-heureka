using UnityEngine;
using System.Collections;

public class SpaceShipController : MonoBehaviour
{
	public PlayerController Player
	{
		get;
		protected set;
	}

	public void Init (PlayerController player)
	{
		this.Player = player;
	}
}
