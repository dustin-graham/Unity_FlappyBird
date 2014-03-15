using UnityEngine;
using System.Collections;

public class GameOverGUI : MonoBehaviour {

	public System.Action OnRetry;
	public tk2dTextMesh scoreText;
	public tk2dTextMesh bestScoreText;
	public tk2dUIItem retryButton;

	private int bestScore = 0;
	private GameManager _gameManager;

	void Awake() {
		_gameManager = GameObject.FindObjectOfType<GameManager>();
	}

	void OnEnable() {
		retryButton.OnClick += OnRetryClicked;
		int currentScore = _gameManager.Score;
		int highScore = _gameManager.HighScore;
		scoreText.text = currentScore.ToString();
		bestScoreText.text = highScore.ToString();
	}

	void OnDisable() {
		retryButton.OnClick -= OnRetryClicked;
	}

	private void OnRetryClicked() {
		if (OnRetry != null) OnRetry();
	}
}
