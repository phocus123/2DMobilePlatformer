using System.Collections;
using UnityEngine;
using PHOCUS.Utilities;
using PHOCUS.UI;

namespace PHOCUS.Character
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Stats")]
        public float MovementSpeed;
        public float DashSpeed;
        public float DashLength;
        public float JumpForce;
        public float DoubleJumpForce;
        public float JumpStaminaCost;
        public float DashStaminaCost;
        public float HorizontalValue;
        [Header("States")]
        public bool IsGrounded;
        public bool IsDisabled;
        public bool IsSliding;
        public bool CanDoubleJump;
        public bool CanDashInAir;

        Rigidbody2D rigidBody;
        PlayerAnimation playerAnim;
        Player player;

        bool resetJump;
        float lastHitTime;
        LayerMask playerLayer = 14;
        LayerMask invincibleLayer = 17;

        const float GROUNDED_RAY_DISTANCE = 1.5f;
        const int GROUND_LAYER = 8;

        void Awake()
        {
            rigidBody = GetComponentInChildren<Rigidbody2D>();
            playerAnim = GetComponent<PlayerAnimation>();
            player = GetComponent<Player>();
        }

        void Update()
        {
            if (!IsDisabled)
                HandleMovement();
        }

        public void StopMoving()
        {
            rigidBody.velocity = Vector2.zero;
            playerAnim.StopMoving();
        }

        void HandleMovement()
        {
            HorizontalValue = Input.GetAxisRaw("Horizontal");
            IsGrounded = CheckIsGrounded();

            if (Input.GetKeyDown(KeyCode.Space))
                HandleJump();

            if (Input.GetMouseButtonDown(1))
            {
                if (!IsSliding && player.CheckStamina(DashStaminaCost))
                    if (IsGrounded || CanDashInAir)
                    {
                        IsSliding = true;
                        StartCoroutine(Dash());
                    }
            }

            rigidBody.velocity = new Vector2((HorizontalValue * MovementSpeed), rigidBody.velocity.y);
            playerAnim.Move(HorizontalValue);
        }

        void HandleJump()
        {
            if (IsGrounded && player.CheckStamina(JumpStaminaCost))
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

        IEnumerator Dash()
        {
            gameObject.layer = invincibleLayer;
            float dashTime = DashLength;
            playerAnim.Slide(true);
            player.Stamina -= DashStaminaCost;

            while (dashTime >= 0)
            {
                dashTime -= Time.deltaTime;
                rigidBody.velocity = new Vector2(HorizontalValue * DashSpeed, rigidBody.velocity.y);
                yield return null;
            }
        
            IsSliding = false;
            playerAnim.Slide(false);
            gameObject.layer = playerLayer;
        }

        bool CheckIsGrounded()
        {
            LayerMask groundLayer = 1 << GROUND_LAYER;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, GROUNDED_RAY_DISTANCE, groundLayer);
            //Debug.DrawRay(transform.position, Vector2.down, Color.red, 1.5f);

            if (hit.collider != null)
                if (!resetJump)
                {
                    playerAnim.Jump(false);
                    playerAnim.DoubleJump(false);
                    CanDashInAir = false;
                    return true;
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