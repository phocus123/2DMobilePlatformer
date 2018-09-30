using UnityEngine;
using PHOCUS.Character;

public class Gem : MonoBehaviour
{
    public int Gems;

    bool isTriggered = false;
    SpriteRenderer parent;

    void Start()
    {
        parent = GetComponentInParent<SpriteRenderer>();    
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isTriggered)
        {
            isTriggered = true;
            var player = collision.GetComponent<Player>();
            player.AddGems(Gems);
            Destroy(parent.gameObject);
        }
    }
}
