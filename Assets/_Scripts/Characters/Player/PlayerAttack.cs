using PHOCUS.UI;
using PHOCUS.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHOCUS.Character
{
    public class PlayerAttack : MonoBehaviour
    {
        public float AttackStaminaCost;
        public float AttackSpeed;
        public float ComboTime = 0.3f;
        public bool IsAttacking;
        public bool MouseOverInteractable;
        public bool IsDisabled;
        public int AttackIndex = 1;

        Player player;
        PlayerMovement playerMovement;
        PlayerAnimation playerAnim;
        InteractableRaycaster ray;
        DialoguePanel dialogue;

        bool combo;

        void Awake()
        {
            player = GetComponent<Player>();
            playerMovement = GetComponent<PlayerMovement>();
            playerAnim = GetComponent<PlayerAnimation>();
            ray = GameManager.Instance.InteractableRaycaster;
            dialogue = UIManager.Instance.DialoguePanel;
        }

        void Start()
        {
            ray.OnMouseOverInteractable += SetDisableAttackBool;

            foreach (Interactable interactable in dialogue.Interactables)
                interactable.OnMouseOverButton += SetDisableAttackBool;
        }


        void Update()
        {
            if (!MouseOverInteractable && !IsDisabled)
                HandleAttack();
        }

       void SetDisableAttackBool(bool rayBool)
        {
            if (MouseOverInteractable == rayBool)
                return;

            MouseOverInteractable = rayBool;
        }

        void HandleAttack()
        {
            if (!IsAttacking)
            {
                if (Input.GetMouseButtonDown(0) && playerMovement.IsGrounded && player.CheckStamina(AttackStaminaCost))
                {
                    IsAttacking = true;
                    StartCoroutine(AttackTarget());
                }
            }

            if (Input.GetMouseButtonDown(0) && !playerMovement.IsGrounded && !IsAttacking && player.CheckStamina(AttackStaminaCost))
            {
                playerAnim.AirAttack();
                player.Stamina -= AttackStaminaCost;
            }
        }

        IEnumerator AttackTarget()
        {
            Attack();
            yield return new WaitForSeconds(0.5f);

            if (IsAttacking)
                StartCoroutine(CheckComboAttack());
        }

        IEnumerator CheckComboAttack()
        {
            float progress = 0f;

            while (progress <= ComboTime)
            {
                progress += Time.deltaTime;

                if (Input.GetMouseButtonDown(0) && !combo && playerMovement.IsGrounded && player.CheckStamina(AttackStaminaCost))
                    combo = true;

                yield return null;
            }

            if (combo && IsAttacking)
            {
                if (AttackIndex == Mathf.Clamp(AttackIndex, 0, 3))
                    AttackIndex++;

                if (AttackIndex > 3)
                    AttackIndex = 1;

                StartCoroutine(AttackTarget());
                combo = false;
            }
            else
            {
                AttackIndex = 1;
                IsAttacking = false;
                combo = false;
            }
        }

        void Attack()
        {
            playerAnim.Attack(AttackIndex, playerMovement.HorizontalValue);
            player.Stamina -= AttackStaminaCost;
        }
    }
}