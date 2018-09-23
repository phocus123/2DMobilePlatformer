using System.Collections;
using UnityEngine;

namespace PHOCUS.Character
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float movementSpeed = 3f;
        [SerializeField] float jumpForce = 5f;
        [SerializeField] float AttackSpeed = 1.5f;

        Rigidbody2D rigidBody;
        PlayerAnimator playerAnim;
        SpriteRenderer sprite;

        bool isGrounded;
        bool resetJump;
        public bool isAttacking;
        const int GROUND_LAYER = 8;
        const float GROUNDED_RAY_DISTANCE = 1.5f;
        int attackIndex = 1;
        float lastHitTime;

        void Start()
        {
            rigidBody = GetComponentInChildren<Rigidbody2D>();
            sprite = GetComponentInChildren<SpriteRenderer>();
            playerAnim = GetComponent<PlayerAnimator>();
        }

        void Update()
        {
            HandleAttack();
            HandleMovement();
        }

        void HandleMovement()
        {
            float horizontalValue = Input.GetAxisRaw("Horizontal");
            isGrounded = IsGrounded();

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
                playerAnim.Jump(true);
                StartCoroutine(ResetJumproutine());
            }

            rigidBody.velocity = new Vector2((horizontalValue * movementSpeed), rigidBody.velocity.y);

            HandleAnimationDirection(horizontalValue);
            playerAnim.Move(horizontalValue);
        }

        void HandleAttack()
        {
            if (Input.GetMouseButtonDown(0) && isGrounded && !isAttacking)
            {
                isAttacking = true;
                StartCoroutine(AttackTarget());
            }
        }

        IEnumerator AttackTarget()
        {
            playerAnim.Attack(attackIndex);
            yield return new WaitForSeconds(AttackSpeed);
            isAttacking = false;
            attackIndex++;

            if (attackIndex > 3)
                attackIndex = 1;
        }

        void HandleAnimationDirection(float horizontalValue)
        {
            if (horizontalValue < 0)
            {
                sprite.flipX = true;
            }
            else if (horizontalValue > 0)
            {
                sprite.flipX = false;
            }
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