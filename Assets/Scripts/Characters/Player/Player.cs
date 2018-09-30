using System.Collections;
using UnityEngine;
using PHOCUS.UI;

namespace PHOCUS.Character
{
    public class Player : MonoBehaviour, IDamageable
    {
        public float MaxHealth;
        public float MaxStamina;
        public float StaminaRegenAmount = 0.5f;
        public int Damage;

        [SerializeField] int gems;
        [SerializeField] float health;
        [SerializeField] float stamina;

        PlayerAnimator anim;
        PlayerController playerController;
        bool canDamage = true;

        public bool IsAlive { get { return Health > 0; } }

        public float Health
        {
            get { return health; }
            set
            {
                health = value;
                health = Mathf.Clamp(health, 0 , MaxHealth);
                UIManager.Instance.UpdatePlayerHealth(health, MaxHealth);
            }
        }

        public float Stamina
        {
            get { return stamina; }
            set
            {
                stamina = value;
                stamina = Mathf.Clamp(stamina, 0, MaxStamina);
                UIManager.Instance.UpdateStamina(stamina, MaxStamina);
            }
        }

        public int Gems
        {
            get { return gems; }
            set
            {
                gems = value;
                UIManager.Instance.UpdateGemCount(gems);
            }
        }

        void Start()
        {
            anim = GetComponent<PlayerAnimator>();
            playerController = GetComponent<PlayerController>();
            Health = MaxHealth;
            Stamina = MaxStamina;
            UIManager.Instance.UpdateGemCount(gems);
        }

        void Update()
        {
            Stamina += StaminaRegenAmount * Time.deltaTime;
            Stamina = Mathf.Clamp(Stamina, 0, MaxStamina);
            UIManager.Instance.UpdateStamina(Stamina, MaxStamina);
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
        }

        public void TogglePlayerActions()
        {
            playerController.isDisabled = !playerController.isDisabled;
            playerController.StopMoving();
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