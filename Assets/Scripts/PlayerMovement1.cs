using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    private Rigidbody2D rb ;
    private BoxCollider2D coll;
    // Start is called before the first frame update
    public float speed = 8f;
    public float croundSpeedDivisor = 3f;
    //nhảy
    public float jumpForce = 6.3f;
    public float jumpHoldForce = 1.9f;
    public float jumpHoldDuration = 0.1f;
    public float crouchJumpBoost = 2.5f;
    public float jumpTime;
    public float hangingJumpForce = 15f;

    // trạng thái
    public bool isCrouch;
    public bool isJump;
    public bool isOnGround;
    public bool isHeadBlocked;
    public bool isHanging;
    
    //
    public float footOffset = 0.4f;
    public float headClearance = 0.5f;
    public float groundDistance = 0.2f;

    float playerHeight;
    public float eyeHeight =1.5f;
    public float GrabDistance = 0.4f;
    public float reachOffset = 0.7f;


    public LayerMask groundLayer;
    public float xVelocity;
    //
    bool jumpPressed;
    bool jumpHeld;
    bool crouchHeld;
    // Kích thước 
    Vector2 colliderStandupSize;
    Vector2 colliderStandupOffset;
    Vector2 colliderCrouchSize;
    Vector2 colliderCrouchOffset;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        playerHeight = coll.size.y;

        colliderStandupSize = coll.size;
        colliderStandupOffset = coll.offset;
        colliderCrouchSize = new Vector2(coll.size.x, coll.size.y / 2f);
        colliderCrouchOffset = new Vector2(coll.offset.x, coll.offset.y / 2f);
    }

    // Update is called once per frame
    void Update()
    {
        jumpPressed = Input.GetButton("Jump");
        jumpHeld = Input.GetButton("Jump");
        crouchHeld = Input.GetButton("Crouch");

    }
    private void FixedUpdate()
    { 
        PhysícsCheck();
        GroundMovement();
        MidAirMovement();
    }
    void PhysícsCheck()
    {
        RaycastHit2D leftcheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightcheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        if (leftcheck || rightcheck)
            isOnGround = true;
        else isOnGround = false;

        RaycastHit2D headcheck = Raycast(new Vector2(0f, coll.size.y), Vector2.up, headClearance, groundLayer);
        if (headcheck)
            isHeadBlocked = true;
        else isHeadBlocked = false;

        float dicrection = transform.localScale.x;
        Vector2 grabDir = new Vector2(dicrection, 0f);

        RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset* dicrection,playerHeight),grabDir,GrabDistance,groundLayer);
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * dicrection, eyeHeight), grabDir, GrabDistance, groundLayer);
        RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * dicrection, playerHeight), Vector2.down, GrabDistance, groundLayer);
        if (!isOnGround && rb.velocity.y<0f && ledgeCheck &&wallCheck && !blockedCheck)
        {
            Vector3 pos = transform.position;
            pos.x += (wallCheck.distance - 0.05f) * dicrection;
            pos.y -= ledgeCheck.distance;
            transform.position = pos;

            rb.bodyType = RigidbodyType2D.Static;
            isHanging = true;
        }
    }
    void GroundMovement()
    {
        if (isHanging)
            return;
        if (crouchHeld && !isCrouch && isOnGround)
            Crouch();
        else if (!crouchHeld && isCrouch && !isHeadBlocked)
            Standup();
        else if (!isOnGround && isCrouch)
            Standup();
        if (isCrouch)
            xVelocity /= croundSpeedDivisor;
        xVelocity = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);
        FlipDirction();
    }
    void MidAirMovement()
    {
        if (isHanging)
        {
            if (jumpPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.velocity = new Vector2(rb.velocity.x, hangingJumpForce);
                isHanging = false;
            }
        }

        if(jumpPressed && isOnGround && !isJump && !isHeadBlocked)
        {
            if(isCrouch )
            {
                Standup();
                rb.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
            }
            isOnGround = false;
            isJump = true;
            jumpTime = Time.time + jumpHoldDuration;
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            AudioManager.PlayJumpAudio();
        }   
        else if (isJump)
        {
            if (jumpHeld)
                rb.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
            if (Time.time > jumpTime)
                isJump = false;
        }
    }
    void FlipDirction()
    {
        if (xVelocity < 0)
            transform.localScale = new Vector3(-1, 1,1);
        if (xVelocity > 0)
            transform.localScale = new Vector3(1, 1,1);
    }
    void Crouch()
    {
        isCrouch = true;
        coll.size = colliderCrouchSize;
        coll.offset = colliderCrouchOffset;
    }    
    void Standup()
    {
        isCrouch = false;
        coll.size = colliderStandupSize;
        coll.offset = colliderStandupOffset;
    }
    RaycastHit2D Raycast(Vector2 offset,Vector2 rayDiraction,float lenght,LayerMask layer)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, lenght, layer);
        Color color = hit ? Color.red : Color.green;
       // Debug.DrawRay(pos + offset, rayDiraction * lenght,color);
        return hit;
    }
}
