using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace PHOCUS.UI
{
    public class Dialogue : MonoBehaviour
    {
        public DialogueButton[] DialogueButtons;
        public TextMeshProUGUI mainText;
        public Button ResetButton;
        public Button ShopButton;
        public Button ExitButton;

        CanvasGroup canvasGroup;
        Shop shop;

        public Action OnResetPlatform = delegate { };

        void Awake()
        {
            DialogueButtons = GetComponentsInChildren<DialogueButton>();
            canvasGroup = GetComponent<CanvasGroup>();
            shop = UIManager.Instance.Shop;

            ResetButton.onClick.AddListener(ResetPlatform);
            ShopButton.onClick.AddListener(ToggleShop);
            ExitButton.onClick.AddListener(ToggleDialogue);
        }

        public void ToggleDialogue()
        {
            canvasGroup.alpha = canvasGroup.alpha == 0 ? 1 : 0;
            canvasGroup.blocksRaycasts = !canvasGroup.blocksRaycasts;
        }

        void ToggleShop()
        {
            shop.ToggleShop();
            ToggleDialogue();
        }

        void ResetPlatform()
        {
            OnResetPlatform();
            ToggleDialogue();
        }
    }
}