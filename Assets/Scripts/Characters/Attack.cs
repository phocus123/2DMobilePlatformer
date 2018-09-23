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
            if(GetComponentInParent<Player>())
                damage = GetComponentInParent<Player>().Damage;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            var damageable = collision.GetComponent<IDamageable>();

            if (damageable is IDamageable)
            {
                damageable.DealDamage(damage);
            }
        }
    }
}