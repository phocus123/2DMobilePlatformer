using TMPro;
using PHOCUS.Utilities;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PHOCUS.UI
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("Alerts")]
        public TextMeshProUGUI alertText;
        public CanvasGroup alertGroup;
        [Header("Player Health")]
        public Image playerHealth;
        public TextMeshProUGUI healthText;
        [Header("Gem Count")]
        public TextMeshProUGUI gemText;
        [Header("Combat Text")]
        [SerializeField] GameObject combatTextPrefab;
        [SerializeField] Canvas combatTextCanvas;
        [SerializeField] float speed;

        public void SetAlertText(string text)
        {
            alertGroup.alpha = 1;
            alertText.text = text;
            StartCoroutine(FadeAlertText());
        }

        IEnumerator FadeAlertText()
        {
            float progress = 1f;

            yield return new WaitForSeconds(1.5f);

            while (progress > 0)
            {
                progress -= Time.deltaTime;
                alertGroup.alpha = Mathf.Lerp(0, 1, progress);
                yield return null;
            }
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

        public void TriggerCombatText(Vector2 position, float healthValue, CombatTextType combatTextType)
        {
            GameObject combatText = Instantiate(combatTextPrefab, position, Quaternion.identity, combatTextCanvas.transform);
            combatText.GetComponent<CombatText>().Initialise(combatTextType);
            combatText.GetComponent<TextMeshProUGUI>().text = healthValue.ToString();
        }
    }
}