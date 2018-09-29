using UnityEngine;
using PHOCUS.UI;
using PHOCUS.Utilities;

namespace PHOCUS.Character
{
    public class Shopkeeper : MonoBehaviour
    {
        public Shop shop;
        public Canvas worldSpaceCanvas;
        public GameObject InteractPrefab;

        Player player;
        bool inTrigger;

        void Awake()
        {
            shop = FindObjectOfType<Shop>(); //TODO Get from GameManager singleton when it is created.
            worldSpaceCanvas = GameObject.FindGameObjectWithTag("WorldSpaceCanvas").GetComponent<Canvas>(); //TODO Get from GameManager singleton when it is created.
            player = FindObjectOfType<Player>();  //TODO Get from GameManager singleton when it is created.
        }

        void Update()
        {
            if (!shop.isEnabled)
                if (inTrigger)
                {
                    LayerMask interactableLayer = 1 << 15;
                    float zPlane = 0f;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Vector3 posAtZ = ray.origin + ray.direction * (zPlane - ray.origin.z) / ray.direction.z;
                    Vector2 point = new Vector2(posAtZ.x, posAtZ.y);
                    RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero, Mathf.Infinity, interactableLayer);

                    if (hit.collider != null && Input.GetMouseButtonDown(0))
                    {
                        shop.ToggleShop();
                    }
                }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            inTrigger = true;

            if (collision.tag == "Player")
            {
                Instantiate(InteractPrefab, transform.position, Quaternion.identity, worldSpaceCanvas.transform);
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            inTrigger = false;
        }
    }
}