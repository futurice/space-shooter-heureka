using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighScoreItemView : MonoBehaviour
{
	[SerializeField]
	private Image _playerShipSilhouetteImage 	= null;
	[SerializeField]
	private Text _playerNameText 				= null;
	[SerializeField]
	private Text _playerScoreText 				= null;

	public void Init (int playerId, int score)
	{
		PlayerInformation playerInformation = PlayerInformationManager.Instance.GetPlayerInformation (playerId);

		// Set the image sprite
		_playerShipSilhouetteImage.sprite = playerInformation.ShipSilhouetteSprite;

		// Set the player name
		_playerNameText.text = playerInformation.Name;

		// Set the player score
		if (score < 0)
		{
			_playerScoreText.text = string.Format ("-{0}", score);
		}
		else
		{
			_playerScoreText.text = string.Format ("+{0}", score);
		}

		// Apply colors to the texts and the sprite
		_playerShipSilhouetteImage.color = playerInformation.Color;
		_playerNameText.color = playerInformation.Color;
		_playerScoreText.color = playerInformation.Color;
	}
}
