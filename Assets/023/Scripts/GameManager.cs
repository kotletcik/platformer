//Wiktoria Burniak, Paweł Gardzijewski
using UnityEngine;
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
        private GUIStyle levelCompletedStyle;
        private int livesNum = 3;
        public Canvas gameCanvas;
        public TMP_Text scoreLabel;



        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                if (scoreLabel != null)
                {
                    scoreLabel.text = score.ToString();
                }
            }
            else
            {
                Debug.LogError("Duplicated Game Manager", gameObject);
            }
        }

        void SetGameState(GameState newGameState)
        {
            currentGameState = newGameState;
            if (gameCanvas != null && currentGameState == GameState.GS_GAME)
            {
                gameCanvas.enabled = true;
            }
            else
            {
                gameCanvas.enabled = false;
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
            }
        }
        public int GetLives()
        {
            return livesNum;
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
        }

        void OnGUI()
        {
            if (currentGameState == GameState.GS_LEVEL_COMPLETED)
            {
                if (levelCompletedStyle == null)
                {
                    levelCompletedStyle = new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fontSize = 48
                    };
                    levelCompletedStyle.normal.textColor = Color.white;
                }

                GUI.Label(
                    new Rect(0, 0, Screen.width, Screen.height),
                    "POZIOM UKOŃCZONY!",
                    levelCompletedStyle);
            }
        }
    }
}
