using UnityEngine;
using System.Collections;

public class FallingAsteroidBehaviour : AsteroidBehaviour
{
	private bool _animate = false;
	private float _timeAcc = 0.0f;
	private float _scaleFactorPerFrame = 1.022f;//TODO calc from animation length 

	private const float ANIMATION_LENGTH = 2.5f;

	public override void DestroyMe (int playerId =-1)
	{
		_animate = true;
		//give a little spin
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * 3 * _spinMagnitude;

        AudioManager.Instance.playClip(AudioManager.AppAudioClip.AsteroidRumble);
		//destroy the collider, since we're only animating at this point
		Destroy(this.GetComponent<Collider>());
	}

	// Update is called once per frame
	void Update () {
		if (_animate) {
			_timeAcc += Time.deltaTime;
			if (_timeAcc > ANIMATION_LENGTH) {
				Destroy(this.gameObject);
			}
			//since we have orthographic projection we need to animate scale
			//with perspective projection we would just move it towards the camera
			this.transform.localScale = _scaleFactorPerFrame * this.transform.localScale;
		}
	}
}
