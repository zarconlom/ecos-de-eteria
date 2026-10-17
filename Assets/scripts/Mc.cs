
using UnityEngine;

public class Mc : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Vector3 _startPosition =
        new Vector3(2.5f, 2.5f, 1f);
    [SerializeField] private float _jumpForce = 10f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    private float _horizontalInput;
    private Rigidbody2D _rb;
    private Animator animator;

    public bool isMoving = false;
    public bool isJumping = false;
    public bool Triggered = false;

    private bool isGrounded;

    void Start()
    {
        transform.localScale = _startPosition;
        _rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Comprueba si los pies están tocando el suelo.
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        isJumping = !isGrounded;

        flip();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump();
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");

        Vector2 movement = new Vector2(
            _horizontalInput * _speed,
            _rb.linearVelocity.y
        );

        _rb.linearVelocity = movement;

        isMoving = _horizontalInput != 0;

        if (animator != null)
        {
            animator.SetBool("isWalking", isMoving);
        }
    }

    private void flip()
    {
        if (_horizontalInput > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(
                _startPosition.x,
                _startPosition.y,
                _startPosition.z
            );
        }
        else if (_horizontalInput < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(
                -_startPosition.x,
                _startPosition.y,
                _startPosition.z
            );
        }
    }

    private void jump()
    {
        // Solo permite saltar cuando toca el suelo.
        if (isGrounded)
        {
            _rb.linearVelocity = new Vector2(
                _rb.linearVelocity.x,
                0f
            );

            _rb.AddForce(
                Vector2.up * _jumpForce,
                ForceMode2D.Impulse
            );

            isGrounded = false;
            isJumping = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(
                groundCheck.position,
                groundCheckRadius
            );
        }
    }
}