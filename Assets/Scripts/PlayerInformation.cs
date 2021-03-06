﻿using UnityEngine;

public class PlayerInformation
{
	public string Name
	{
		get;
		protected set;
	}

	public Color Color
	{
		get;
		protected set;
	}

	protected string PlayerShipResourcePath
	{
		get;
		set;
	}

	protected string SpriteResourcePath
	{
		get;
		set;
	}

	protected string ExplosionResourcePath
	{
		get;
		set;
	}

	private GameObject _playerShipPrefab;

	public GameObject PlayerShipPrefab
	{
		get
		{
			if (_playerShipPrefab == null)
			{
				// Load the player ship prefab from resources
				_playerShipPrefab = Resources.Load<GameObject> (PlayerShipResourcePath);
			}

			return _playerShipPrefab;
		}
	}

	private Sprite _shipSilhouetteSprite;

	public Sprite ShipSilhouetteSprite
	{
		get
		{
			if (_shipSilhouetteSprite == null)
			{
				// Load the sprite from resources
				_shipSilhouetteSprite = Resources.Load<Sprite> (SpriteResourcePath);
			}

			return _shipSilhouetteSprite;
		}
	}

	private GameObject _explosionPrefab;

	public GameObject ExplosionPrefab
	{
		get
		{
			if (_explosionPrefab == null)
			{
				// Load the projectile explosion prefab from resources
				_explosionPrefab = Resources.Load<GameObject> (ExplosionResourcePath);
			}

			return _explosionPrefab;
		}
	}

	public PlayerInformation (string name, Color color, string playerShipResourcePath, string spriteResourcePath, string explosionResourcePath)
	{
		this.Name = name;
		this.Color = color;
		this.PlayerShipResourcePath = playerShipResourcePath;
		this.SpriteResourcePath = spriteResourcePath;
		this.ExplosionResourcePath = explosionResourcePath;
	}
}
