using UnityEngine;
using System.Collections;

public class Collectable {
	//We could also use inheritance.. refactor if needed
	//TODO Shields, SlowDown?
	public enum CollectableType {
		Empty,
		SpeedUp, 
		Weapon,
		Enlarge,
		BonusPoints
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

	public int Points {
		get { 
			if (_type == CollectableType.BonusPoints)
				return GameConstants.POINTS_FOR_BONUS;
			else 
				return GameConstants.POINTS_FOR_COLLECTABLE;
		}
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
		CollectableType type = CollectableType.Empty;
		if (rand < 0.4f) {
			type = CollectableType.SpeedUp;
		}
		else if (rand < 0.8f) {
			type = CollectableType.Weapon;
		}
		else {
			type = CollectableType.Enlarge;
		}
		WeaponType weapon = WeaponType.None;
		if (type == CollectableType.Weapon) {
			//TODO add new types of weapons
			weapon = WeaponType.Projectile;
		}
		return new Collectable(type, weapon);
	}

	public static Collectable newBonusCollectable() {
		return new Collectable(CollectableType.BonusPoints, WeaponType.None);
	}

}
