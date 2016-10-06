using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Timeoutable : MonoBehaviour {
	protected float _lifetimeInSecs = 0.0f;
	private List<TimeoutListener> _listeners = new List<TimeoutListener>();

	public void addTimeoutListener(TimeoutListener l) {
		_listeners.Add(l);
	}

	public interface TimeoutListener {
		void timeoutElapsed(Timeoutable t);
	}

	public virtual void Update() {
		if (shouldReset()) {
			reset();
		}
		else {
			_lifetimeInSecs += Time.deltaTime;
		}

		if (_lifetimeInSecs >= GetTimeout()) {
			foreach(TimeoutListener l in _listeners) {
				l.timeoutElapsed(this);
			}
		}
	}

	public virtual float GetTimeout() {
		return 10.0f;
	}

	public virtual bool shouldReset() {
		return false;
	}

	public virtual void reset() {
		_lifetimeInSecs = 0.0f;
	}
}
