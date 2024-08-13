using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DinoController : MonoBehaviour
{
 /*   public float maxJumpForce = 20f;
    public float minJumpForce = 5f;
    public float JumpEspeed = 5f;
    public float chargeTime = 3f;
    public float glideSpeed = 1f;
    public float groundCheckDistance = 1.0f;

    private Rigidbody rb;
    private bool isJumping;
    private bool isGliding;
    private bool Predictjump;
    public float jumpCharge;
    public Image JumpImage;
    public static DinoController i;
    DinoManager Dm;
    void Start()
    {
        Dm = GetComponent<DinoManager>();
        rb = GetComponent<Rigidbody>();
        jumpCharge = minJumpForce;
        i = this;
    }

    void Update()
    {
        CheckGroundStatus();
        DetectInput();
    }
    public void updateUi()
    {
        JumpImage.fillAmount = jumpCharge / maxJumpForce;
    }
    void CheckGroundStatus()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                isGrounded = true;
                isJumping = false;
                isGliding = false;
            }
        }
        else
        {
            isGrounded = false;
        }
    }

    void DetectInput()
    {
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            if (!isJumping)
            {
                jumpCharge += (maxJumpForce - minJumpForce + JumpEspeed) / chargeTime * Time.deltaTime;
                jumpCharge = Mathf.Clamp(jumpCharge, minJumpForce, maxJumpForce);
                updateUi();
                if (!Dm.dinos[0].isGrounded)
                {
                    Predictjump = true;
                }
            }
        }
        else
        {
            if ( Predictjump)
            {
                Jump();
            }
        }
        if (Input.GetMouseButtonUp(0) || (Input.touchCount == 0 && isJumping))
        {
            Jump();
        }
    }

    void Jump()
    {
        for (int i = 0; i < Dm.dinos.Count; i++)
        {
            if (Dm.dinos[i].isGrounded)
            {
                Dm.dinos[i].Jump(jumpCharge);
                Dm.dinos[i].isGrounded = false;
                isJumping = true;
                jumpCharge = minJumpForce;
                Predictjump = false;
            }
            else if (isJumping)
            {
                StartGlide();
            }
            updateUi();
        }
    }

    void StartGlide()
    {
        if (!isGliding)
        {
            rb.velocity = new Vector3(rb.velocity.x, -glideSpeed, rb.velocity.z);
            isGliding = true;
        }
    }*/
}