using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using PHOCUS.UI;
using System;

namespace PHOCUS.Character
{
    public class Enemy : MonoBehaviour
    {
        public int Gems;
        public float MoveSpeed;
        public float AttackSpeed;
        public float AttackRange;
        public int Damage;
        public float maxHealth;
        public Image HealthBar;
        public GameObject GemPrefab;

        protected Animator anim;
        protected bool canDamage = true;
        protected bool isAlive = true;

        SpriteRenderer sprite;
        PlayerController player;
        Vector3 currentTarget;

        public enum EnemyState { Attack, Follow }
        public EnemyState State;

        float distanceToPlayer;
        float lastHitTime;
        bool inAttackRange;

        public float Health { get; set; }
        public Action<Enemy> OnEnemyDeath = delegate { };

        void Start()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
            anim = GetComponentInChildren<Animator>();
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

            Health = maxHealth;
        }

        void Update()
        {
            GetRanges();
            QueryStates();
            HandleFlipX();
        }

        protected IEnumerator ChasePlayer()
        {
            while (distanceToPlayer >= AttackRange)
            {
                currentTarget = player.transform.position;
                transform.position = Vector2.MoveTowards(transform.position, new Vector3(currentTarget.x, transform.position.y, 0), MoveSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }

        protected  IEnumerator AttackTarget()
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

        protected IEnumerator ResetBool()
        {
            yield return new WaitForSeconds(.2f);
            canDamage = true;
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
            if (canDamage && isAlive)
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
            StopAllCoroutines();
            StartCoroutine(AttackTarget());
        }

        void ExecuteFollowState()
        {
            anim.SetBool("Move", true);
            State = EnemyState.Follow;
            StopAllCoroutines();
            StartCoroutine(ChasePlayer());
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
        }
    }
}