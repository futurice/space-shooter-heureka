using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class PlayerController : Timeoutable
{
	[SerializeField]
	private float 						_speed = 10.0f;
	[SerializeField]
	private float 						_rotationSpeed = 5.0f;
	[SerializeField]
	private Transform 					_transformLimits;
	[SerializeField]
	private PlayerColorIndicator		_colorIndicator;
	[SerializeField]
	private Transform					_shipContainer;
	[SerializeField]
	private Text						_scoreText;

	private GameConstants.PlayerKeys 	_keys = new GameConstants.PlayerKeys (0);
    private List<Collectable> 			_collectables = new List<Collectable> ();//TODO add timestamp, so we can 
	private bool 						_hasInput = false;
	private SpaceShipController			_playerShip;

	private int	_id = 0;

	public int Id
	{
		set
		{
			_id = value;
		}

		get
		{
			return _id;
		}
	}

	private PlayerInformation _playerInformation;

	public PlayerInformation PlayerInformation
	{
		get
		{
			return _playerInformation;
		}

		private set
		{ 
			_playerInformation = value;

			// Set color indicator color
			if (_colorIndicator != null)
			{
				_colorIndicator.Color = value.Color;
			}

			// Set score text color
			if (_scoreText != null)
			{
				_scoreText.color = new Color (value.Color.r, value.Color.g, value.Color.b, 0.5f);
			}
		}
	}

    private bool _isEnlargened = false;

    public bool IsEnlargened
	{
        get
		{
			return _isEnlargened;
		}
    }

	private Rigidbody _rigidbody;

	private Rigidbody Rigidbody
	{
		get
		{
			if (_rigidbody == null)
			{
				_rigidbody = GetComponent<Rigidbody> ();
			}

			return _rigidbody;
		}
	}

	private Collider _shipCollider;

	private Collider ShipCollider
	{
		get
		{
			if (_shipCollider == null && _playerShip != null)
			{
				_shipCollider = _playerShip.GetComponent<Collider> ();
			}

			return _shipCollider;
		}
	}

	public void Init (int id, GameConstants.PlayerKeys keys, PlayerInformation playerInformation)
	{
		this.Id = id;
		SetPlayerKeys (keys);
		this.PlayerInformation = playerInformation;

		GameObject go = Instantiate (playerInformation.PlayerShipPrefab, _shipContainer, false) as GameObject;
		_playerShip = go.GetComponent<SpaceShipController> ();
		_playerShip.Init (this);

		_playerShip.OnTriggerEnterAsObservable ().Subscribe (collider => {
			OnPlayerShipTriggerEnter (collider);
		});
	}

	private void SetPlayerKeys (GameConstants.PlayerKeys keys)
	{
		_keys = keys;
	}

	public override float GetTimeout() {
		return GameConstants.PLAYER_IDLE_TIMEOUT;
	}
	
	public override bool shouldReset() {
		return _hasInput;
	}

	public void addCollectable(Collectable collectable) {
		//TODO use these on update / input, etc
		//TODO remove it automatically after timeout, run out of ammo, etc
		Debug.Log (string.Format ("SpaceShip {0}: Adding collectable of type: {1} + points {2}", Id, collectable.Type, collectable.Points));
		_collectables.Add(collectable);
		ScoreManager.Instance.addPoints(Id, collectable.Points);

        if (collectable.Type == Collectable.CollectableType.Weapon) {
			GetComponent<WeaponLauncher>().addWeapon(collectable);
		}
		else if (collectable.Type == Collectable.CollectableType.Enlarge) {
            enlarge();
		}
		else if (collectable.Type == Collectable.CollectableType.SpeedUp) {
			InsultManager.Instance.insultAboutSpeed(Id, _collectables); 
		}
	}

    private void enlarge() {
        if (_isEnlargened) {
            Debug.Log(string.Format("Ship {0} is already enlargened, won't do it twice", Id));
        }
        else {
            _isEnlargened = true;
            //TODO tween
            Vector3 curScale = this.gameObject.transform.localScale;
            this.gameObject.transform.localScale = 2.5f * curScale;
        }
    }

	public override void Update()
	{
		base.Update();

		// Update the score text
		int points = ScoreManager.Instance.GetPointsForPlayer (Id);

		if (_scoreText != null)
		{
			_scoreText.text = string.Format ("{0}{1} PTS", points < 0 ? string.Empty : "+", points);
		}
	}

	private void FixedUpdate () 
	{
		float horizontal = Input.GetAxis(_keys.HorizontalAxis);
		float vertical = Input.GetAxis(_keys.VerticalAxis);
		_hasInput = horizontal > 0.00001f || vertical > 0.000001f;
		_playerShip.SetEnginesOn (_hasInput);

		Rigidbody rb = Rigidbody;

		float speedFactor = GetSpeedFactor();
		float rotationSpeed = speedFactor * _rotationSpeed; 
		float currentSpeed = speedFactor * _speed;

		Quaternion rotation = transform.rotation * Quaternion.Euler (new Vector3(0.0f, rotationSpeed * horizontal, 0.0f));
		rb.MoveRotation (rotation);

		//Vector3 movement = new Vector3 (horizontal, 0.0f, vertical);
		//TODO fix: move towards rotation head
		//Vector3 movement = new Vector3 (0.0f, 0.0f, vertical);
		//let's remove the velocities in all other directions as the movement direction
		//rb.velocity = new Vector3(0.0f, 0.0f, rb.velocity.z);

		Vector3 position = rb.position;
		position += transform.forward * Time.deltaTime * currentSpeed * vertical;
		position = Vector3.Scale (new Vector3 (1.0f, 0.0f, 1.0f), position);

		// Limit the position to stay inside the game area. not 100% mathematically correct but good enough
		if (ShipCollider != null)
		{
			Vector3 shipExtents = 1.1f * ShipCollider.bounds.extents;//works without the factor, but put it just-in-case
		
			float xMin = -1*(_transformLimits.localScale.x / 2) + shipExtents.x;
			float xMax = (_transformLimits.localScale.x / 2) - shipExtents.x;
			float zMin = -1*(_transformLimits.localScale.z / 2) + shipExtents.z;
			float zMax = (_transformLimits.localScale.z / 2) - shipExtents.z;
			position = new Vector3(
				Mathf.Clamp (position.x, xMin, xMax), 
				0.0f, 
				Mathf.Clamp (position.z, zMin, zMax));
		}

		rb.MovePosition (position);
	}

	public float thrust = 3.0f;

	private float GetSpeedFactor ()
	{
		float speed = 1.0f;

		foreach (Collectable c in _collectables)
		{
			speed *= c.SpeedUpFactor;
		}
		return speed;
	}

	private void OnPlayerShipTriggerEnter (Collider other)
	{
		if (other.CompareTag ("projectile"))
		{
			ProjectileBehaviour projectile = other.gameObject.GetComponent<ProjectileBehaviour>();

			// If the projectile was from another player - destroy our ship
			if (projectile.SourceId != Id)
			{
				int sourceId = projectile.SourceId;
				ScoreManager.Instance.addPoints (sourceId, GameConstants.POINTS_FOR_KILL);

				Destroy (other.gameObject);
				GameManager.Instance.DestroyWithExplosion (this.gameObject, Id);
			}
		}
		else if (other.CompareTag ("spaceship"))
		{
			SpaceShipController otherSpaceShip = other.gameObject.GetComponent<SpaceShipController> ();
			PlayerController otherPlayer = otherSpaceShip.Player;

            bool thisEnlargend = IsEnlargened;
            bool thatEnlargend = otherPlayer.IsEnlargened;
            bool both = thisEnlargend && thatEnlargend;
            bool neither = !thisEnlargend && !thatEnlargend;

            if (both || neither)
			{
                GameManager.Instance.DestroyWithExplosion (this.gameObject);
				GameManager.Instance.DestroyWithExplosion (otherPlayer.gameObject, true, false);
            }
            else if (thisEnlargend)
			{
				GameManager.Instance.DestroyWithExplosion (otherPlayer.gameObject, true, true);
                ScoreManager.Instance.addPoints (Id, GameConstants.POINTS_FOR_KILL);
            }
            else if (thatEnlargend)
			{
                GameManager.Instance.DestroyWithExplosion (this.gameObject);
                ScoreManager.Instance.addPoints (otherPlayer.Id, GameConstants.POINTS_FOR_KILL);
            }
            else
			{
                Debug.LogError ("BUG IN PLAYERCONTROLLER");
                Debug.Assert(true);
            }
        }
	}

	public bool HasInput ()
	{
		return _hasInput;
	}
}
