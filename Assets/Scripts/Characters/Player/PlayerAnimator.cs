using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHOCUS.Character
{
    public class PlayerAnimator : MonoBehaviour
    {
        Animator anim;

        void Start()
        {
            anim = GetComponentInChildren<Animator>();
        }

        public void Move(float horizontalValue)
        {
            anim.SetFloat("Move", Mathf.Abs(horizontalValue));
        }

        public void Jump(bool isJumping)
        {
            anim.SetBool("Jump", isJumping);
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

        public void Attack(int index)
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
    }
}