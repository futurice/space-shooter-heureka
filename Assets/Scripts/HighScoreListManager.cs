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
			GameObject go = Instantiate (_highScoreItemViewPrefab, transform, false) as GameObject;
			RectTransform rt = go.GetComponent<RectTransform> ();

			rt.anchoredPosition3D = Vector3.zero;
			rt.localScale = Vector3.one;
			rt.localRotation = Quaternion.identity;

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
		transform.DestroyChildren ();
	}
}
