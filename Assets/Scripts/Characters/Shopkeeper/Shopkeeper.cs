using PHOCUS.UI;
using UnityEngine;
using PHOCUS.Utilities;

namespace PHOCUS.Character
{
    public class Shopkeeper : MonoBehaviour
    {
        public Shop shop;
        public Canvas worldSpaceCanvas;
        public GameObject InteractPrefab;
        public InteractableRaycaster ray;

        bool inTrigger;

        void Awake()
        {
            shop = FindObjectOfType<Shop>(); //TODO Get from GameManager singleton when it is created.
            worldSpaceCanvas = GameObject.FindGameObjectWithTag("WorldSpaceCanvas").GetComponent<Canvas>(); //TODO Get from GameManager singleton when it is created.
            ray = FindObjectOfType<InteractableRaycaster>(); //TODO Get from GameManager singleton when it is created.

            ray.OnInteractableClicked += OnClicked;
        }

        void OnClicked()
        {
            if (!shop.isEnabled)
                if (inTrigger)
                    shop.ToggleShop();
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
    }
}