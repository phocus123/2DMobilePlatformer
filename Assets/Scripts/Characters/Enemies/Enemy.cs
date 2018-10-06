using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using PHOCUS.UI;
using System;

namespace PHOCUS.Character
{
    public class Enemy : MonoBehaviour
    {
        public GameObject GemPrefab;
        public GameObject healthPrefab;
        public GameObject staminaPrefab;
        [Header("Stats")]
        public int Gems;
        public float MoveSpeed;
        public float AttackSpeed;
        public float AttackRange;
        public int Damage;
        public float maxHealth;
        public Image HealthBar;
        [Header("States")]
        public bool IsAttacking;
        public bool IsAlive = true;

        protected Animator anim;
        protected bool canDamage = true;

        SpriteRenderer sprite;
        Player player;
        Vector3 currentTarget;

        public enum EnemyState { Attack, Follow, Wait }
        public EnemyState State;

        float lastHitTime;
        float distanceToPlayer;
        bool inAttackRange;
        const float healthPotDropRate = 0.45f;
        const float stamPotDropRate = 0.8f;

        public float Health { get; set; }
        public Action<Enemy> OnEnemyDeath = delegate { };

        void Start()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
            anim = GetComponentInChildren<Animator>();
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            Health = maxHealth;
        }

        void Update()
        {
            if (player.IsAlive)
            {
                GetRanges();
                QueryStates();
                HandleFlipX();
            }
            else
                StopAllCoroutines();
        }

        protected IEnumerator ChasePlayer()
        {
            while (distanceToPlayer >= AttackRange && !IsAttacking)
            {
                currentTarget = player.transform.position;
                transform.position = Vector2.MoveTowards(transform.position, new Vector3(currentTarget.x, transform.position.y, 0), MoveSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }

        protected virtual IEnumerator AttackTarget()
        {
            IsAttacking = true;
            bool isTimeToHitAgain = Time.time - lastHitTime > AttackSpeed;

            if (isTimeToHitAgain)
            {
                anim.SetTrigger("Attack");
                lastHitTime = Time.time;
            }

            yield return new WaitForSeconds(AttackSpeed);
            State = EnemyState.Wait;
            IsAttacking = false;
            anim.SetBool("Move", true);
        }

        protected IEnumerator ResetBool()
        {
            yield return new WaitForSeconds(.2f);
            canDamage = true;
        }

        protected void DropLoot()
        {
            var gem = Instantiate(GemPrefab, transform.position, Quaternion.identity);
            gem.GetComponentInChildren<Gem>().Gems = Gems;

            int randomValue = UnityEngine.Random.Range(0, 100);

            if (randomValue <= (healthPotDropRate * 100))
                Instantiate(healthPrefab, transform.position, Quaternion.identity);
            if (randomValue <= (stamPotDropRate * 100) && randomValue > (healthPotDropRate * 100))
                Instantiate(staminaPrefab, transform.position, Quaternion.identity);
        }

        protected void RemoveCollisions()
        {
            BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
            GetComponent<Rigidbody2D>().gravityScale = 0;

            foreach (BoxCollider2D col in colliders)
                col.enabled = false;
        }

        void GetRanges()
        {
            distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            var direction = player.transform.position - transform.position;
            anim.SetFloat("Horizontal", direction.normalized.x);
            inAttackRange = distanceToPlayer <= AttackRange;
        }

        void QueryStates()
        {
            if (IsAlive)
            {
                if (inAttackRange && State != EnemyState.Attack)
                    ExecuteAttackState();
                if (!inAttackRange && State != EnemyState.Follow)
                    ExecuteFollowState();
            }
        }

        void HandleFlipX()
        {
            if (anim.GetFloat("Horizontal") > 0)
                sprite.flipX = true;
            else if (anim.GetFloat("Horizontal") < 0)
                sprite.flipX = false;
        }

        void ExecuteAttackState()
        {
            anim.SetBool("Move", false);
            State = EnemyState.Attack;
            StopCoroutine(ChasePlayer());
            StartCoroutine(AttackTarget());
        }

        void ExecuteFollowState()
        {
            if (!IsAttacking)
            {
                anim.SetBool("Move", true);
                State = EnemyState.Follow;
                StopCoroutine(AttackTarget());
                StartCoroutine(ChasePlayer());
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
        }
    }
}