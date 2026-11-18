//Wiktoria Burniak, Paweł Gardzijewski
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using TMPro;
namespace Game023
{
    public enum GameState { GS_GAME, GS_PAUSE_MENU, GS_LEVEL_COMPLETED, GS_GAME_OVER }

    public class GameManager : MonoBehaviour
    {

        public static GameManager instance;
        public GameState currentGameState = GameState.GS_PAUSE_MENU;
        private int score = 0;
        private int livesNum = 3;
        public Canvas gameCanvas;
        public Canvas pauseMenuCanvas;
        public Canvas leveCompletedCanvas;
        public TMP_Text scoreLabel;
        public TMP_Text levelCompletedScoreLabel;
        public TMP_Text highScoreLabel;
        public TMP_Text HPLabel;
        public TMP_Text Timer;
        private float timeCounter;
        public TMP_Text EaglesKilledLabel;
        private int eaglesKilled = 0;
        private int keysFound = 0;
        public const int maxKeysNumber = 3;
        public const string keyHighScore = "HighScoreLevel1";
        public bool keysCompleted = false;
        public Image[] keysTab;

        public void AddKeys(int keyIndex, Color keyColor)
        {
            keysFound += 1;
            keysTab[keyIndex].color = keyColor;
            if (keysFound == maxKeysNumber)
            {
                keysCompleted = true;
                Debug.Log("znaleziono klucze");
            }
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                if (!PlayerPrefs.HasKey(keyHighScore))
                {
                    PlayerPrefs.SetInt(keyHighScore, 0);
                }
                if (scoreLabel != null)
                {
                    scoreLabel.text = score.ToString();
                }
                if (HPLabel != null)
                {
                    HPLabel.text = livesNum.ToString();
                }
                if(Timer != null)
                {
                    Timer.text = "00:00";
                }
                if (EaglesKilledLabel != null)
                {
                    EaglesKilledLabel.text = eaglesKilled.ToString();
                }
                InGame();
            }
            else
            {
                Debug.LogError("Duplicated Game Manager", gameObject);
            }
            for (int i = 0; i < maxKeysNumber; i++)
            {
                keysTab[i].color = Color.grey;
            }
        }

        void SetGameState(GameState newGameState)
        {
            currentGameState = newGameState;
            if (gameCanvas != null)
            {
                gameCanvas.enabled = currentGameState == GameState.GS_GAME;
            }
            if (pauseMenuCanvas != null)
            {
                pauseMenuCanvas.enabled = currentGameState == GameState.GS_PAUSE_MENU;
            }
            if (leveCompletedCanvas != null)
            {
                leveCompletedCanvas.enabled = currentGameState == GameState.GS_LEVEL_COMPLETED;
            }
            if (newGameState == GameState.GS_LEVEL_COMPLETED)
            {
                Scene currentScene = SceneManager.GetActiveScene();
                if (currentScene.name == "Level1")
                {
                    int highScore = PlayerPrefs.GetInt(keyHighScore);
                    if (highScore < score)
                    {
                        highScore = score;
                        PlayerPrefs.SetInt(keyHighScore, highScore);
                    }
                    if (levelCompletedScoreLabel != null)
                    {
                        levelCompletedScoreLabel.text = score.ToString();
                    }
                    if (highScoreLabel != null)
                    {
                        highScoreLabel.text = highScore.ToString();
                    }
                }
            }
        }

        public void PauseMenu()
        {
            SetGameState(GameState.GS_PAUSE_MENU);
        }

        public void InGame()
        {
            SetGameState(GameState.GS_GAME);
        }

        public void OnResumeButtonClicked()
        {
            InGame();
        }

        public void OnRestartButtonClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OnReturnToMainMenuButtonClicked()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void LevelCompleted()
        {
            SetGameState(GameState.GS_LEVEL_COMPLETED);
        }

        public void GameOver()
        {
            SetGameState(GameState.GS_GAME_OVER);
        }

        public void AddPoints(int points)
        {
            score += points;
            scoreLabel.text = score.ToString();
            //Debug.Log(score);
        }
        public void AddLives(int lives)
        {
            livesNum += lives;
            
            if (livesNum < 0)
            {
                livesNum = 0;
                GameOver();
            }
            HPLabel.text = livesNum.ToString();
        }
        public int GetLives()
        {
            return livesNum;
        }
        public void AddEagleKilled(int killed)
        {
            eaglesKilled += killed;
            EaglesKilledLabel.text = eaglesKilled.ToString(); 
            
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (currentGameState == GameState.GS_GAME)
                {
                    PauseMenu();
                }
                else if (currentGameState == GameState.GS_PAUSE_MENU)
                {
                    InGame();
                }
            }
            if (currentGameState == GameState.GS_GAME)
            {
                timeCounter += Time.deltaTime;
                int minutes = Mathf.FloorToInt(timeCounter / 60f);
                int seconds = Mathf.FloorToInt(timeCounter % 60f);

                if (Timer != null)
                {
                    Timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                }
            }


        }
    }
}