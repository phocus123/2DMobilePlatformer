using PHOCUS.Character;
using UnityEngine;

public class Spike : MonoBehaviour
{
    int damage = 500;

    void OnTriggerEnter2D(Collider2D collision)
    {
        var damageable = collision.GetComponent<IDamageable>();

        if (damageable is IDamageable)
        {
            damageable.DealDamage(damage);
        }
    }
}
