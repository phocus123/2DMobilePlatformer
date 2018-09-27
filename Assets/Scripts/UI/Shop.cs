using PHOCUS.Character;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PHOCUS.Utilities;

namespace PHOCUS.UI
{
    public class Shop : MonoBehaviour
    {
        public GameObject ShopItemPrefab;
        public ShopItem[] Items;
        public Button ExitButton;
        public TextMeshProUGUI GemText;
        public CanvasGroup ShopCanvas;

        ShopItem selectedItem;
        Player player; 

        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            ShopCanvas = GetComponent<CanvasGroup>();
            ExitButton.onClick.AddListener(ToggleShop);

            foreach (var item in Items)
            {
                item.OnItemClicked += SelectItem;
            }

            SelectItem(Items[0]);
            UpdateGemsText();
        }

        public void ToggleShop()
        {
            ShopCanvas.Toggle();
        }

        void SelectItem(ShopItem item)
        {
            if (selectedItem == null)
            {
                selectedItem = item;
                item.SelectItem();
            }
            else
            {
                selectedItem.DeselectItem();
                selectedItem = item;
                item.SelectItem();
            }
        }

        void UpdateGemsText()
        {
            GemText.text = string.Format("{0}G", player.Gems.ToString());
        }
    }
}