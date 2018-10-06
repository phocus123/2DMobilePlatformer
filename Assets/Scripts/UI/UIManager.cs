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
        public TextMeshProUGUI AlertText;
        public CanvasGroup AlertGroup;
        [Header("Player HUD")]
        public Image PlayerHealth;
        public TextMeshProUGUI HealthText;
        public Image PlayerStamina;
        public TextMeshProUGUI GemText;
        [Header("Combat Text")]
        public GameObject CombatTextPrefab;
        public float Speed;
        [Header("UI References")]
        public Shop Shop;
        public Dialogue Dialogue;
        public Canvas WorldSpaceCanvas;

        bool alertActive;

        public void SetAndFadeAlertText(string text)
        {
            AlertGroup.alpha = 1;
            AlertText.text = text;

            if (!alertActive)
                StartCoroutine(FadeAlertText());
        }

        public void SetAlertText(string text)
        {
            AlertGroup.alpha = 1;
            AlertText.text = text;
        }

        IEnumerator FadeAlertText() // TODO we dont always want to fade, for instance with spawn countdown
        {
            alertActive = true;
            float progress = 1f;

            yield return new WaitForSeconds(1.5f);

            while (progress > 0)
            {
                progress -= Time.deltaTime;
                AlertGroup.alpha = Mathf.Lerp(0, 1, progress);
                yield return null;
            }

            alertActive = false;
        }

        public void UpdatePlayerHealth(float currentHealth, float maxHealth)
        {
            float health = currentHealth / maxHealth;
            PlayerHealth.fillAmount = health;
            HealthText.text = string.Format("{0}/{1}", currentHealth, maxHealth);
        }

        public void UpdateStamina(float current, float max)
        {
            float stamina = current / max;
            PlayerStamina.fillAmount = stamina;
        }

        public void UpdateEnemyHealth(Image healthBar, float healthPercentage)
        {
            healthBar.fillAmount = healthPercentage;
        }

        public void UpdateGemCount(int count)
        {
            GemText.text = count.ToString();
        }

        public void TriggerCombatText(Vector2 position, float healthValue, CombatTextType combatTextType)
        {
            GameObject combatText = Instantiate(CombatTextPrefab, position, Quaternion.identity, WorldSpaceCanvas.transform);
            combatText.GetComponent<CombatText>().Initialise(combatTextType);
            combatText.GetComponent<TextMeshProUGUI>().text = healthValue.ToString();
        }
    }
}