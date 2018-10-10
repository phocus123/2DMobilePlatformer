using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PHOCUS.Utilities
{
    public class MenuManager : Singleton<MenuManager>
    {
        public Button PlayButton;
        public Button LeaderboardsButton;
        public Button SettingsButton;
        public Button ExitButton;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            PlayButton.onClick.AddListener(() => LoadScene(1));
            ExitButton.onClick.AddListener(QuitGame);
        }

        public void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
        }

        void QuitGame()
        {
            Application.Quit();
        }
    }
}