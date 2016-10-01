using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TODO rename
public class InsultManager : Singleton<InsultManager> {
	
	private int _lastKIA = 0;

	private string[] _kiaInsults = new string[0];
	private int _kiaIndex = 0;

	public void readInsults() {
		string[] lines = System.IO.File.ReadAllLines( Application.streamingAssetsPath + "/insults.txt");
		if (lines != null && lines.Length > 0) {
			_kiaInsults = lines;
		}
		else {
			//let's put something
			_kiaInsults = new string[1];
			_kiaInsults[0] = "You need to wake up Player {0}.";
		}
	}

	public void tellInstructions() {
		AudioManager.Instance.speak("Listen up cadets, I'm not gonna tell this twice.");
	}
	
	public void tellIntro() {
		AudioManager.Instance.speak("Welcome Space Cadets! This is Admiral Marcus, Commander of the USS Futurice. Get ready and board your ships.");	
	}
	
	public void insultAboutSpeed(int id, List<Collectable> collectables) {
		//Just for laughs
		int count = 0;
		foreach (Collectable c in collectables) {
			if (c.Type == Collectable.CollectableType.SpeedUp)
				count++;
		}           
		if (count == 4) {
			AudioManager.Instance.speak(string.Format("Player {0}, you're going too fast!", id));
		}
		else if (count == 5) {
			AudioManager.Instance.speak(string.Format("Player {0}, slow down!", id));
		}
	}

	public void playerDied(int id) {
		if (_lastKIA == id) {
			AudioManager.Instance.speak(string.Format(_kiaInsults[_kiaIndex++], id));
			if (_kiaIndex >= _kiaInsults.Length) {
				_kiaIndex = 0;
			}
			//this guy died twice in a row so he deserves an insult
		}
		_lastKIA = id;
	}
}
