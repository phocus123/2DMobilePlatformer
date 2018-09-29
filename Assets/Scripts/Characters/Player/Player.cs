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

        PlayerAnimator anim;
        PlayerController playerController;
        bool canDamage = true;

        public float Health { get; set; }
        public float Stamina { get; set; }

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
            playerController.ActionsDisabled = !playerController.ActionsDisabled;
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