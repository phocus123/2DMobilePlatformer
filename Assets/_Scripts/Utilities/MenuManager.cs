using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace PHOCUS.Utilities
{
    public class MenuManager : Singleton<MenuManager>
    {
        public Button PlayButton;
        public Button LeaderboardsButton;
        public Button SettingsButton;
        public Button ExitButton;
        public TextMeshProUGUI CoinsText;
        public TextMeshProUGUI AccountNameText;
        public TextMeshProUGUI AccountLevelText;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            PlayButton.onClick.AddListener(() => LoadScene(1));
            ExitButton.onClick.AddListener(QuitGame);

            UpdateAccountDetails();
            UpdateGemCount();
        }

        public void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
        }

        void QuitGame()
        {
            Application.Quit();
        }

        void UpdateGemCount()
        {
            CoinsText.text = PlayerAccount.Instance.Coins.ToString();
        }

        void UpdateAccountDetails()
        {
            AccountNameText.text = PlayerAccount.Instance.DisplayName;
            AccountLevelText.text = string.Format("Level: {0}", PlayerAccount.Instance.AccountLevel.ToString());
        }
    }
}