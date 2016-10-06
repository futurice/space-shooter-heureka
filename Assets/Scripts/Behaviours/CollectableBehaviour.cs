using UnityEngine;
using System.Collections;

public class CollectableBehaviour : Timeoutable {

	private Collectable _collectable;

	//naming just sucks..
	public enum ReceivedCollectable {
		Random,
		Bonus
	}
	[SerializeField]
	ReceivedCollectable _receivableType = ReceivedCollectable.Random;

	public ReceivedCollectable ReceivableType {
		set { _receivableType = value; }
	}

	// Use this for initialization
	void Start () {
		//TODO change sprite based on the type?
		if (_receivableType == ReceivedCollectable.Random) {
			_collectable = Collectable.newRandomCollectable();
		}
		else {
			_collectable = Collectable.newBonusCollectable();
		}
	}

	public override float getTimeout() {
		return GameConstants.COLLECTABLE_TIMEOUT;
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "spaceship") {
			//This code could also be in PlayerController.. refactor if needed
			PlayerController ctrl = other.GetComponent<PlayerController>();
			ctrl.addCollectable(_collectable);
			Destroy(this.gameObject);
			PlaySoundFX();
		}
		else if (other.tag == "planet") {
			//destroy just so that it doesn't stay within the planet looking stupid.
			//spawning these should however check that this doesn't happen, but leave this just in case
			Destroy(this.gameObject);
		}
	}

	private void PlaySoundFX ()
	{
		switch (_collectable.Type)
		{
			case Collectable.CollectableType.SpeedUp: AudioManager.Instance.playClip(AudioManager.AppAudioClip.AcquireSpeedup); 
				break;
			case Collectable.CollectableType.Weapon: AudioManager.Instance.playClip(AudioManager.AppAudioClip.AcquireWeapon); 
				break;
			case Collectable.CollectableType.Enlarge: AudioManager.Instance.playClip (AudioManager.AppAudioClip.AcquireEnlarge);
				break;
		}
	}
}
