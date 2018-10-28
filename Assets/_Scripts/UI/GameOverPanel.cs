using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PHOCUS.Utilities;

namespace PHOCUS.UI
{
    public class GameOverPanel : MonoBehaviour
    {
        public CanvasGroup CanvasGroup;
        public Button RestartButton;
        public Button QuitButton;
        public TextMeshProUGUI ScoreText;

        public Action OnRestart = delegate { };

        void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
            Button[] buttons = GetComponentsInChildren<Button>();

            RestartButton = buttons[0];
            QuitButton = buttons[1];
        }

        void Start()
        {
            RestartButton.onClick.AddListener(Restart);
            QuitButton.onClick.AddListener(Quit);
        }

        public void TogglePanel()
        {
            CanvasGroup.alpha = CanvasGroup.alpha == 0 ? 1 : 0;
            CanvasGroup.blocksRaycasts = CanvasGroup.blocksRaycasts == false ? true : false;
        }

        void Restart()
        {
            OnRestart();
            TogglePanel();
        }

        void Quit()
        {
            MenuManager.Instance.LoadScene(0);
        }

        public void SetScore(string score)
        {
            ScoreText.text = score;
        }
    }
}