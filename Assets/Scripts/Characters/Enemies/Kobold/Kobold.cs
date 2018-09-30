using UnityEngine;
using PHOCUS.UI;

namespace PHOCUS.Character
{
    public class Kobold : Enemy, IDamageable
    {
        public void DealDamage(float damageAmount)
        {
            if (canDamage)
            {
                bool characterDies = (Health - damageAmount <= 0);
                Health = Mathf.Clamp(Health - damageAmount, 0, maxHealth);
                canDamage = false;
                anim.SetTrigger("Hit");
                UIManager.Instance.UpdateEnemyHealth(HealthBar, Health / maxHealth);
                UIManager.Instance.TriggerCombatText(new Vector3(transform.position.x, transform.position.y - 0.5f, 0f), damageAmount, CombatTextType.NormalDamage);

                StopCoroutine(ChasePlayer());
                StopCoroutine(AttackTarget());
                StartCoroutine(ResetBool());

                if (isAlive && characterDies)
                {
                    RemoveCollisions();
                    isAlive = false;
                    OnEnemyDeath(this);
                    anim.SetTrigger("Death");
                    float animLength = anim.GetCurrentAnimatorClipInfo(0).Length;
                    DropLoot();
                    Destroy(gameObject, animLength);
                }
            }
        }
    }
}