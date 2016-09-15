using UnityEngine;
using System.Collections;

public class Collectable {
	//We could also use inheritance.. refactor if needed
	public enum CollectableType {
		SpeedUp, 
		Weapon
	}

	private CollectableType _type;
	public CollectableType Type {
		get { return _type; }
	}

	public Collectable(CollectableType type) {
		_type = type;
	}

	public static Collectable newRandomCollectable() {
		float rand = Random.value;
		CollectableType type = rand > 0.5f ? CollectableType.SpeedUp : CollectableType.Weapon;
		return new Collectable(type);
	}
}
