using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {
	const string highscore = "high_score";

	public Bird bird;
	public Text scoreText;
	public GameOverGUI gameOverGUI;

	public static System.Action OnGameStart;
	public static System.Action OnGameStop;

	private int _scoreCount = 0;
	public int Score {
		get {
			return _scoreCount;
		}
		set {
			_scoreCount = value;
		}
	}

	private int _highScore = 0;
	public int HighScore {
		get {
			if (_highScore == 0) {
				_highScore = PlayerPrefs.GetInt (highscore, 0);
			}
			return _highScore;
		}
		set {
			PlayerPrefs.SetInt(highscore,value);
			_highScore = value;
		}
	}

	void OnEnable() {
		bird.OnBirdCollision += OnBirdCollisionDetected;
		bird.OnBirdScore += OnBirdScoreDetected;
	}

	void OnDisable() {
		bird.OnBirdCollision -= OnBirdCollisionDetected;
		bird.OnBirdScore -= OnBirdScoreDetected;
	}

	void Start() {
		gameOverGUI.gameObject.SetActive(false);
		if (OnGameStart != null) OnGameStart();
	}

	void OnBirdCollisionDetected() {
		if (Score > HighScore) {
			HighScore = Score;
		}
		if (OnGameStop != null) OnGameStop();
		gameOverGUI.gameObject.SetActive(true);
	}

	void OnBirdScoreDetected() {
		Score++;
		scoreText.text = _scoreCount.ToString();
	}

	public void OnRetyRequested() {
		Application.LoadLevel(0);
	}
}
