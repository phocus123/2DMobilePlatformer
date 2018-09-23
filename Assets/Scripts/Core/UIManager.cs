using TMPro;
using PHOCUS.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace PHOCUS.Core
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("Alerts")]
        public TextMeshProUGUI alertText;
        [Header("Player Health")]
        public Image playerHealth;
        public TextMeshProUGUI healthText;
        [Header("Gem Count")]
        public TextMeshProUGUI gemText;

        public void SetAlertText(string text)
        {
            alertText.text = text;
        }

        public void UpdatePlayerHealth(float currentHealth, float maxHealth)
        {
            float health = currentHealth / maxHealth;
            playerHealth.fillAmount = health;
            healthText.text = string.Format("{0}/{1}", currentHealth, maxHealth);
        }

        public void UpdateEnemyHealth(Image healthBar, float healthPercentage)
        {
            healthBar.fillAmount = healthPercentage;
        }

        public void UpdateGemCount(int count)
        {
            gemText.text = count.ToString();
        }
    }
}