using PHOCUS.Utilities;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PHOCUS.UI
{
    public class MenuUI : MonoBehaviour
    {
        public Button PlayButton;
        public Button LeaderboardsButton;
        public Button SettingsButton;
        public Button ExitButton;
        public Button NextFreeLifeButton;
        public CanvasGroup TimerCanvasGroup;
        public TextMeshProUGUI CoinsText;
        public TextMeshProUGUI LivesText;
        public TextMeshProUGUI TimerText;
        public TextMeshProUGUI AccountNameText;
        public TextMeshProUGUI AccountLevelText;
        public PlayFabCurrencyManager PlayFabCurrencyManager;

        bool alertActive;

        public Action OnNextFreeLifeClicked = delegate { };


        void Awake()
        {
            PlayButton.onClick.AddListener(() => PlayClicked());
            ExitButton.onClick.AddListener(QuitGame);
            NextFreeLifeButton.onClick.AddListener(GetNextFreeLifeTime);

            UpdateAccountDetails();
            PlayFabCurrencyManager.OnReceivedRechargeTime += UpdateLifeTimer;
            PlayFabCurrencyManager.OnReceivedCurrency += UpdateCurrencyValues;
        }

        void QuitGame()
        {
            Application.Quit();
        }

        void PlayClicked()
        {
            PlayFabCurrencyManager.Instance.CheckLives();
        }

        void UpdateAccountDetails()
        {
            AccountNameText.text = PlayerAccount.Instance.DisplayName;
            AccountLevelText.text = string.Format("Level: {0}", PlayerAccount.Instance.AccountLevel.ToString());
        }

        void UpdateCurrencyValues(string lives, string coins)
        {
            LivesText.text = lives;
            CoinsText.text = coins;
        }

        void UpdateLifeTimer(string text)
        {
            TimerCanvasGroup.alpha = 1;
            TimerText.text = text;

            if (!alertActive)
                StartCoroutine(FadeTimerTextOut());
        }

        void GetNextFreeLifeTime()
        {
            OnNextFreeLifeClicked();
            NextFreeLifeButton.interactable = false;
            StartCoroutine(EnableButtonDelay());
        }

        IEnumerator FadeTimerTextOut() 
        {
            alertActive = true;
            float progress = 1f;

            yield return new WaitForSeconds(1.5f);

            while (progress > 0)
            {
                progress -= Time.deltaTime;
                TimerCanvasGroup.alpha = Mathf.Lerp(0, 1, progress);
                yield return null;
            }

            alertActive = false;
        }

        IEnumerator EnableButtonDelay()
        {
            yield return new WaitForSeconds(15f);
            NextFreeLifeButton.interactable = true;
        }
    }
}