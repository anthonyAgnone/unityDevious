using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

  //these are references to the various objects that we have "attached" to our player  
  private Rigidbody2D rb;
  public Animator anim;

  public Transform groundCheck;

  // variables that are pretty self explanatary. just wanted this comment in here to section out the declarations. 
  private float movementInputDirection;
  private bool isFacingRight = true;

  private bool isWalking;
  private bool canJump;

  private int amountOfJumpsLeft;



  // these are tested and feels pretty good, however they are exposed so if you select the man
  // during play mode you can play with them yourself too. I don't claim to be an expert at platformers. 
  // Wanted to feel like slightly magic boosted jumping. 
  public float movementSpeed = 6.5f;
  public float jumpForce = 9.5f;
  public float fallMultiplier = 1.8f;
  public float lowJumpMultiplier = 3f;

  public int amountOfJumps = 1;

  // experimental variables that are working so far
  public bool isFalling, isJumping, releasedDirection, isGrounded;
  public float groundCheckRadius;
  public LayerMask whatIsGround;

  public Vector2 size;

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    amountOfJumpsLeft = amountOfJumps;
  }

  // Update is called once per frame
  void Update()
  {
    CheckInput();
    CheckMovementDirection();
    UpdateAnimations();
    CheckIfCanJump();
  }

  private void FixedUpdate()
  {
    ApplyMovement();
    ApplyJumpPhysics();
    ApplyForwardMomentum();
    CheckSurroundings();
  }

  private void CheckSurroundings()
  {

    // isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    isGrounded = Physics2D.OverlapBox(groundCheck.position, size, 180.0f, whatIsGround);
  }

  private void CheckIfCanJump()
  {
    if (isGrounded && rb.velocity.y <= 0)
    {
      amountOfJumpsLeft = amountOfJumps;
    }

    if (amountOfJumpsLeft <= 0)
    {
      canJump = false;
    }
    else
    {
      canJump = true;
    }


  }
  private void CheckMovementDirection()
  {
    if (isFacingRight && movementInputDirection < 0)
    {
      Flip();
    }
    else if (!isFacingRight && movementInputDirection > 0)
    {
      Flip();
    }

    if (Mathf.Abs(rb.velocity.x) > 0.1)
    {
      isWalking = true;
    }
    else
    {
      isWalking = false;
    }
  }

  private void UpdateAnimations()
  {
    anim.SetBool("isWalking", isWalking);
    anim.SetBool("isGrounded", isGrounded);
    anim.SetFloat("yVelocity", rb.velocity.y);
  }
  private void CheckInput()
  {
    movementInputDirection = Input.GetAxis("Horizontal");
    if (Input.GetButtonDown("Jump") && !isFalling)
    {
      Jump();
    }

    if (Input.GetButtonUp("Horizontal") && isFalling)
    {
      releasedDirection = true;
    }
    else if (Input.GetButtonDown("Horizontal"))
    {
      releasedDirection = false;
    }
  }

  private void Jump()
  {
    if (canJump)
    {
      rb.velocity = Vector2.up * jumpForce;
      amountOfJumpsLeft--;
      isJumping = true;
    }
  }

  // these jump physics are like mario, little less like real life for now. in mario you jump normal, fall faster. got this equation off of a forum
  private void ApplyJumpPhysics()
  {
    if (rb.velocity.y < 0)
    {
      rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
    }
    else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
    {
      rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    }
  }

  // this script keeps some forward momentum going if the user is in the air and lets go of the directoin key
  private void ApplyForwardMomentum()
  {
    if (isFalling && releasedDirection)
    {
      if (isFacingRight)
      {
        rb.velocity = new Vector2(movementSpeed * .6f, rb.velocity.y);
      }
      else
      {
        rb.velocity = new Vector2(movementSpeed * .6f * -1, rb.velocity.y);
      }

    }
  }

  private void ApplyMovement()
  {
    rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
  }
  private void Flip()
  {
    isFacingRight = !isFacingRight;
    transform.Rotate(0.0f, 180.0f, 0.0f);
  }

  // these scripts are overwriting the oncollision scripts that are built in
  // the col variable is actually the thing you are coming into contact with.
  void OnCollisionEnter2D(Collision2D col)
  {
    //set in the layers. ground is layer 8
    // if (col.gameObject.layer == 8)
    // {
    //   isFalling = false;
    //   isJumping = false;
    //   releasedDirection = false;
    // }

  }

  void OnCollisionExit2D(Collision2D col)
  {
    // if (col.gameObject.layer == 8)
    // {
    //   isFalling = true;
    // }
  }

  private void OnDrawGizmos()
  {
    // Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    Gizmos.DrawWireCube(groundCheck.position, size);
  }
}




//starting the animations