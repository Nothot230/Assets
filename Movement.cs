using UnityEngine;

public class Movement : MonoBehaviour
{
    private int amountJumpLeft;


    public int AmountJump = 2;
    public float playerSpeed = 10.00f;
    public float Force = 2;
    public float CheckGroundRadius;
    public float wallCheckDistance;
    public float WallSlideSpeed;
    public Transform GroundCheck;
    public Transform wallCheck;

    private float InputDirection;
    private bool isFacingRight = true;
    private bool isGround;
    private bool isRunning;
    private bool canJump;
    public bool isWall;
    public bool isWallSlide;
    private Animator anim;

    public LayerMask whatisGround;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountJumpLeft = AmountJump;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckDirection();
        AnimationControl();
        CheckCanJump();
        CheckIfwallSlide();
    }
    private void FixedUpdate(){
        DoMovement();
        CheckFunction();
    }

    private void CheckIfwallSlide(){
        if(isWall && !isGround && rb.linearVelocity.y < 0){
            isWallSlide = true;
        }else{
            isWallSlide = false;
        }
    }

    private void CheckDirection(){
        if(isFacingRight && InputDirection < 0){
            Flip();
        }else if(!isFacingRight && InputDirection > 0){
            Flip();
        }

        if(rb.linearVelocity.x > 1 || rb.linearVelocity.x < -1){
            isRunning = true;
        }else{
            isRunning = false;
        }
    }

    private void CheckInput(){
        InputDirection = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump")){
            Jump();
        }
    }

    private void DoMovement(){
        rb.linearVelocity = new Vector2(playerSpeed * InputDirection, rb.linearVelocity.y);

        if(isWallSlide == true){
            if(rb.linearVelocity.y < -WallSlideSpeed){
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -WallSlideSpeed);
            }
        }
    }

    private void Flip(){
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void Jump(){
        if(canJump){
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Force);
            amountJumpLeft--;
        }
        
    }

    private void AnimationControl(){
        anim.SetBool("IsRunning", isRunning);
        anim.SetBool("IsGrounded", isGround);
        anim.SetFloat("Yvelocity", rb.linearVelocity.y);
    }

    private void CheckFunction(){
        isGround = Physics2D.OverlapCircle(GroundCheck.position, CheckGroundRadius, whatisGround);

        isWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatisGround);
    }

    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(GroundCheck.position, CheckGroundRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }

    private void CheckCanJump(){
        if(isGround && rb.linearVelocity.y <= 0){
            amountJumpLeft = AmountJump;
        }
        if(amountJumpLeft <= 0){
            canJump = false;
        }else{
            canJump = true;
        }
    }

    
}
