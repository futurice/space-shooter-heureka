using UnityEngine;
using System.Collections;

public class WeaponLauncher : Timeoutable, Timeoutable.TimeoutListener
{
	[SerializeField]
	private GameObject 				_prefab;

	private string 					_keyCode 	= "Fire0";
	private Collectable.WeaponType 	_curWeapon 	= Collectable.WeaponType.Projectile;

	private static Transform _projectileContainer;

	private static Transform ProjectileContainer
	{
		get 
		{
			if (_projectileContainer == null)
			{
				GameObject go = new GameObject ("Projectiles");
				_projectileContainer = go.transform;
				_projectileContainer.position = Vector3.zero;
				_projectileContainer.localRotation = Quaternion.identity;
				_projectileContainer.localScale = Vector3.one;
			}

			return _projectileContainer;
		}
	}

	private PlayerController _playerController;

	private PlayerController PlayerController
	{
		get
		{
			if (_playerController == null)
			{
				_playerController = GetComponent<PlayerController> ();
			}

			return _playerController;
		}
	}

	private void Start ()
	{
		addTimeoutListener (this);
	}

	public void timeoutElapsed(Timeoutable t)
	{
		//_curWeapon = Collectable.WeaponType.None;
	}

	public override float getTimeout()
	{
		return GameConstants.WEAPON_TIMEOUT;
	}

	public void setFireKeyCode(string keycode)
	{
		_keyCode = keycode;
	}

	public void addWeapon(Collectable collectable)
	{
		//TODO allow possibly multiple simultaneous weapons, based on type
		_curWeapon = collectable.Weapon;
		reset();
	}

	private void Update ()
	{
		base.Update();
		bool pressed = Input.GetButtonDown(_keyCode);

		if (pressed && _curWeapon != Collectable.WeaponType.None)
		{
			GameObject projectile = Instantiate (_prefab, ProjectileContainer) as GameObject;
			projectile.GetComponent<ProjectileBehaviour> ().Init (PlayerController.Id, PlayerController.PlayerInformation.Color);

			//TODO Select prefab based on weapontype
			Collider shipCollider = GetComponentInChildren<Collider> ();
			Vector3 shipDirection = this.transform.forward;

			//let's try to position the projectile so it's ahead. Should take the max speed of the ship into account..
			projectile.transform.localPosition = this.transform.localPosition + 2.0f * shipCollider.bounds.extents.z * shipDirection;
			projectile.transform.localRotation = this.transform.rotation;

			AudioManager.Instance.playClip(AudioManager.AppAudioClip.Shoot);
		}


	}
}
