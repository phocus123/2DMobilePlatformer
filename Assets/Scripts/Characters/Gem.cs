using UnityEngine;
using PHOCUS.Character;

public class Gem : MonoBehaviour
{
    public int Gems;

    bool isTriggered = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isTriggered)
        {
            isTriggered = true;

            var player = collision.GetComponent<Player>();
            player.AddGems(Gems);
            Destroy(gameObject);
        }
    }
}
