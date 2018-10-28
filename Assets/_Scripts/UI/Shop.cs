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
        public GameObject ShopButtonPrefab;
        public Transform ShopButtonParent;
        public Camera PathCamera;
        public List<ShopButton> Buttons;
        public Button BuyButton;
        public Button ExitButton;
        public TextMeshProUGUI GemText;
        public CanvasGroup ShopCanvas;

        ShopButton selectedButton;
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
            if (selectedButton != null)
            {
                selectedButton.DeselectItem();
                selectedButton = null;
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
                var go = Instantiate(ShopButtonPrefab, ShopButtonParent);
                var item = go.GetComponent<ShopButton>();
                Buttons.Add(item);
                item.OnButtonClicked += SelectButton;

                item.ItemNameText.text = path.name.ToString();
                item.ItemCostText.text = path.GemCost.ToString() + "G";
                item.GemCost = path.GemCost;
                item.PathController = path;
            }

            if (Shopkeeper.LoadedShopPaths)
            {
                for (int i = 0; i < Buttons.Count; i++)
                {
                    if (Shopkeeper.ItemsPurchased[i])
                    {
                        Buttons[i].BuyItem();
                    }
                }
            }

            Shopkeeper.ItemsPurchased.Clear();
            Shopkeeper.LoadedShopPaths = true;
        }

        void DeletePaths()
        {
            for (int i = 0; i < Buttons.Count; i++)
            {
                Shopkeeper.ItemsPurchased.Add(Buttons[i].HasBeenPurchased);
                Buttons[i].OnButtonClicked -= SelectButton;
                Destroy(Buttons[i].gameObject);
            }

            Buttons.Clear();
        }

        void SelectButton(ShopButton item)
        {
            if (selectedButton == null)
            {
                selectedButton = item;
                item.SelectItem();
                SetCamera();
            }
            else
            {
                selectedButton.DeselectItem();
                selectedButton = item;
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
            if (selectedButton == null)
                return;

            bool canAfford = selectedButton.GemCost <= player.Gems;

            if (canAfford && !selectedButton.HasBeenPurchased)
            {
                selectedButton.BuyItem();
                player.Gems -= selectedButton.GemCost;
                UpdateGemsText();
            }
            else if (!canAfford && !selectedButton.HasBeenPurchased)
            {
                UIManager.Instance.SetAndFadeAlertText("You do not have enough gems!");
            }
        }

        void SetCamera()
        {
            PathCamera.transform.localPosition = selectedButton.PathController.CameraLookAt.transform.localPosition;
        }
    }
}