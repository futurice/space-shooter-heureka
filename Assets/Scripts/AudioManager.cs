using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager> {

    [SerializeField]
    private GameObject _audioSourcePrefab = null;

    private List<AudioSource> _currentlyPlaying = new List<AudioSource>();

	public enum AppAudioClip {
		Explosion
	}

    private bool _muted = false;
    public bool Muted {
        set {
            _muted = value;
        }
        get {
            return _muted;
        }
    }

    private string ClipPath(AppAudioClip clip) {

        string path = null;
        if (clip == AppAudioClip.Explosion) {
			path = "Audio/explosion_player";
        }
        return path;
    }

    protected AudioManager() {}


    public void playClip(AppAudioClip clip) {
        if (_muted) {
            Debug.Log("AudioManager Muted, not playing anything");
            return;
        }

        string path = ClipPath(clip);
		Debug.Assert(path != null);

		if (path != null) {
			//AudioClip playMe = (AudioClip)Resources.Load(path, typeof(AudioClip));//Resources.Load(path) as AudioClip;
			AudioClip playMe = Resources.Load<AudioClip>(path);
			Debug.Assert(playMe != null);
			if (playMe != null) {
				//we have to use separate audiosources per clip, for polyphony
				GameObject audioSource = Instantiate(_audioSourcePrefab) as GameObject;
				audioSource.transform.SetParent(this.transform);
				
				
				AudioSource source = audioSource.GetComponent<AudioSource>();
				Debug.Assert(source != null);
				_currentlyPlaying.Add(source);
				
				source.clip = playMe;
				
				source.Play();//or PlayOneShot
				
				//cleanup after finished
				float timeInSecs = playMe.length;
				Debug.Assert (timeInSecs > 0.0f);
				if (timeInSecs <= 0.0f) {
					//some safety programming. important is that the clip get's cleaned up at some point
					Debug.LogWarning("[AudioManager]: clip length reported " + timeInSecs);
					timeInSecs = 10.0f;
				}
				StartCoroutine(cleanUpFinished(audioSource, timeInSecs));
			}
        }

    }

    private IEnumerator cleanUpFinished(GameObject source, float secs) {
        yield return new WaitForSeconds(secs); 
        Destroy(source);
    }


}
