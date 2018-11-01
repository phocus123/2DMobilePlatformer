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
        public int CoinCount;

        bool gameOver;
        bool timerActive;
        int finalScore;
        float timeElapsed;

        void Awake()
        {
            UIManager.Instance.GameOverPanel.OnRestartClicked += CheckLives;
            UIManager.Instance.GameOverPanel.OnQuitClicked += ReturnToMenu;
            PlayFabCurrencyManager.Instance.OnReceivedCurrency += UIManager.Instance.GameOverPanel.SetLives;
            PlayFabCurrencyManager.Instance.OnPlayerHasLives += Restart;
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
                PlayFabCurrencyManager.Instance.SubtractLife();
                PlayFabCurrencyManager.Instance.AddCoins(CoinCount);
                UIManager.instance.GameOverPanel.SetScore(scoreText);
                UIManager.instance.GameOverPanel.CanvasGroup.Toggle();
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

                UIManager.Instance.SetTimerText(time);

                yield return null;
            }
        }

        void CheckLives()
        {
            PlayFabCurrencyManager.Instance.CheckLives();
        }

        void Restart(bool restart)
        {
            if (restart)
            {
                gameOver = false;
                timerActive = false;
                SceneManager.LoadScene(2);
            }
            else
            {
                Destroy(Player.gameObject);
                Time.timeScale = 1;
                StartCoroutine(ReturnToMenuCountdown());
            }
        }

        void ReturnToMenu()
        {
            PlayFabCurrencyManager.Instance.OnReceivedCurrency -= UIManager.Instance.GameOverPanel.SetLives;
            SceneManager.LoadScene(1);
        }

        void DetermineScore()
        {
            int gems = TotalGemCount;
            int time = (int)timeElapsed;

            finalScore = ((time / 4) * gems);
        }

        IEnumerator ReturnToMenuCountdown()
        {
            float countdown = 3f;
            float progress = 0f;

            while (progress < countdown)
            {
                progress += Time.deltaTime;
                float timeRemaining = countdown - progress;
                string time = string.Format("You are out of lives. Returning to menu in {0} seconds", Mathf.Round(timeRemaining));
                UIManager.Instance.SetAlertText(time);
                yield return null;
            }

            SceneManager.LoadScene(1);
        }
    }
}
