using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 3f;
    [SerializeField] float jumpForce = 5;
    [SerializeField] bool isGrounded;

    Rigidbody2D rigidBody;
    PlayerAnimator playerAnim;
    SpriteRenderer sprite;

    bool resetJump;
    const int GROUND_LAYER = 8;
    const float GROUNDED_RAY_DISTANCE = 1.5f;

    void Start()
    {
        rigidBody = GetComponentInChildren<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        playerAnim = GetComponent<PlayerAnimator>();
    }
    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float horizontalValue = Input.GetAxisRaw("Horizontal");
        isGrounded = IsGrounded();

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
            playerAnim.Jump(true);
            StartCoroutine(ResetJumproutine());
        }

        rigidBody.velocity = new Vector2((horizontalValue * movementSpeed), rigidBody.velocity.y);

        HandleAnimationDirection(horizontalValue);
        playerAnim.Move(horizontalValue);
    }

    void HandleAnimationDirection(float horizontalValue)
    {
        //Vector3 newPos = swordArcSprite.transform.localPosition;

        if (horizontalValue < 0)
        {
            sprite.flipX = true;
            //swordArcSprite.flipY = true;

            //newPos.x = -1.01f;

           // swordArcSprite.transform.localPosition = newPos;
            //swordArcSprite.transform.rotation = Quaternion.Euler(-75, 48, -80);
        }
        else if (horizontalValue > 0)
        {
            sprite.flipX = false;
            //swordArcSprite.flipY = false;

            //newPos.x = 1.01f;

            //swordArcSprite.transform.localPosition = newPos;
            //swordArcSprite.transform.rotation = Quaternion.Euler(66, 48, -80);
        }
    }

    bool IsGrounded()
    {
        LayerMask groundLayer = 1 << GROUND_LAYER;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, GROUNDED_RAY_DISTANCE, groundLayer);
        //Debug.DrawRay(transform.position, Vector2.down, Color.red, 1.5f);

            print(hit.collider);
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
