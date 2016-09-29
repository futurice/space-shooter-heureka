using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : Singleton<ScoreManager> {

	public class Score {
		public int score;
		public int id;
	}

	Dictionary<int, int> _scores = new Dictionary<int, int>();
	public Dictionary<int, int> Scores {
		get { return new Dictionary<int, int>(_scores); }
	}
	/*
	public List<Score> ScoresSorted {
		get { 
			_scores. 
		}
	}*/


	public void newSession() {
		_scores = new Dictionary<int, int>();
	}

	public void saveSession() {
		//TODO write to CSV / SQLite-database
	}

	public void addPoints(int playerId, int points) {
		int oldScore = 0;
		_scores.TryGetValue(playerId, out oldScore);
		_scores[playerId] = oldScore + points;
		Debug.Log(string.Format("player {0} has now points {1}", playerId, _scores[playerId])); 

		Debug.Log(_scores.ToString());
		//TODO if same player get's consecutive points, then play some audio clip / TTS?
		/*
		if (_scores.ContainsKey(playerId)) {

		}*/

	}
}
