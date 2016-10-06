using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Crosstales.RTVoice.Model;
using Crosstales.RTVoice;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private GameObject 	_audioSourcePrefab 		= null;

	[Header("Text To Speech (TTS")]
	[SerializeField]
	private GameObject 	_audioSourceTTSPrefab 	= null;
	[SerializeField]
	private Transform	_ttsContainer			= null;
	[SerializeField]
	private Voice 		_speakerVoice;

    private List<AudioSource> _currentlyPlaying = new List<AudioSource>();

	public enum AppAudioClip
	{
		Explosion,
		AcquireWeapon,
		AcquireSpeedup,
		AcquireEnlarge,
		Shoot
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

		switch (clip)
		{
			case AppAudioClip.Explosion: 		return "Audio/explosion_player";
			case AppAudioClip.AcquireWeapon: 	return "Audio/pick_up_1";
			case AppAudioClip.AcquireSpeedup: 	return "Audio/pick_up_2";
			case AppAudioClip.AcquireEnlarge: 	return "Audio/pick_up_3";
			case AppAudioClip.Shoot: 			return "Audio/weapon_enemy";
		}

		return path;
    }

    protected AudioManager() {}


    public void playClip(AppAudioClip clip) {
        if (_muted) {
            Debug.Log("AudioManager Muted, not playing anything");
            return;
        }
		//Debug.Log("AudioManager play clip " + clip);

        string path = ClipPath(clip);
		Debug.Assert(path != null);

		if (path != null) {
			//AudioClip playMe = (AudioClip)Resources.Load(path, typeof(AudioClip));//Resources.Load(path) as AudioClip;
			AudioClip playMe = Resources.Load<AudioClip>(path);
			Debug.Assert(playMe != null);
			if (playMe != null) {
				//Debug.Log("AudioManager clip loaded " + playMe);

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

	public void speak(string speech) {

		// Silence all existing speakers - we are polite no talking on top of
		// our selves
		silence ();

		bool isNative = false;
		float rate = 1.0f;
		float vol = 1.0f;
		float pitch = 1.0f;
		if (isNative)
		{
			//Speaker.SpeakNative(speech, _speakerVoice, rate, vol, pitch);
		}
		else
		{
			// Create a new speaker
			GameObject audioSource = Instantiate (_audioSourceTTSPrefab, _ttsContainer) as GameObject;
			AudioSource source = audioSource.GetComponent<AudioSource> ();
			Speaker.Speak (speech, source, _speakerVoice, true, rate, vol, "", pitch);
		}
	}

	public void silence() {
		Speaker.Silence ();
		_ttsContainer.DestroyChildren ();
	}
}
