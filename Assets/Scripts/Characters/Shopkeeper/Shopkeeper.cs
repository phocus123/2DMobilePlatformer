using PHOCUS.UI;
using UnityEngine;
using PHOCUS.Utilities;

namespace PHOCUS.Character
{
    public class Shopkeeper : MonoBehaviour
    {
        public Canvas worldSpaceCanvas;
        public GameObject InteractPrefab;
        public Dialogue dialogue;

        bool inTrigger;

        void Awake()
        {
            worldSpaceCanvas = UIManager.Instance.WorldSpaceCanvas;
            dialogue = UIManager.Instance.Dialogue;
            InteractableRaycaster ray = GameManager.Instance.interactableRaycaster;

            ray.OnInteractableClicked += OnClicked;
            dialogue.OnResetPlatform += DestroyShopkeeper;
        }

        void OnClicked()
        {
            if (inTrigger)
                dialogue.ToggleDialogue();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            inTrigger = true;

            if (collision.tag == "Player")
            {
                Instantiate(InteractPrefab, transform.position, Quaternion.identity, worldSpaceCanvas.transform);
            }
        }

        void OnTriggerStay2D(Collider2D collision)
        {
            inTrigger = true;
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            inTrigger = false;
        }

        void DestroyShopkeeper()
        {
            Destroy(transform.gameObject);
            dialogue.OnResetPlatform -= DestroyShopkeeper;
        }
    }
}