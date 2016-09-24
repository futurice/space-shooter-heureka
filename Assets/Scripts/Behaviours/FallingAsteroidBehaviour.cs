using UnityEngine;
using System.Collections;

public class FallingAsteroidBehaviour : AsteroidBehaviour {

	private bool _animate = false;
	private float _timeAcc = 0.0f;
	private float _scaleFactorPerFrame = 1.025f;//TODO calc from animation length 

	private const float ANIMATION_LENGTH = 3.0f;

	protected override void destroyMe()
	{
		_animate = true;
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
			//TODO also increase Y, so that something can happen underneath the falling particle 
			//(otherwise it's collider still gets the events)
			this.transform.localScale = _scaleFactorPerFrame * this.transform.localScale;
		}
	}
}
