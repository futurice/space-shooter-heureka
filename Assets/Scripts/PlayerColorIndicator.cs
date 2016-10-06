using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PlayerColorIndicator : MonoBehaviour {

	[SerializeField]
	private SpriteRenderer 	_sprite;
	[SerializeField]
	private float			_minAlpha;
	[SerializeField]
	private float			_maxAlpha;
	[SerializeField]
	private float			_fadeAnimDuration;

	private Sequence		_sequence;

	public Color Color
	{
		get
		{
			return new Color (_sprite.color.r, _sprite.color.g, _sprite.color.b);
		}

		set
		{
			_sprite.color = new Color (value.r, value.g, value.b, _sprite.color.a);
		}
	}

	private void Start ()
	{
		_sequence = DOTween.Sequence ();
		_sequence.Append (_sprite.DOFade (_minAlpha, _fadeAnimDuration/2.0f).SetEase (Ease.Linear));
		_sequence.Append (_sprite.DOFade (_maxAlpha, _fadeAnimDuration/2.0f).SetEase (Ease.Linear));
		_sequence.SetLoops (-1, LoopType.Yoyo);
		_sequence.Play ();
	}
}
