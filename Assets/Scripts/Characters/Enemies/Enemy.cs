using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using PHOCUS.Core;

namespace PHOCUS.Character
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        public int Gems;
        public float MoveSpeed;
        public float AttackSpeed;
        public float AttackRange;
        public int Damage;
        public float maxHealth;
        public Image HealthBar;
        public GameObject gemPrefab;

        SpriteRenderer sprite;
        Animator anim;
        PlayerController player;
        Vector3 currentTarget;

        public enum EnemyState { Attack, Follow }
        public EnemyState State;

        float distanceToPlayer;
        float lastHitTime;
        bool inAttackRange;
        bool canDamage = true;
        bool isAlive = true;

        public float Health { get; set; }

        void Start()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
            anim = GetComponentInChildren<Animator>();
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

            Health = maxHealth;
        }

        void Update()
        {
            distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            inAttackRange = distanceToPlayer <= AttackRange;
            QueryStates();
        }

        public void DealDamage(float damageAmount)
        {
            if (canDamage)
            {
                canDamage = false;
                anim.SetTrigger("Hit");
                bool characterDies = (Health - damageAmount <= 0);
                Health = Mathf.Clamp(Health - damageAmount, 0, maxHealth);
                UIManager.Instance.UpdateEnemyHealth(HealthBar, Health / maxHealth);

                StopCoroutine(ChasePlayer());
                StopCoroutine(AttackTarget());
                StartCoroutine(ResetBool());

                if (characterDies)
                {
                    isAlive = false;
                    anim.SetTrigger("Death");
                    float animLength = anim.GetCurrentAnimatorClipInfo(0).Length;
                    DropLoot();
                    Destroy(gameObject, animLength);
                }
            }
        }

        void QueryStates()
        {
            if (canDamage && isAlive)
            {
                if (inAttackRange && State != EnemyState.Attack)
                    ExecuteAttackState();
                if (!inAttackRange && State != EnemyState.Follow)
                    ExecuteFollowState();
            }
        }

        void ExecuteAttackState()
        {
            State = EnemyState.Attack;
            StopAllCoroutines();
            StartCoroutine(AttackTarget());
        }

        void ExecuteFollowState()
        {
            State = EnemyState.Follow;
            StopAllCoroutines();
            StartCoroutine(ChasePlayer());
        }

        IEnumerator ChasePlayer()
        {
            while (distanceToPlayer >= AttackRange)
            {
                currentTarget = player.transform.position;
                transform.position = Vector2.MoveTowards(transform.position, new Vector3(currentTarget.x, transform.position.y, 0), MoveSpeed * Time.deltaTime);
                HandleMoveDirection();
                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator AttackTarget()
        {
            bool attackerStillAlive = Health >= Mathf.Epsilon;

            while (attackerStillAlive)
            {
                bool isTimeToHitAgain = Time.time - lastHitTime > AttackSpeed;

                if (isTimeToHitAgain)
                {
                    anim.SetTrigger("Attack");
                    lastHitTime = Time.time;
                }
                yield return new WaitForSeconds(AttackSpeed);
            }
        }

        void HandleMoveDirection()
        {
            var direction = transform.position - player.transform.position;

            if (direction.x < 0)
                sprite.flipX = true;
            else
                sprite.flipX = false;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
        }

        void DropLoot()
        {
            var gem = Instantiate(gemPrefab, transform.position, Quaternion.identity);
            gem.GetComponent<Gem>().Gems = Gems;
        }


        IEnumerator ResetBool()
        {
            yield return new WaitForSeconds(.5f);
            canDamage = true;
        }
    }
}