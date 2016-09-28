using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InsultManager : Singleton<InsultManager> {
	private int _lastKIA = 0;

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
			AudioManager.Instance.speak(string.Format("Player {0}, you fly like a space monkey.", id));
			//this guy died twice in a row so he deserves an insult
		}
		_lastKIA = id;
	}
}
