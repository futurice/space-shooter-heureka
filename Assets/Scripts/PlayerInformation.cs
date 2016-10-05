using UnityEngine;

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

	protected string ResourcePath
	{
		get;
		set;
	}

	private Sprite _shipSilhouetteSprite;

	public Sprite ShipSilhouetteSprite
	{
		get
		{
			if (_shipSilhouetteSprite == null)
			{
				// Load the sprite from resources
				_shipSilhouetteSprite = Resources.Load<Sprite> (ResourcePath);
			}

			return _shipSilhouetteSprite;
		}
	}

	public PlayerInformation (string name, Color color, string resourcePath)
	{
		this.Name = name;
		this.Color = color;
		this.ResourcePath = resourcePath;
	}
}
