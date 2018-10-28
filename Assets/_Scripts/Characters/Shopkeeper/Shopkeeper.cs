using UnityEngine;
using PHOCUS.UI;
using PHOCUS.Utilities;
using PHOCUS.Environment;
using System.Collections;
using System.Collections.Generic;

namespace PHOCUS.Character
{
    public class Shopkeeper : MonoBehaviour
    {
        public DialoguePanel DialoguePanel;
        public PlatformController Platform;
        public InteractableIndicator Indicator;
        public bool IsVisible;
        public bool LoadedShopPaths;
        public List<bool> ItemsPurchased = new List<bool>();

        SpriteRenderer spriteRenderer;
        BoxCollider2D boxCollider;
        bool inTrigger;

        void Awake()
        {
            DialoguePanel = UIManager.Instance.DialoguePanel;
            Indicator = GetComponentInChildren<InteractableIndicator>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            boxCollider = GetComponent<BoxCollider2D>();
            InteractableRaycaster ray = GameManager.Instance.InteractableRaycaster;

            ray.OnInteractableClicked += OnClicked;
            DialoguePanel.OnResetPlatform += DestroyShopkeeper;
        }

        public void SpawnShopkeeper()
        {
            StartCoroutine(ToggleShopkeeper(true));
        }

        void OnClicked()
        {
            if (inTrigger)
            {
                DialoguePanel.ToggleDialogue();
                DialoguePanel.Shop.Platform = Platform;
                DialoguePanel.Shop.Shopkeeper = this;
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            inTrigger = true;

            if (collision.tag == "Player")
            {
                if (!IsVisible)
                {
                    StopAllCoroutines();
                    StartCoroutine(ToggleShopkeeper(true));
                }
            }
        }

        void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag == "Player")
                inTrigger = true;
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                inTrigger = false;

                if (IsVisible)
                {
                    StopAllCoroutines();
                    StartCoroutine(ToggleShopkeeper(false));
                }

                if (DialoguePanel.IsActive)
                    DialoguePanel.ToggleDialogue();
            }
        }

        void DestroyShopkeeper()
        {
            if (Platform.IsPlatformActive)
            {
                Destroy(transform.gameObject);
                DialoguePanel.OnResetPlatform -= DestroyShopkeeper;
            }
        }

        IEnumerator ToggleShopkeeper(bool isVisible) 
        {
            Platform.Portal.GetComponentInChildren<Animator>().SetBool("Animate", true);

            yield return new WaitForSeconds(1f);

            spriteRenderer.enabled = spriteRenderer.enabled ? false : true;
            Platform.Portal.GetComponentInChildren<Animator>().SetBool("Animate", false);

            if(!IsVisible)
                Indicator.ToggleIndicator();

            IsVisible = isVisible;
            boxCollider.enabled = true;
        }
    }
}