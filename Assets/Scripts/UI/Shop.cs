using PHOCUS.Character;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PHOCUS.Utilities;
using PHOCUS.Environment;
using System.Collections.Generic;

namespace PHOCUS.UI
{
    public class Shop : MonoBehaviour
    {
        public PlatformController Platform;
        public Shopkeeper Shopkeeper;
        public GameObject ShopItemPrefab;
        public Transform ShopItemParent;
        public Camera PathCamera;
        public List<ShopItem> Items;
        public Button BuyButton;
        public Button ExitButton;
        public TextMeshProUGUI GemText;
        public CanvasGroup ShopCanvas;

        ShopItem selectedItem;
        Player player;
        bool isEnabled;
    
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (!isEnabled)
                {
                    LoadPaths();
                    isEnabled = value;
                }
                else
                { 
                    DeletePaths();
                    isEnabled = value;
                }
            }
        }

        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            ShopCanvas = GetComponent<CanvasGroup>();

            ExitButton.onClick.AddListener(ToggleShop);
            BuyButton.onClick.AddListener(BuyItem);
        }

        public void ToggleShop()
        {
            if (selectedItem != null)
            {
                selectedItem.DeselectItem();
                selectedItem = null;
            }

            UpdateGemsText();
            ShopCanvas.Toggle();
            IsEnabled = !IsEnabled;
            player.TogglePlayerActions();
        }

        void LoadPaths()
        {
            foreach (PathController path in Platform.Paths)
            {
                var go = Instantiate(ShopItemPrefab, ShopItemParent);
                var item = go.GetComponent<ShopItem>();
                Items.Add(item);
                item.OnItemClicked += SelectItem;

                item.ItemNameText.text = path.name.ToString();
                item.ItemCostText.text = path.GemCost.ToString() + "G";
                item.GemCost = path.GemCost;
                item.PathController = path;
            }

            if (Shopkeeper.LoadedShopPaths)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Shopkeeper.ItemsPurchased[i])
                    {
                        Items[i].BuyItem();
                    }
                }
            }

            Shopkeeper.ItemsPurchased.Clear();
            Shopkeeper.LoadedShopPaths = true;
        }

        void DeletePaths()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                Shopkeeper.ItemsPurchased.Add(Items[i].HasBeenPurchased);
                Items[i].OnItemClicked -= SelectItem;
                Destroy(Items[i].gameObject);
            }

            Items.Clear();
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
                UIManager.Instance.SetAndFadeAlertText("You do not have enough gems!");
            }
        }

        void SetCamera()
        {
            PathCamera.transform.localPosition = selectedItem.PathController.CameraLookAt.transform.localPosition;
        }
    }
}