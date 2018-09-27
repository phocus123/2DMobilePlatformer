using UnityEngine;
using PHOCUS.UI;
using PHOCUS.Utilities;

namespace PHOCUS.Character
{
    public class Shopkeeper : MonoBehaviour
    {
        public Shop shop;

        void Awake()
        {
            shop = FindObjectOfType<Shop>(); //TODO Get from GameManager singleton when it is created.
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
                shop.ToggleShop();

        }
    }
}