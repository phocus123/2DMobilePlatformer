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
        [SerializeField] int attack01Damage;
        [SerializeField] int attack02Damage;
        [SerializeField] int attack03Damage;

        [SerializeField] int gems;
        [SerializeField] float health;
        [SerializeField] float stamina;

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
                switch (playerAttack.AttackIndex)
                {
                    case 1:
                        damage = attack01Damage;
                        break;
                    case 2:
                        damage = attack02Damage;
                        break;
                    case 3:
                        damage = attack03Damage;
                        break;
                    default:
                        damage = attack01Damage;
                        break;
                }
                return damage;
            }
        }

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

        void Awake()
        {
            anim = GetComponent<PlayerAnimation>();
            sprite = GetComponentInChildren<SpriteRenderer>();
            playerMovement = GetComponent<PlayerMovement>();
            playerAttack = GetComponent<PlayerAttack>();
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