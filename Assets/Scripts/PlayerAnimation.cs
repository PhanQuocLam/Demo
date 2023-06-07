using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;
    PlayerMovement1 movement1;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        movement1 = GetComponentInParent<PlayerMovement1>();
        rb = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speed", Mathf.Abs(movement1.xVelocity));
        anim.SetBool("isOnGround", movement1.isOnGround);
        anim.SetBool("isHanging", movement1.isHanging);
        anim.SetBool("isCrouching", movement1.isCrouch);
        //anim.SetFloat("verticalVelocity", rb.velocity.y);
    }

    public void StepAudio()
    {
        AudioManager.PlayFootstepAudio();
    }
    public void CrouchStepAudio()
    {
        AudioManager.PlayCrouchFootstepAudio();
    }
}
