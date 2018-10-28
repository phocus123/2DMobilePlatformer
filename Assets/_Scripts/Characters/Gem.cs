using PHOCUS.Utilities;
using UnityEngine;

namespace PHOCUS.Character
{
    public class Gem : MonoBehaviour, ICollectable
    {
        public int Gems;

        bool isTriggered;
        SpriteRenderer parentRenderer;

        void Start()
        {
            parentRenderer = GetComponentInParent<SpriteRenderer>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player" && !isTriggered)
            {
                isTriggered = true;
                var player = collision.GetComponent<Player>();
                Collect(player);
            }
        }

        public void Collect(Player player)
        {
            GameManager.Instance.TotalGemCount += Gems;
            player.AddGems(Gems);
            Destroy(parentRenderer.gameObject);
        }
    }
}