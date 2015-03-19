using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverGUI : MonoBehaviour {

	public Text scoreText;
	public Text bestScoreText;
	public Button retryButton;

	private int bestScore = 0;
	private GameManager _gameManager;

	void Awake() {
		_gameManager = GameObject.FindObjectOfType<GameManager>();
	}

	void OnEnable() {
		int currentScore = _gameManager.Score;
		int highScore = _gameManager.HighScore;
		scoreText.text = currentScore.ToString();
		bestScoreText.text = highScore.ToString();
	}
}
