using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class GameController : MonoBehaviour {

    public GameObject MainMenuContainer;
    public GameObject[] GameContainers;
    public Text ScoreText;
    public Text BestScoreText;
    public bool SkipMenu;
	public Text PreStartTimeText;
    public float PlayerDeathScreenShake;
    public GameOverText GameOverText;

	private float gameTime;
	private float timer;
	private float preStartTime = 3;

    private bool _gameFinished;
	private  int _score;
    private int _bestScore = -1;
    private string _bestScorePattern;
	private bool GameStarted;
	public bool StartSequence;

    private void Awake() {
        GameVisible = SkipMenu;
        HumanPlayer.Instance.EntityDiedListener = PlayerDied;
        AiPlayer.Instance.EntityDiedListener = EnemyDied;

        _bestScorePattern = BestScoreText.text;

        _score = 0;
        UpdateScore();
    }

    public void StartGame() { 
		GameStarted = false;
		StartSequence = true;
		Cursor.SetCursor (Options.Global.AimSprite, new Vector2(17, 9), CursorMode.Auto);
		preStartTime = 0f;//Тут задавалось время отсчета
		PreStartTimeText.gameObject.SetActive (true);
		Analytics.CustomEvent ("GameStarted");
    }

	void Update () {
		
		gameTime += Time.deltaTime;
		if (StartSequence == true) {
			preStartTime -= Time.deltaTime;
		}

		if (preStartTime <= 0 && GameStarted == false) {
			GameVisible = true;
			RestartGame ();
			PreStartTimeText.gameObject.SetActive (false);
			AudioController.Global.StartGame ();
			GameStarted = true;
		} else {
			PreStartTimeText.text = preStartTime.ToString ("00");
		}

		if (GameStarted == true) {
			timer += Time.deltaTime;
			if (timer >= 20f) {
				timer = 0;
				ObstaclesController.Instance.RegenerateObstacles ();
			}
		} else {
			timer = 0;
		}
		if (Input.GetKeyDown (KeyCode.P) || Input.GetKeyDown (KeyCode.Pause)) {
			if (Time.timeScale != 0) {
				Time.timeScale = 0;
			}
			else {
				Time.timeScale = 1;
			}
		}
	}

    private bool GameVisible {
        set {
            MainMenuContainer.SetActive(!value);
            foreach (var container in GameContainers) {
                container.SetActive(value);
            }
        }
    }

    private void RestartGame() {
        _gameFinished = false;
        Arena.Instance.Restart();
        ObstaclesController.Instance.RegenerateObstacles();

        HumanPlayer.Instance.Restart();
        HumanPlayer.Instance.transform.localPosition = new Vector3(-3.5f, 0, transform.position.z);

        AiPlayer.Instance.DifficultyLevel(0);
        AiPlayer.Instance.Restart();
        AiPlayer.Instance.transform.localPosition = new Vector3(3.5f, 0, transform.position.z);

        PickupsController.Instance.Restart();
        ProjectilesController.Instance.Restart();

        _score = 0;
        UpdateScore();
    }

    private void PlayerDied() {
        if (!_gameFinished) {
            FinishGame();
            CameraController.Instance.Shake(PlayerDeathScreenShake);
        }
    }

    private void EnemyDied() {
        if (!_gameFinished) {
            _score++;
            UpdateScore();
            StartCoroutine(RespawnEnemyCoroutine());
            CameraController.Instance.Shake(PlayerDeathScreenShake);
        }
    }

    private void RespawnEnemy() {
		AiPlayer.Instance.DifficultyLevel(_score);
        AiPlayer.Instance.Restart();
        AiPlayer.Instance.transform.localPosition = -HumanPlayer.Instance.transform.localPosition;
    }

    private void FinishGame() {
        _gameFinished = true;
		Analytics.CustomEvent("gameover", new Dictionary<string, object>
			{
				{ "bestScore", _bestScore },
				{ "Time", gameTime }
			});
		gameTime = 0;
        StopAllCoroutines();
        GameOverText.ShowAnimation();
		GameStarted = false;
		StartSequence = false;
		preStartTime = 3f;
		Cursor.SetCursor (Options.Global.CursorSrpite, new Vector2(17, 9), CursorMode.Auto);
        StartCoroutine(FinishGameCoroutine());
    }

    private void UpdateScore() {
        ScoreText.text = _score.ToString();
        if (_bestScore < _score) {
            _bestScore = _score;
            BestScoreText.text = String.Format(_bestScorePattern, _bestScore);
        }
    }

    private IEnumerator RespawnEnemyCoroutine() {
        yield return new WaitForSeconds(1);
        RespawnEnemy();
    }

    private IEnumerator FinishGameCoroutine() {
        yield return new WaitForSeconds(2);
        GameVisible = false;
    }
}