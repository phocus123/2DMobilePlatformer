using System;
using System.Collections;
using UnityEngine;

namespace PHOCUS.Character
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Stats")]
        public float MovementSpeed;
        public float DashSpeed;
        public float DashLength;
        public float JumpForce;
        public float DoubleJumpForce;
        public float AttackSpeed;
        public float AttackStaminaCost;
        public float JumpStaminaCost;
        public float DashStaminaCost;
        [Header("States")]
        public bool isAttacking;
        public bool isGrounded;
        public bool isDisabled;
        public bool isSliding;
        public bool CanDoubleJump;
        public bool CanDashInAir;

        Rigidbody2D rigidBody;
        PlayerAnimator playerAnim;
        Player player;

        bool resetJump;
        int attackIndex = 1;
        float lastHitTime;
        float horizontalValue;
        LayerMask playerLayer = 14;
        LayerMask invincibleLayer = 17;

        const float GROUNDED_RAY_DISTANCE = 1.5f;
        const int GROUND_LAYER = 8;

        void Start()
        {
            rigidBody = GetComponentInChildren<Rigidbody2D>();
            playerAnim = GetComponent<PlayerAnimator>();
            player = GetComponent<Player>();
        }

        void Update()
        {
            if (!isDisabled)
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
                    StartCoroutine(ResetJump());
                    player.Stamina -= JumpStaminaCost;
                    CanDoubleJump = true;
                }
                else
                {
                    if (CanDoubleJump)
                    {
                        playerAnim.DoubleJump(true);
                        CanDoubleJump = false;
                        CanDashInAir = true;
                        rigidBody.velocity = new Vector2(rigidBody.velocity.x, DoubleJumpForce);
                        StartCoroutine(ResetJump());
                        player.Stamina -= JumpStaminaCost;
                    }
                }
            }

            if (!isGrounded && Input.GetMouseButtonDown(0) && CheckStamina(AttackStaminaCost))
            {
                playerAnim.AirAttack();
                player.Stamina -= AttackStaminaCost;
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (!isSliding && CheckStamina(DashStaminaCost))
                {
                    if (isGrounded || CanDashInAir)
                    {
                        isSliding = true;
                        StartCoroutine(Dash());
                    }
                }
            }

            rigidBody.velocity = new Vector2((horizontalValue * MovementSpeed), rigidBody.velocity.y);
            playerAnim.Move(horizontalValue);
        }

        IEnumerator Dash()
        {
            gameObject.layer = invincibleLayer;
            float dashTime = DashLength;
            playerAnim.Slide(true);
            player.Stamina -= DashStaminaCost;

            while (dashTime >= 0)
            {
                dashTime -= Time.deltaTime;
                rigidBody.velocity = new Vector2(horizontalValue * DashSpeed, rigidBody.velocity.y);
                yield return null;
            }
        
            isSliding = false;
            playerAnim.Slide(false);
            gameObject.layer = playerLayer;
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
                    CanDashInAir = false;
                    return true;
                }
            }

            return false;
        }

        IEnumerator ResetJump()
        {
            resetJump = true;
            yield return new WaitForSeconds(.1f);
            resetJump = false;
        }
    }
}