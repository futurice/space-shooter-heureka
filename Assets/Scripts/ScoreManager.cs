using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class ScoreManager : Singleton<ScoreManager> {

	private string HighScoreFilePath {
		get { return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/high-scores.csv"; }
	}

	Dictionary<int, int> _scores = new Dictionary<int, int>();

	public Dictionary<int, int> Scores
	{
		get
		{
			return new Dictionary<int, int> (_scores);
		}
	}

	private int _highScoreOfToday = 0;//TODO init at startup from file
	public int HighScore {
		get { return _highScoreOfToday; }
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


	public void clearScores() {
		_scores = new Dictionary<int, int>();
	}

	public void saveSession(int sessionId) {
		string path = HighScoreFilePath;
		Debug.Log("Writing high scores to file: " + path);
		using (StreamWriter sw = File.AppendText(path)) {
			List<KeyValuePair<int, int>> scores = ScoresSorted;
			foreach(KeyValuePair<int, int> s in scores) {
				Debug.Log(string.Format("player {0} has points {1}", s.Key, s.Value));
				sw.WriteLine(string.Format("{0};player {1};{2}", sessionId, s.Key, s.Value));

				if (s.Value >= _highScoreOfToday) {
					_highScoreOfToday = s.Value;
				}
			}
		}
	}

	public int GetPointsForPlayer (int playerId)
	{
		int points = 0;

		if (_scores.TryGetValue (playerId, out points))
		{
			return points;
		}

		return 0;
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
