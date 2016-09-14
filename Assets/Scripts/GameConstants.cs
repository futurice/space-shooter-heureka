using System.Collections;
using System.Collections.Generic;

public class GameConstants {

	public class PlayerKeys {
		private int _id;
		//Keys are mapped in InputManager, based on this convention
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
	}
	
	private static Dictionary<int, PlayerKeys> _keyCodeLUT;

	public const int NUMBER_OF_PLAYERS = 2;

	static GameConstants() {
		_keyCodeLUT = new Dictionary<int, PlayerKeys>();
		for (int id = 1; id < NUMBER_OF_PLAYERS + 1; id++) {
			_keyCodeLUT.Add(id, new PlayerKeys(id));
		}
	}

	public static PlayerKeys getPlayerKeys(int id) {
		return _keyCodeLUT[id];
	}
}
