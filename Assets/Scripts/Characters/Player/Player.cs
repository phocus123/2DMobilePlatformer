using PHOCUS.Core;
using System.Collections;
using UnityEngine;

namespace PHOCUS.Character
{
    public class Player : MonoBehaviour, IDamageable
    {
        public float MaxHealth;
        public int Damage;
        public int Gems;

        PlayerAnimator anim;
        bool canDamage = true;

        public float Health { get; set; }

        void Start()
        {
            anim = GetComponent<PlayerAnimator>();
            Health = MaxHealth;
        }

        public void DealDamage(float damageAmount)
        {
            if (canDamage)
            {
                canDamage = false;
                anim.Hit();
                bool characterDies = (Health - damageAmount <= 0);
                Health = Mathf.Clamp(Health - damageAmount, 0, MaxHealth);
                UIManager.Instance.UpdatePlayerHealth(Health, MaxHealth);
                StartCoroutine(ResetBool());

                if (characterDies)
                {
                    float animLength = anim.Die();
                    Destroy(gameObject, animLength);
                }
            }
        }

        public void AddGems(int amount)
        {
            Gems += amount;
            UIManager.Instance.UpdateGemCount(Gems);
        }

        bool isKillingBlow(float damage)
        {
            return Health - damage <= 0;
        }

        IEnumerator ResetBool()
        {
            yield return new WaitForSeconds(.5f);
            canDamage = true;
        }
    }
}