using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHOCUS.Character
{
    public class PlayerAnimator : MonoBehaviour
    {
        Animator anim;
        PlayerController player;

        void Start()
        {
            anim = GetComponentInChildren<Animator>();
            player = GetComponent<PlayerController>();
        }

        void Update()
        {
            HandleLayers();    
        }

        private void HandleLayers()
        {
            if (player.isAttacking)
            {
                ActivateLayer("Attack Layer");
            }
            else
            {
                ActivateLayer("Base Layer");
            }
        }

        public void Move(float horizontalValue)
        {
            if (horizontalValue == 0)
            {
                anim.SetBool("Move", false);
                return;
            }
            anim.SetBool("Move", true);
            anim.SetFloat("Horizontal", horizontalValue);
        }

        public void StopMoving()
        {
            anim.SetBool("Move", false);
        }

        public void Jump(bool isJumping)
        {
            anim.SetBool("Jump", isJumping);
        }

        public void DoubleJump(bool isDoubleJump)
        {
            anim.SetBool("DoubleJump", isDoubleJump);
        }

        public void AirAttack()
        {
            anim.SetTrigger("AirAttack");
        }

        public void Slide(bool slide)
        {
            anim.SetBool("Slide", slide);
        }

        public void Hit()
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack01") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack02") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
                return;

            anim.SetTrigger("Hit");
        }

        public float Die()
        {
            anim.SetTrigger("Death");
            return anim.GetCurrentAnimatorClipInfo(0).Length;
        }

        public void Attack(int index, float horizontal)
        {
            switch (index)
            {
                case 1:
                    anim.SetTrigger("Attack01");
                    break;
                case 2:
                    anim.SetTrigger("Attack02");
                    break;
                case 3:
                    anim.SetTrigger("Attack03");
                    break;
            }
        }

        void ActivateLayer(string layerName)
        {
            for (int i = 0; i < anim.layerCount; i++)
            {
                anim.SetLayerWeight(i, 0);
            }

            anim.SetLayerWeight(anim.GetLayerIndex(layerName), 1);
        }
    }
}