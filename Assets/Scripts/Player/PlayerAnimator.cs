using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
