using PHOCUS.Character;
using PHOCUS.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PHOCUS.Utilities
{
    public class GameManager : Singleton<GameManager>
    {
        public InteractableRaycaster InteractableRaycaster;
        public Player Player;
        public int TotalGemCount;

        bool gameOver;
        bool timerActive;
        int finalScore;
        float timeElapsed;

        void Awake()
        {
            UIManager.instance.GameOverPanel.OnRestart += Restart;
            Time.timeScale = 1;
            StartCoroutine(StartTimer());
        }

        void Update()
        {
            if (Player != null)
            {
                CheckPlayerDeath();
            }
        }

        void CheckPlayerDeath()
        {
            if (Player.Health <= 0 && !gameOver)
            {
                gameOver = true;
                DetermineScore();

                string scoreText = string.Format("Final Score: {0}", finalScore);
                UIManager.instance.GameOverPanel.SetScore(scoreText);
                UIManager.instance.GameOverPanel.TogglePanel();
            }
        }

        IEnumerator StartTimer()
        {
            timerActive = true;

            while (!gameOver)
            {
                timeElapsed += Time.deltaTime;

                string minutes = Mathf.Floor(timeElapsed / 60).ToString("00");
                string seconds = (timeElapsed % 60).ToString("00");
                string time = string.Format("Timer: {0}:{1}", minutes, seconds);

                UIManager.instance.SetTimerText(time);

                yield return null;
            }
        }

        void Restart()
        {
            gameOver = false;
            timerActive = false;
            SceneManager.LoadScene(1);
        }

        void DetermineScore()
        {
            int gems = TotalGemCount;
            int time = (int)timeElapsed;

            finalScore = ((time / 4) * gems);
        }
    }
}
