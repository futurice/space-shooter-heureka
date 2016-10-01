using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : Singleton<ScoreManager> {

	Dictionary<int, int> _scores = new Dictionary<int, int>();
	public Dictionary<int, int> Scores {
		get { return new Dictionary<int, int>(_scores); }
	}

	public List<KeyValuePair<int, int>> ScoresSorted {
		get { 
			List<KeyValuePair<int, int>> scores = new List<KeyValuePair<int, int>>();
			foreach (int id in _scores.Keys) {
				scores.Add(new KeyValuePair<int, int>(id, _scores[id]));
			}
			scores.Sort((x, y) => {
				if (x.Value > y.Value) return -1; 
				else if (x.Value < y.Value) return 1;
				else return 0;
			});
			return scores;
		}
	}


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

		//printScores();
	}

	public void printScores() {
		List<KeyValuePair<int, int>> scores = ScoresSorted;
		foreach(KeyValuePair<int, int> s in scores) {
			Debug.Log(string.Format("player {0} has points {1}", s.Key, s.Value));
		}
	}
}
