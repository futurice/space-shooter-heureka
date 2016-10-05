using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInformationManager : Singleton<PlayerInformationManager>
{
	Dictionary<int, PlayerInformation> _playerInformationMap = new Dictionary<int, PlayerInformation> ()
	{
		{0, new PlayerInformation ("SPACE ROCKET", 		new Color (255.0f/255.0f, 120.0f/255.0f, 173.0f/255.0f), 	"Sprites/PlayerShipSilhouettes/0")},
		{1, new PlayerInformation ("SPACE DONUT", 		new Color (30.0f/255.0f, 214.0f/255.0f, 212.0f/255.0f), 	"Sprites/PlayerShipSilhouettes/1")},
		{2, new PlayerInformation ("SPACE IN-VADER", 	new Color (134.0f/255.0f, 79.0f/255.0f, 166.0f/255.0f), 	"Sprites/PlayerShipSilhouettes/2")},
		{3, new PlayerInformation ("SPACE EGGS", 		new Color (82.0f/255.0f, 124.0f/255.0f, 38.0f/255.0f), 		"Sprites/PlayerShipSilhouettes/3")},
		{4, new PlayerInformation ("X-WING", 			new Color (175.0f/255.0f, 80.0f/255.0f, 137.0f/255.0f), 	"Sprites/PlayerShipSilhouettes/4")},
		{5, new PlayerInformation ("SPACE DOG LAIKA", 	new Color (0.0f/255.0f, 129.0f/255.0f, 213.0f/255.0f), 		"Sprites/PlayerShipSilhouettes/5")},
		{6, new PlayerInformation ("MISTER TOM", 		new Color (246.0f/255.0f, 227.0f/255.0f, 136.0f/255.0f), 	"Sprites/PlayerShipSilhouettes/6")},
		{7, new PlayerInformation ("FUTUNAUT", 			new Color (22.0f/255.0f, 205.0f/255.0f, 135.0f/255.0f), 	"Sprites/PlayerShipSilhouettes/7")}
	};

	public PlayerInformation GetPlayerInformation (int id)
	{
		PlayerInformation playerInformation = null;

		if (!_playerInformationMap.TryGetValue (id, out playerInformation))
		{
			Debug.LogErrorFormat ("PlayerInformationManager GetPlayerInformation: Could not find player information for id: {0}", id);
		}

		return playerInformation;
	}
}
