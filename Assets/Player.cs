using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private float movespeed;
    [SerializeField] private float jumpheight;

    [Header("Dash Info")]
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashspeed;
    [SerializeField] private float dashCoolDown;
    private float dashTime;
    private float dashCoolDownTimer;

    private float xInput;
    private int facingDir = 1;
    private bool facingRight = true;


    [Header("Ground Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        Movement();
        GroundCheck();
        TurnController();
        AnimatorControllers();

        dashTime -= Time.deltaTime;
        dashCoolDownTimer -= Time.deltaTime;
      
    }

    private void AnimatorControllers()
    {
        bool isMoving = rb.linearVelocity.x != 0;
        
        anim.SetBool("Running", isMoving);
        anim.SetBool("Grounded", isGrounded);
        anim.SetBool("Dashing", dashTime >0);
    }

    private void Jump()
    {
        if(isGrounded)
            rb.linearVelocity = new Vector2(0, jumpheight);
    }

    private void Movement()
    {
        if(dashTime > 0)
        {
            rb.linearVelocity = new Vector2 (xInput * dashspeed, 0);
        } 
        else
        {
            rb.linearVelocity = new Vector2 (xInput * movespeed, rb.linearVelocity.y);   
        }

    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }
    }


    private void Turn()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
    }

    private void TurnController()
    {
        if(rb.linearVelocity.x > 0 && !facingRight)
        {
            Turn();
        }
            else if(rb.linearVelocity.x < 0 && facingRight)
        {
            Turn();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }

    private void GroundCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
    }

    private void Dash()
    {
        if(dashCoolDownTimer < 0)
        {
            dashCoolDownTimer = dashCoolDown;
            dashTime = dashDuration;
        }
    }
}

