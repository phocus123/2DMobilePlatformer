using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PHOCUS.UI
{
    public class ShopItem : MonoBehaviour, IPointerClickHandler
    {
        public TextMeshProUGUI itemNameText;
        public TextMeshProUGUI itemCostText;

        Color defaultColor = Color.white;
        Color selectedColor = new Color(255, 232, 0, 1);

        public Action<ShopItem> OnItemClicked = delegate { };

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
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnItemClicked(this);
        }

        public void SelectItem()
        {
            itemNameText.color = selectedColor;
            itemCostText.color = selectedColor;
        }

        public void DeselectItem()
        {
            itemNameText.color = defaultColor;
            itemCostText.color = defaultColor;
        }
    }
}