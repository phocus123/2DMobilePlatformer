using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHOCUS.Character
{
    public class Attack : MonoBehaviour
    {
        int damage;

        void Start()
        {
            if (GetComponentInParent<Enemy>())
                damage = GetComponentInParent<Enemy>().Damage;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (GetComponentInParent<Player>())
                damage = GetComponentInParent<Player>().Damage;

            var damageable = collision.GetComponent<IDamageable>();

            if (damageable is IDamageable)
            {
                damageable.DealDamage(damage);
            }
        }
    }
}