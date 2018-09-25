﻿using System;
using System.Collections;
using UnityEngine;

namespace PHOCUS.Character
{
    public class PlayerController : MonoBehaviour
    {
        public float MovementSpeed = 3f;
        public float JumpForce = 5f;
        public float AttackSpeed = 1.5f;
        public float horizontalValue;
        public bool isAttacking;

        Rigidbody2D rigidBody;
        PlayerAnimator playerAnim;
        SpriteRenderer sprite;

        bool isGrounded;
        bool resetJump;
        int attackIndex = 1;
        float lastHitTime;

        const float GROUNDED_RAY_DISTANCE = 1.5f;
        const int GROUND_LAYER = 8;

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
            horizontalValue = Input.GetAxisRaw("Horizontal");
            isGrounded = IsGrounded();

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, JumpForce);
                playerAnim.Jump(true);
                StartCoroutine(ResetJumproutine());
                sprite.flipX = true;
            }

            rigidBody.velocity = new Vector2((horizontalValue * MovementSpeed), rigidBody.velocity.y);

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