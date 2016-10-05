using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HighScoreListManager : Singleton<HighScoreListManager>
{
	[SerializeField]
	private CanvasGroup _canvasGroup;
	[SerializeField]
	private GameObject	_highScoreItemViewPrefab;

	public void ShowHighScores ()
	{
		DestroyHighScoreItems ();

		List<KeyValuePair<int, int>> scores = ScoreManager.Instance.ScoresSorted;

		foreach (KeyValuePair<int, int> score in scores)
		{
			GameObject go = Instantiate (_highScoreItemViewPrefab, transform) as GameObject;
			HighScoreItemView highScoreItemView = go.GetComponent <HighScoreItemView> ();
			highScoreItemView.Init (score.Key, score.Value);
		}

		_canvasGroup.alpha = 1.0f;
	}

	public void HideHighScores ()
	{
		_canvasGroup.alpha = 0.0f;
		DestroyHighScoreItems ();
	}

	private void DestroyHighScoreItems ()
	{
		foreach (Transform child in transform)
		{
			GameObject.Destroy (child.gameObject);
		}
	}
}
