using PHOCUS.Utilities;
using UnityEngine;

namespace PHOCUS.Character
{
    public class Coin : MonoBehaviour, ICollectable
    {
        public int Coins;

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
            GameManager.Instance.CoinCount += Coins;
            player.AddCoins(Coins);
            Destroy(parentRenderer.gameObject);
        }
    }
}