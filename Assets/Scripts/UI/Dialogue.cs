using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace PHOCUS.UI
{
    public class Dialogue : MonoBehaviour
    {
        public Interactable[] Interactables;
        public TextMeshProUGUI mainText;
        public Button ResetButton;
        public Button ShopButton;
        public Button ExitButton;
        public Shop Shop;

        public bool IsActive;

        CanvasGroup canvasGroup;

        public Action OnResetPlatform = delegate { };

        void Awake()
        {
            Interactables = GetComponentsInChildren<Interactable>();
            canvasGroup = GetComponent<CanvasGroup>();
            Shop = UIManager.Instance.Shop;

            ResetButton.onClick.AddListener(ResetPlatform);
            ShopButton.onClick.AddListener(ToggleShop);
            ExitButton.onClick.AddListener(ToggleDialogue);
        }

        public void ToggleDialogue()
        {
            IsActive = !IsActive;
            canvasGroup.alpha = canvasGroup.alpha == 0 ? 1 : 0;
            canvasGroup.blocksRaycasts = !canvasGroup.blocksRaycasts;
        }

        void ToggleShop()
        {
            Shop.ToggleShop();
            ToggleDialogue();
        }

        void ResetPlatform()
        {
            ToggleDialogue();
            OnResetPlatform();
        }
    }
}