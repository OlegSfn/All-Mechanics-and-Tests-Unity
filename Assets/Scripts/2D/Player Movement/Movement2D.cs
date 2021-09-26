using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform CeilingChecker;
    [SerializeField] private Transform GroundedChecker;
    [SerializeField] [Range(0f, 10f)] private float groundCheckerRadius = 0.3f;
    [SerializeField] [Range(0f, 10f)] private float ceilingCheckerRadius = 0.3f;
    [SerializeField] private bool displayGroundAndCeilingRad = false;

    [Header("Running")]
    // Movement characteristics
    [SerializeField] [Range(0f, 10f)] private float maxSpeed = 5;
    [SerializeField] [Range(0f, 20f)] private float maxRunningSpeed = 10;
    [SerializeField] private AnimationCurve accelerationControl;
    [SerializeField] private AnimationCurve breakingControl;
    private float defaultMaxSpeed;
    private float speedX = 0;
    private float timeWhenButtonWasPressed, timeWhenButtonWasUnPressed;

    // Flipping parameters
    [SerializeField] private bool isFacingRight = true;
    private float directionX;

    [Header("Jumping")]
    // Jump characteristics
    [SerializeField] [Range(0f, 10f)] private float jumpForce = 7;
    [SerializeField] [Range(0f, 10f)] private float fallingMultiplier = 3.1f;
    [SerializeField] [Range(0f, 10f)] private float brakeJumpingMultiplier = 3f;

    // Jump handler
    [Space]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] [Range(0f, 1f)] private float rememberGroundedFor = 0.1f;
    private bool isGrounded = false;
    private float lastTimeGrounded;

    [Space]
    // Count of Jumps
    [SerializeField] [Range(0, 10)] private int startAdditionalJumps = 0;
    private int additionalJumps;

    [SerializeField] private bool canMoveInAir = true;
    [SerializeField] private bool canRunInAir = false;

    // Functions
    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        if (displayGroundAndCeilingRad)
        {
            Gizmos.DrawWireSphere(GroundedChecker.position, groundCheckerRadius);
            Gizmos.DrawWireSphere(CeilingChecker.position, ceilingCheckerRadius);
        }
    }


    private void Awake()
	{
        defaultMaxSpeed = maxSpeed;
        if (maxRunningSpeed < maxSpeed) maxRunningSpeed = maxSpeed;
    }

	private void Update()
    {
        if (Input.GetButton("Run") && (isGrounded || canRunInAir)) maxSpeed = maxRunningSpeed;
        else if (isGrounded) maxSpeed = defaultMaxSpeed;
        Move();
        Jump();
        HandleJump();
        CheckIfGrounded();
    }

    void Move() {
        if (!canMoveInAir && isGrounded == false) return;

        if (Input.GetButtonDown("Horizontal")) timeWhenButtonWasPressed = Time.time;
        else if (Input.GetButtonUp("Horizontal")) timeWhenButtonWasUnPressed = Time.time;

        if (Input.GetButton("Horizontal"))
        {
            directionX = Input.GetAxisRaw("Horizontal");
            if (directionX > 0 && !isFacingRight) Flip();
            if (directionX < 0 && isFacingRight) Flip();

            Accelerating();
        }
        else
        {
            Braking();
        }
    }

    void Accelerating()
	{
        if (speedX < maxSpeed) speedX += accelerationControl.Evaluate(Time.time - timeWhenButtonWasPressed);
        else speedX = maxSpeed;

        rb.velocity = new Vector2(speedX * directionX, rb.velocity.y);
    }

    void Braking()
	{
        if (speedX > 0) speedX -= breakingControl.Evaluate(Time.time - timeWhenButtonWasUnPressed);
        else speedX = 0;
        rb.velocity = new Vector2(speedX * directionX, rb.velocity.y);
    }

    void Flip()
	{
        isFacingRight = !isFacingRight;

        // Rotating the player
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
	}

    void Jump() {
        if (Input.GetButtonDown("Jump") && (isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor || additionalJumps > 0))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            additionalJumps--;
        }
    }

    void HandleJump() {
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity * fallingMultiplier * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
            rb.velocity += Vector2.up * Physics2D.gravity * brakeJumpingMultiplier * Time.deltaTime;
        }   
    }

    void CheckIfGrounded() {
        Collider2D colliders = Physics2D.OverlapCircle(GroundedChecker.position, groundCheckerRadius, groundLayer);

        if (colliders != null) {
            isGrounded = true;
            additionalJumps = startAdditionalJumps;
        } 
        else 
        {
            if (isGrounded) 
            {
                lastTimeGrounded = Time.time;
            }
            isGrounded = false;
        }
    }
}
