using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInformationManager : Singleton<PlayerInformationManager>
{
	Dictionary<int, PlayerInformation> _playerInformationMap = new Dictionary<int, PlayerInformation> ()
	{
		{0, new PlayerInformation ("SPACE ROCKET", 		new Color (255.0f/255.0f, 72.0f/255.0f, 140.0f/255.0f), 	"Prefabs/Player/0",	"Sprites/PlayerShipSilhouettes/0", "Prefabs/PlayerExplosions/explosion_projectile_0")},
		{1, new PlayerInformation ("SPACE DONUT", 		new Color (0.0f/255.0f, 249.0f/255.0f, 255.0f/255.0f), 		"Prefabs/Player/1", "Sprites/PlayerShipSilhouettes/1", "Prefabs/PlayerExplosions/explosion_projectile_1")},
		{2, new PlayerInformation ("SPACE IN-VADER", 	new Color (171.0f/255.0f, 60.0f/255.0f, 247.0f/255.0f), 	"Prefabs/Player/2", "Sprites/PlayerShipSilhouettes/2", "Prefabs/PlayerExplosions/explosion_projectile_2")},
		{3, new PlayerInformation ("SPACE EGGS", 		new Color (118.0f/255.0f, 187.0f/255.0f, 44.0f/255.0f), 	"Prefabs/Player/3", "Sprites/PlayerShipSilhouettes/3", "Prefabs/PlayerExplosions/explosion_projectile_3")},
		{4, new PlayerInformation ("XYZ-WING", 			new Color (255.0f/255.0f, 130.0f/255.0f, 126.0f/255.0f), 	"Prefabs/Player/4", "Sprites/PlayerShipSilhouettes/4", "Prefabs/PlayerExplosions/explosion_projectile_4")},
		{5, new PlayerInformation ("SPACE DOG LAIKA", 	new Color (5.0f/255.0f, 133.0f/255.0f, 209.0f/255.0f), 		"Prefabs/Player/5", "Sprites/PlayerShipSilhouettes/5", "Prefabs/PlayerExplosions/explosion_projectile_5")},
		{6, new PlayerInformation ("MISTER TOM", 		new Color (235.0f/255.0f, 224.0f/255.0f, 20.0f/255.0f), 	"Prefabs/Player/6", "Sprites/PlayerShipSilhouettes/6", "Prefabs/PlayerExplosions/explosion_projectile_6")},
		{7, new PlayerInformation ("FUTUNAUT", 			new Color (0.0f/255.0f, 142.0f/255.0f, 14.0f/255.0f), 		"Prefabs/Player/7", "Sprites/PlayerShipSilhouettes/7", "Prefabs/PlayerExplosions/explosion_projectile_7")}
	};

	public PlayerInformation GetPlayerInformation (int id)
	{
		int defaultId = 7;
		PlayerInformation playerInformation = null;

		if (!_playerInformationMap.TryGetValue (id, out playerInformation))
		{
			Debug.LogErrorFormat ("PlayerInformationManager GetPlayerInformation: Could not find player information for id: {0} - defaulting to {1}", defaultId);
			return _playerInformationMap[defaultId];
		}

		return playerInformation;
	}
}
