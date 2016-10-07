using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameConstants {

	public class PlayerKeys {
		private int _id;
		//Keys are mapped in "Project Settings > Input", based on this convention
		public PlayerKeys(int id) {
			_id = id;
		}

		public string VerticalAxis {
			get { return "Vertical" + _id; }
		}
		public string HorizontalAxis {
			get { return "Horizontal" + _id; }
		}
		public string FireBtn {
			get { return "Fire" + _id; }
		}
		public string SpawnBtn {
			get { return "Spawn" + _id; }
		}

	}
	
	private static Dictionary<int, PlayerKeys> _keyCodeLUT = new Dictionary<int, PlayerKeys>();
	private static Dictionary<int, Vector3> _startPosLUT = new Dictionary<int, Vector3>();

	public const int NUMBER_OF_PLAYERS = 5;

	//Timeouts are in seconds
	public const float PLAYER_IDLE_TIMEOUT = 120.0f; 
	public const float COLLECTABLE_TIMEOUT = 10.0f; 
	public const float WEAPON_TIMEOUT = 10.0f; 

	public const float NUMBER_OF_ASTEROIDS = 25; 

	public const int POINTS_FOR_BONUS = 1000;
	public const int POINTS_FOR_KILL = 500;
	public const int POINTS_FOR_COLLECTABLE = 250;
	public const int POINTS_FOR_ASTEROID = 25;
	public const int POINTS_FOR_DYING = -100;
	public const int POINTS_FOR_CHILICORN = 1000;
	//Points for each second of lifetime? could increase over time?

	static GameConstants()
	{
		const float angleInc = 2.0f * Mathf.PI / NUMBER_OF_PLAYERS;

		for (int id = 0; id < NUMBER_OF_PLAYERS; id++)
		{
			_keyCodeLUT.Add (id, new PlayerKeys (id));
			//create positions on unit circle in 2d space
			Vector3 startPos = new Vector3 (Mathf.Cos((id-1) * angleInc), 0.0f, Mathf.Sin((id-1) * angleInc));
			_startPosLUT.Add (id, startPos);
		}
	}

	public static PlayerKeys getPlayerKeys (int id)
	{
		return _keyCodeLUT[id];
	}

	/**
	 * return unique direction on unit circle to place the player around some point (f.ex planet)
	 * */
	public static Vector3 getPlayerStartDirection (int id)
	{
		return _startPosLUT[id];
	}

}
