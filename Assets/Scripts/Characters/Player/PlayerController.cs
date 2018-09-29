using System;
using System.Collections;
using UnityEngine;

namespace PHOCUS.Character
{
    public class PlayerController : MonoBehaviour
    {
        public float MovementSpeed = 3f;
        public float JumpForce = 5f;
        public float DoubleJumpForce = 2.5f;
        public float AttackSpeed = 1.5f;
        public float AttackStaminaCost = 7f;
        public float JumpStaminaCost = 2f;
        public bool isAttacking;
        public bool isGrounded;
        public bool ActionsDisabled;
        public bool CanDoubleJump;

        Rigidbody2D rigidBody;
        PlayerAnimator playerAnim;
        SpriteRenderer sprite;
        Player player;

        bool resetJump;
        int attackIndex = 1;
        float lastHitTime;
        float horizontalValue;

        const float GROUNDED_RAY_DISTANCE = 1.5f;
        const int GROUND_LAYER = 8;

        void Start()
        {
            rigidBody = GetComponentInChildren<Rigidbody2D>();
            sprite = GetComponentInChildren<SpriteRenderer>();
            playerAnim = GetComponent<PlayerAnimator>();
            player = GetComponent<Player>();
        }

        void Update()
        {
            if (!ActionsDisabled)
            {
                HandleAttack();
                HandleMovement();
            }
        }

        public void StopMoving()
        {
            rigidBody.velocity = Vector2.zero;
            playerAnim.StopMoving();
        }

        bool CheckStamina(float amount)
        {
            return amount < player.Stamina;
        }

        void HandleMovement()
        {
            horizontalValue = Input.GetAxisRaw("Horizontal");
            isGrounded = IsGrounded();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded && CheckStamina(JumpStaminaCost))
                {
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, JumpForce);
                    playerAnim.Jump(true);
                    StartCoroutine(ResetJumproutine());
                    player.Stamina -= JumpStaminaCost;
                    CanDoubleJump = true;
                }
                else
                {
                    if (CanDoubleJump)
                    {
                        playerAnim.DoubleJump(true);
                        CanDoubleJump = false;
                        rigidBody.velocity = new Vector2(rigidBody.velocity.x, DoubleJumpForce);
                        playerAnim.Jump(true);
                        StartCoroutine(ResetJumproutine());
                        player.Stamina -= JumpStaminaCost;
                    }
                }
            }

                if (!isGrounded && Input.GetMouseButtonDown(0) && CheckStamina(AttackStaminaCost))
            {
                playerAnim.AirAttack();
                player.Stamina -= AttackStaminaCost;
            }

            rigidBody.velocity = new Vector2((horizontalValue * MovementSpeed), rigidBody.velocity.y);

            playerAnim.Move(horizontalValue);
        }

        void HandleAttack()
        {
            if (Input.GetMouseButtonDown(0) && isGrounded && !isAttacking && CheckStamina(AttackStaminaCost))
            {
                isAttacking = true;
                StartCoroutine(AttackTarget());
                player.Stamina -= AttackStaminaCost;
            }
        }

        IEnumerator AttackTarget()
        {
            playerAnim.Attack(attackIndex, horizontalValue);
            yield return new WaitForSeconds(AttackSpeed);
            isAttacking = false;
            attackIndex++;

            if (attackIndex > 3)
                attackIndex = 1;
        }

        bool IsGrounded()
        {
            LayerMask groundLayer = 1 << GROUND_LAYER;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, GROUNDED_RAY_DISTANCE, groundLayer);
            //Debug.DrawRay(transform.position, Vector2.down, Color.red, 1.5f);

            if (hit.collider != null)
            {
                if (!resetJump)
                {
                    playerAnim.Jump(false);
                    playerAnim.DoubleJump(false);
                    return true;
                }
            }

            return false;
        }

        IEnumerator ResetJumproutine()
        {
            resetJump = true;
            yield return new WaitForSeconds(.1f);
            resetJump = false;
        }
    }
}