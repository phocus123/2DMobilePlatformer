using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using PHOCUS.Environment;
using UnityEngine.UI;

namespace PHOCUS.UI
{
    public class ShopButton : MonoBehaviour, IPointerClickHandler
    {
        public TextMeshProUGUI ItemNameText;
        public TextMeshProUGUI ItemCostText;
        public PathController PathController;
        public int GemCost;
        public bool HasBeenPurchased;

        public Action<ShopButton> OnButtonClicked = delegate { };

        Color defaultColor = Color.white;
        Color selectedColor = new Color(255, 232, 0, 1);

        void Awake()
        {
            TextMeshProUGUI[] childTexts = GetComponentsInChildren<TextMeshProUGUI>();

            if (ItemNameText == null)
                ItemNameText = childTexts[0];
            if (ItemCostText == null)
                ItemCostText = childTexts[1];

            if (PathController != null)
                GemCost = PathController.GemCost;

            SetUI();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnButtonClicked(this);
        }

        public void SelectItem()
        {
            if (!HasBeenPurchased)
            {
                ItemNameText.color = selectedColor;
                ItemCostText.color = selectedColor;
                TogglePath();
            }
        }

        public void DeselectItem()
        {
            ItemNameText.color = defaultColor;
            ItemCostText.color = defaultColor;

            if (!HasBeenPurchased)
            {
                TogglePath();
            }
        }

        public void BuyItem()
        {
            ItemNameText.text = "Purchased";
            ItemCostText.text = string.Empty;
            Button button = GetComponent<Button>();
            button.interactable = false;
            HasBeenPurchased = true;
            DeselectItem();
        }

        void TogglePath()
        {
            if (PathController != null)
                PathController.TogglePaths();
        }

        void SetUI()
        {
            if (PathController != null)
            { 
                ItemNameText.text = PathController.gameObject.name;
                ItemCostText.text = GemCost.ToString() + "G";
            }
        }
    }
}