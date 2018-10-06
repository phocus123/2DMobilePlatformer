using UnityEngine;
using PHOCUS.UI;
using PHOCUS.Utilities;
using PHOCUS.Environment;
using System.Collections;

namespace PHOCUS.Character
{
    public class Shopkeeper : MonoBehaviour
    {
        public Dialogue Dialogue;
        public PlatformController Platform;
        public InteractableIndicator Indicator;
        public bool IsVisible;

        SpriteRenderer spriteRenderer;
        BoxCollider2D boxCollider;
        bool inTrigger;

        void Awake()
        {
            Dialogue = UIManager.Instance.Dialogue;
            Indicator = GetComponentInChildren<InteractableIndicator>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            boxCollider = GetComponent<BoxCollider2D>();
            InteractableRaycaster ray = GameManager.Instance.InteractableRaycaster;

            ray.OnInteractableClicked += OnClicked;
            Dialogue.OnResetPlatform += DestroyShopkeeper;
        }

        public void SpawnShopkeeper()
        {
            StartCoroutine(ToggleShopkeeper(true));
        }

        void OnClicked()
        {
            if (inTrigger)
            {
                Dialogue.ToggleDialogue();
                Dialogue.Shop.Platform = Platform;
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

                if (Dialogue.IsActive)
                    Dialogue.ToggleDialogue();
            }
        }

        void DestroyShopkeeper()
        {
            if (Platform.IsPlatformActive)
            {
                Destroy(transform.gameObject);
                Dialogue.OnResetPlatform -= DestroyShopkeeper;
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