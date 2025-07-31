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
    [SerializeField] GameObject dashEffect;
    Animator anim;

    [HideInInspector] public playerstatelist pstate;

    public static playercontroller Instanece;
    private bool canDash = true;
    private bool dashed;
    private float gravity;

    [Header("wall jump")]
    [SerializeField] private Transform WallCheck;
    [SerializeField] private float WallSlidingSpeed;

    [Header("attacking settings")]
    bool attack = false;
    [SerializeField] private float timeBetweenAttack;
    private float timeSinceAttack;
    [SerializeField] Transform SideAttackTransfrom, UpAttackTransfrom, DownAttackTransfrom;
    [SerializeField] Vector2 SideAttackArea, UpAttackArea, DownAttackArea;
    [SerializeField] LayerMask attackableLayer;
    [SerializeField] float damage;
    bool restoreTime;
    float restoreTimeSpeed;

    [Header("Recoil")]
    [SerializeField] int recoilXSteps = 5;
    [SerializeField] int recoilYsteps = 5;
    [SerializeField] float recoilXSpeed = 100;
    [SerializeField] float recoilYSpeed = 100;
    int stepsXRecoiled, stepsYRecoiled;
    [Header("health settings")]
    public float health;
    public float maxHealth;
    [SerializeField] float HitFlashSpeed;
    private SpriteRenderer sr;
    public delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate OnHealthChangedCallback;
    [SerializeField] GameObject Bloodspurt;

    [Header("player settings")]
    [SerializeField] private bool WillThisDash;
    [SerializeField] private bool WillThiswalljump;
    [SerializeField] protected bool WillThisBreakObjects;
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
        pstate = GetComponent<playerstatelist>();

        rb = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        gravity = rb.gravityScale;

        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        UpdateJumpVariables();
        if (pstate.Dashing) return;
        flip();
        Move();
        jump();
        StratDash();
        Attack();
        RestoreTimeScale();
        FlashWhileInvincible();
        WallSlide();
    }
    private void FixedUpdate()
    {
        if (pstate.Dashing) return;
        Recoil();
    }
    void GetInput()
    {
        xAsis = Input.GetAxisRaw("Horizontal");
        yAsis = Input.GetAxisRaw("Vertical");
        attack = Input.GetButtonDown("Attack");
    }
    void flip()
    {
        if (xAsis < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
            pstate.lookingRight = false;
        }
        else if (xAsis > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
            pstate.lookingRight = true;
        }
    }
    private void Move()
    {
        rb.linearVelocity = new Vector2(walkSpeed * xAsis, rb.linearVelocity.y);
        anim.SetBool("Walking", rb.linearVelocity.x != 0 && Grounded());
        pstate.walking = (Grounded() && xAsis != 0 && !pstate.Dashing && !pstate.recoilingX && !pstate.recoilingY);
    }
    void StratDash()
    {
        if (Walled()) StopCoroutine(Dash());
        if (Input.GetButtonDown("Dash") && canDash && !dashed && WillThisDash)
        {
            StartCoroutine(Dash());
            dashed = true;
        }
        if (Grounded())
        {
            dashed = false;
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        pstate.Dashing = true;
        anim.SetTrigger("Dashing");
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * dashSpeed, 0);
        if (Grounded()) Instantiate(dashEffect, transform);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravity;
        pstate.Dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    void Attack()
    {
        timeSinceAttack += Time.deltaTime;
        if (attack && timeSinceAttack >= timeBetweenAttack)
        {
            timeSinceAttack = 0;
            anim.SetTrigger("Attacking");
            if (yAsis == 0 || yAsis < 0 && Grounded())
            {
                Hit(SideAttackTransfrom, SideAttackArea, ref pstate.recoilingX, recoilXSpeed);
            }
            else if (yAsis > 0)
            {
                Hit(UpAttackTransfrom, UpAttackArea, ref pstate.recoilingY, recoilYSpeed);
            }
            else if (yAsis < 0 && !Grounded())
            {
                Hit(DownAttackTransfrom, DownAttackArea, ref pstate.recoilingY, recoilYSpeed);
            }
        }
    }
    void Hit(Transform _attackTransform, Vector2 _attackArea, ref bool _recoilDir, float _recoilStrenght)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);
        List<Enemy> hitenemies = new List<Enemy>();
        if (objectsToHit.Length > 0)
        {
            _recoilDir = true;
        }
        for (int i = 0; i < objectsToHit.Length; i++)
        {
            Enemy e = objectsToHit[i].GetComponent<Enemy>();
            if (e && !hitenemies.Contains(e))
            {
                e.EnemyHit(damage, (transform.position - objectsToHit[i].transform.position).normalized, _recoilStrenght);
                hitenemies.Add(e);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(SideAttackTransfrom.position, SideAttackArea);
        Gizmos.DrawWireCube(UpAttackTransfrom.position, UpAttackArea);
        Gizmos.DrawWireCube(DownAttackTransfrom.position, DownAttackArea);
        Gizmos.DrawWireCube(WallCheck.position, WallCheck.position);
    }
    void Recoil()
    {
        if (pstate.recoilingX)
        {
            if (pstate.lookingRight)
            {
                rb.linearVelocity = new Vector2(-recoilXSpeed, 0);
            }
            else
            {
                rb.linearVelocity = new Vector2(recoilXSpeed, 0);
            }
        }

        if (pstate.recoilingY)
        {

            if (yAsis < 0)
            {

                rb.linearVelocity = new Vector2(rb.linearVelocity.x, recoilYSpeed);
            }
            else
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -recoilYSpeed);
            }
            airJumpCounter = 0;
        }
        else
        {
            rb.gravityScale = gravity;
        }

        if (pstate.recoilingX && stepsXRecoiled < recoilXSteps)
        {
            stepsXRecoiled++;
        }
        else
        {
            StopRecoilX();
        }
        if (!pstate.recoilingY && stepsYRecoiled < recoilYsteps)
        {
            stepsYRecoiled++;
        }
        else
        {
            StopRecoilY();
        }
        if (Grounded())
        {
            StopRecoilY();
        }
    }

    void StopRecoilX()
    {
        stepsXRecoiled = 0;
        pstate.recoilingX = false;
    }
    void StopRecoilY()
    {
        stepsYRecoiled = 0;
        pstate.recoilingY = false;
    }

    public void TakeDamge(float _damge)
    {
        health -= Mathf.RoundToInt(_damge);
        StartCoroutine(StopTakingDamage());
    }

    IEnumerator StopTakingDamage()
    {
        pstate.invincible = true;
        GameObject _BloodspurtParticles = Instantiate(Bloodspurt, UpAttackTransfrom.position, Quaternion.identity);
        Destroy(_BloodspurtParticles, 1.5f);
        anim.SetTrigger("takeDamage");
        yield return new WaitForSeconds(1f);
        pstate.invincible = false;
    }

    void FlashWhileInvincible()
    {
        sr.material.color = pstate.invincible ? Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time * HitFlashSpeed, 1.0f)) : Color.white;
    }
    void RestoreTimeScale()
    {
        if (restoreTime)
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale += Time.unscaledDeltaTime * restoreTimeSpeed;
            }
            else
            {
                Time.timeScale = 1;
                restoreTime = false;
            }
        }
    }




    public bool Grounded()
    {
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround) ||
            Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround) ||
            Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround))
        {
            return true;
            pstate.OnGround = true;
        }
        else
        {
            return false;
            pstate.OnGround = false;
        }
    }

    private void jump()
    {
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(x: rb.linearVelocity.x, y: 0);
            pstate.jumping = false;
        }
        if (!pstate.jumping)
        {
            if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce);
                pstate.jumping = true;
            }
            else if (!Grounded() && airJumpCounter < maxAirJump && Input.GetButtonDown("Jump"))
            {
                pstate.jumping = true;
                airJumpCounter++;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce);
            }
        }

        anim.SetBool("Jumping", !Grounded());
    }

    void UpdateJumpVariables()
    {
        if (Grounded())
        {
            pstate.jumping = false;
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

    private bool Walled()
    {
        if (
            Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.right, groundCheckX, whatIsGround) ||
            Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.left, groundCheckX, whatIsGround))
        { return true; }
        else
        { return false; }

    }

    private void WallSlide()
    {
        if (Walled() && !Grounded() && WillThiswalljump)
        {
            pstate.WallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -WallSlidingSpeed, float.MaxValue));
            if (Input.GetButtonDown("Jump") ) 
            { 
                rb.linearVelocity = new Vector2(transform.position.x * (transform.localScale.x * -2), rb.linearVelocity.y);
                airJumpCounter--;
            }
        }
        else
        {
            pstate.WallSliding = false;
        }
    }

    
}
