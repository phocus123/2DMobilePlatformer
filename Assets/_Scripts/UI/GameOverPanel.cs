using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PHOCUS.Utilities;

namespace PHOCUS.UI
{
    public class GameOverPanel : MonoBehaviour
    {
        public CanvasGroup CanvasGroup;
        public Button RestartButton;
        public Button QuitButton;
        public TextMeshProUGUI ScoreText;
        public TextMeshProUGUI LivesText;

        public Action OnRestartClicked = delegate { };
        public Action OnQuitClicked = delegate { };

        void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
            Button[] buttons = GetComponentsInChildren<Button>();

            RestartButton = buttons[0];
            QuitButton = buttons[1];
        }

        void Start()
        {
            RestartButton.onClick.AddListener(RestartClicked);
            QuitButton.onClick.AddListener(QuitClicked);
        }

        public void SetScore(string score)
        {
            ScoreText.text = score;
        }

        public void SetLives(string lives, string coins)
        {
            string livesText = string.Format("Lives Remaining: {0}", lives);
            LivesText.text = livesText;
        }

        void RestartClicked()
        {
            OnRestartClicked();
            CanvasGroup.Toggle();
        }

        void QuitClicked()
        {
            OnQuitClicked();
        }
    }
}