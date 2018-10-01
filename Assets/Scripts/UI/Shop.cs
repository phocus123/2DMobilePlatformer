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
        public Camera PathCamera;
        public ShopItem[] Items;
        public Button BuyButton;
        public Button ExitButton;
        public TextMeshProUGUI GemText;
        public CanvasGroup ShopCanvas;
        public bool isEnabled;

        ShopItem selectedItem;
        Player player; 


        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            ShopCanvas = GetComponent<CanvasGroup>();
            ExitButton.onClick.AddListener(ToggleShop);
            BuyButton.onClick.AddListener(BuyItem);

            foreach (var item in Items)
            {
                item.OnItemClicked += SelectItem;
            }
        }

        public void ToggleShop()
        {
            if (selectedItem != null)
            {
                selectedItem.DeselectItem();
                selectedItem = null;
            }

            player.TogglePlayerActions();
            UpdateGemsText();
            ShopCanvas.Toggle();
            isEnabled = !isEnabled;
        }

        void SelectItem(ShopItem item)
        {
            if (selectedItem == null)
            {
                selectedItem = item;
                item.SelectItem();
                SetCamera();
            }
            else
            {
                selectedItem.DeselectItem();
                selectedItem = item;
                item.SelectItem();
                SetCamera();
            }
        }

        void UpdateGemsText()
        {
            GemText.text = string.Format("{0}G", player.Gems.ToString());
        }

        void BuyItem()
        {
            if (selectedItem == null)
                return;

            bool canAfford = selectedItem.GemCost <= player.Gems;

            if (canAfford && !selectedItem.HasBeenPurchased)
            {
                selectedItem.BuyItem();
                player.Gems -= selectedItem.GemCost;
                UpdateGemsText();
            }
            else if (!canAfford && !selectedItem.HasBeenPurchased)
            {
                UIManager.Instance.SetAlertText("You do not have enough gems!");
            }
        }

        void SetCamera()
        {
            PathCamera.transform.localPosition = selectedItem.PathController.CameraLookAt.transform.localPosition;
        }
    }
}