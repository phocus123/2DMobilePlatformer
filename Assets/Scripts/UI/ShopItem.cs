using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using PHOCUS.Environment;
using UnityEngine.UI;

namespace PHOCUS.UI
{
    public class ShopItem : MonoBehaviour, IPointerClickHandler
    {
        public TextMeshProUGUI itemNameText;
        public TextMeshProUGUI itemCostText;
        public PathController PathController;
        public int GemCost;
        public bool HasBeenPurchased;

        public Action<ShopItem> OnItemClicked = delegate { };

        Color defaultColor = Color.white;
        Color selectedColor = new Color(255, 232, 0, 1);


        void OnValidate()
        {
            TextMeshProUGUI[] childTexts = GetComponentsInChildren<TextMeshProUGUI>();

            if (itemNameText == null)
                itemNameText = childTexts[0];
            if (itemCostText == null)
                itemCostText = childTexts[1];
        }

        void Start()
        {
            TextMeshProUGUI[] childTexts = GetComponentsInChildren<TextMeshProUGUI>();

            if (itemNameText == null)
                itemNameText = childTexts[0];
            if (itemCostText == null)
                itemCostText = childTexts[1];

            if (PathController != null)
                GemCost = PathController.GemCost;

            SetUI();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnItemClicked(this);
        }

        public void SelectItem()
        {
            if (!HasBeenPurchased)
            {
                itemNameText.color = selectedColor;
                itemCostText.color = selectedColor;
                TogglePath();
            }
        }

        public void DeselectItem()
        {
            if (!HasBeenPurchased)
            {
                itemNameText.color = defaultColor;
                itemCostText.color = defaultColor;
                TogglePath();
            }
        }

        public void BuyItem()
        {
            itemNameText.text = "Purchased";
            itemCostText.text = string.Empty;
            Button button = GetComponent<Button>();
            button.interactable = false;
            HasBeenPurchased = true;
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
                itemNameText.text = PathController.gameObject.name;
                itemCostText.text = GemCost.ToString() + "G";
            }
        }
    }
}