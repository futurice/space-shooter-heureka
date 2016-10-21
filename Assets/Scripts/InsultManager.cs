using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

//TODO rename
public class InsultManager : Singleton<InsultManager> {
	
	private int _lastKIA = 0;

	private string[] _kiaInsults = new string[0];
	private int _kiaIndex = 0;

	private string _instructions = "";

	public void readAllSpeeches() {
		string[] lines = System.IO.File.ReadAllLines (Application.streamingAssetsPath + "/insults.txt");
		if (lines != null && lines.Length > 0) {
			_kiaInsults = lines;
		}
		else {
			//let's put something
			_kiaInsults = new string[1];
			_kiaInsults[0] = "You need to wake up Player {0}.";
		}

		_instructions = System.IO.File.ReadAllText (Application.streamingAssetsPath + "/instructions.txt");
	}

	public void tellInstructions() {
		AudioManager.Instance.Speak(_instructions);
	}
	
	public void tellIntro() {
		AudioManager.Instance.Speak("Welcome Space Cadets! This is Admiral Marcus, Commander of the USS Futurice.");	
	}

	public void tellHighscores(List<KeyValuePair<int, int>> scores, bool firstRound) {
		StringBuilder s = new StringBuilder();
		s.AppendLine("Here are the scores.");
		if (scores.Count > 0) {
			int best = scores[0].Key;
			s.AppendLine(string.Format("Well done player {0}.", best));

			//TODO speak if total high score
            int bestScore = scores[0].Value;
			if (bestScore >= ScoreManager.Instance.HighScore) {
				s.AppendLine("This is a new high score!");
			}

			if (scores.Count > 1) {
				int worst = scores[scores.Count - 1].Key;
				//TODO read from file and add more
				if (Random.value > 0.5f) {
					s.AppendLine(string.Format("Player {0}, pathetic.", worst));
                }
                else {
                    s.AppendLine(string.Format("Player {0}, you're dismissed.", worst));
                }
            }
        }
		if (firstRound) {
			s.AppendLine("Cadets, now focus, this is your last chance.");
		}
		else {
			s.AppendLine("I'd like the winners to report to the deck, we have a surprise for you. All you others can piss off");
        }

		AudioManager.Instance.Speak(s.ToString());	
    }
    
    
	public void insultAboutSpeed(int id, List<Collectable> collectables) {
		//Just for laughs
		int count = 0;
		foreach (Collectable c in collectables) {
			if (c.Type == Collectable.CollectableType.SpeedUp)
				count++;
		}           
		if (count == 4) {
			AudioManager.Instance.Speak(string.Format("Player {0}, you're going too fast!", id));
		}
		else if (count == 5) {
			AudioManager.Instance.Speak(string.Format("Player {0}, slow down!", id));
		}
	}

	public void playerDied(int id) {
		if (_lastKIA == id) {
			AudioManager.Instance.Speak(string.Format(_kiaInsults[_kiaIndex++], id));
			if (_kiaIndex >= _kiaInsults.Length) {
				_kiaIndex = 0;
			}
			//this guy died twice in a row so he deserves an insult
		}
		_lastKIA = id;
	}
}
