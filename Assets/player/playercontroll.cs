using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class playercontroller : MonoBehaviour
{
    [Header("Horizontal movement Srttings")]
    private Rigidbody2D rb;
    [SerializeField] private float walkSpeed = 1;
    private float xAsis, yAsis;
    [Header("virtecal movement srttings")]
    [SerializeField] private float jumpForce = 45;
    private float jumpBufferCounter = 0;
    [SerializeField] private float jumpBufferFrames;
    private float coyoteTimeCounter = 0;
    [SerializeField] private float coyoteTime = 0;
    private int airJumpCounter = 0;
    [SerializeField] private int maxAirJump;
    [Header("Ground Check Settings")]

    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask whatIsGround;
    [Header("Dash settings")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;


    

    public static playercontroller Instanece;
    private bool canDash = true;
    private bool dashed;
    private float gravity;


    
    bool jumping;
    private void Awake()
    {
        if (Instanece != null && Instanece != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instanece = this;
        }
       

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        rb = GetComponent<Rigidbody2D>();


        gravity = rb.gravityScale;

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        UpdateJumpVariables();
        flip();
        Move();
        jump();
        StratDash();

    }

    void GetInput()
    {
        xAsis = Input.GetAxisRaw("Horizontal");
        yAsis = Input.GetAxisRaw("Vertical");
    }
    void flip()
    {
        if (xAsis < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
            
        }
        else if (xAsis > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
            
        }
    }
    private void Move()
    {
        rb.linearVelocity = new Vector2(walkSpeed * xAsis, rb.linearVelocity.y);
    }
    void StratDash()
    {
        if (Input.GetButtonDown("Dash") && canDash && !dashed)
        {
            StartCoroutine(Dash());
            dashed = true;
        }
        if (Grounded() && canDash)
        {
            dashed = false;
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashSpeed, 0);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravity;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }


   
    public bool Grounded()
    {
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround) ||
            Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround) ||
            Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void jump()
    {
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(x: rb.linearVelocity.x, y: 0);
            jumping = false;
        }
        if (!jumping)
        {
            if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce);
                jumping = true;
            }
            else if (!Grounded() && airJumpCounter < maxAirJump && Input.GetButtonDown("Jump"))
            {
                jumping = true;
                airJumpCounter++;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce);
            }
        }
    }

    void UpdateJumpVariables()
    {
        if (Grounded())
        {
            jumping = false;
            coyoteTimeCounter = coyoteTime;
            airJumpCounter = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferFrames;
        }
        else
        {
            jumpBufferCounter = jumpBufferCounter - Time.deltaTime;
        }
    }
}

