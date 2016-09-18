using UnityEngine;
using System.Collections;

public class Collectable {
	//We could also use inheritance.. refactor if needed
	//TODO Shields, SlowDown?
	public enum CollectableType {
		SpeedUp, 
		Weapon
	}

	//TODO machine gun, flamethrower, 
	public enum WeaponType {
		None,
		Projectile
	}

	private CollectableType _type;
	public CollectableType Type {
		get { return _type; }
	}

	private WeaponType _weapon = WeaponType.None;
	public WeaponType Weapon {
		get { return _weapon; }
	}

	public float SpeedUpFactor { 
		get {
			switch(_type) {
			case CollectableType.SpeedUp: return 1.3f;
			default: return 1.0f;
			}
		}
	}

	public Collectable(CollectableType type, WeaponType weapon) {
		_type = type;
		_weapon = weapon;
	}

	public static Collectable newRandomCollectable() {
		float rand = Random.value;
		CollectableType type = rand > 0.5f ? CollectableType.SpeedUp : CollectableType.Weapon;
		WeaponType weapon = WeaponType.None;
		if (type == CollectableType.Weapon) {
			//TODO add new types of weapons
			weapon = WeaponType.Projectile;
		}
		return new Collectable(type, weapon);
	}

}
