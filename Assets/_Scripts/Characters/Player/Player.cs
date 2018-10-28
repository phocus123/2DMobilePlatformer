using System.Collections;
using UnityEngine;
using PHOCUS.UI;

namespace PHOCUS.Character
{
    public class Player : MonoBehaviour, IDamageable
    {
        public PlayerStat AttackStat;
        public PlayerStat HealthStat;
        public PlayerStat StaminaStat;
        
        [SerializeField] int gems;
        [SerializeField] int coins;
        [SerializeField] float currentHealth;
        [SerializeField] float currentStamina;

        PlayerAnimation anim;
        PlayerMovement playerMovement;
        PlayerAttack playerAttack;
        SpriteRenderer sprite;
        bool canDamage = true;
        int damage;

        public bool IsAlive { get { return Health > 0; } }

        public int Damage
        {
            get
            {
                var attackStat = AttackStat as AttackStat;

                switch (playerAttack.AttackIndex)
                {
                    case 1:
                        damage = attackStat.Attack01Damage;
                        break;
                    case 2:
                        damage = attackStat.Attack02Damage;
                        break;
                    case 3:
                        damage = attackStat.Attack03Damage;
                        break;
                    default:
                        damage = attackStat.Attack01Damage;
                        break;
                }
                return damage;
            }
        }

        public float Health
        {
            get { return currentHealth; }
            set
            {
                currentHealth = value;
                currentHealth = Mathf.Clamp(currentHealth, 0 , (HealthStat as HealthStat).MaxHealth);
                UIManager.Instance.UpdatePlayerHealth(currentHealth, (HealthStat as HealthStat).MaxHealth);
            }
        }

        public float Stamina
        {
            get { return currentStamina; }
            set
            {
                currentStamina = value;
                currentStamina = Mathf.Clamp(currentStamina, 0, (StaminaStat as StaminaStat).MaxStamina);
                UIManager.Instance.UpdateStamina(currentStamina, (StaminaStat as StaminaStat).MaxStamina);
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

        public int Coins
        {
            get { return coins; }
            set
            {
                coins = value;
                UIManager.Instance.UpdateCoinCount(coins);
            }
        }

        void Awake()
        {
            anim = GetComponent<PlayerAnimation>();
            sprite = GetComponentInChildren<SpriteRenderer>();
            playerMovement = GetComponent<PlayerMovement>();
            playerAttack = GetComponent<PlayerAttack>();
            Health = (HealthStat as HealthStat).MaxHealth;
            Stamina = (StaminaStat as StaminaStat).MaxStamina;
            UIManager.Instance.UpdateGemCount(gems);
        }

        void Update()
        {
            Stamina += (StaminaStat as StaminaStat).RegenRate * Time.deltaTime;
            Stamina = Mathf.Clamp(Stamina, 0, (StaminaStat as StaminaStat).MaxStamina);
            UIManager.Instance.UpdateStamina(Stamina, (StaminaStat as StaminaStat).MaxStamina);
        }

        public void DealDamage(float damageAmount)
        {
            if (canDamage)
            {
                canDamage = false;
                anim.Hit();
                bool characterDies = (Health - damageAmount <= 0);
                Health = Mathf.Clamp(Health - damageAmount, 0, (HealthStat as HealthStat).MaxHealth);
                UIManager.Instance.UpdatePlayerHealth(Health, (HealthStat as HealthStat).MaxHealth);
                StartCoroutine(ResetBool());

                if (characterDies)
                {
                    float animLength = anim.Die();
                    Destroy(gameObject, animLength);
                }
            }
        }

        public void TriggerFallingDeath()
        {
            Health = 0;
            sprite.sprite = null;
            Time.timeScale = 0;
        }

        public bool CheckStamina(float amount)
        {
            bool temp = amount < Stamina;

            if (temp)
                return temp;
            else
            {
                UIManager.Instance.SetAndFadeAlertText("You do not have enough stamina");
                return temp;
            }
        }

        public void AddGems(int amount)
        {
            Gems += amount;
        }

        public void AddCoins(int amount)
        {
            Coins += amount;
        }

        public void TogglePlayerActions()
        {
            playerMovement.IsDisabled = !playerMovement.IsDisabled;
            playerAttack.IsDisabled = !playerAttack.IsDisabled;
            playerMovement.StopMoving();
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