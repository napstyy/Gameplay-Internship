using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float rotationSpeed = 10f;
    public float jumpForce = 5f;
    public float gravityScale = 20f;
    public float groundDrag = 0.2f;

    private CharacterController _controller;
    private float verticalVelocity;
    private bool isGrounded;
    private bool isJumping;
    private int jumpsRemaining;
    private int maxJumps = 2;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        isGrounded = _controller.isGrounded;
        
        if (isGrounded)
        {
            jumpsRemaining = maxJumps;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Transform cam = Camera.main.transform;
        Vector3 forward = Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(cam.right, Vector3.up).normalized;
        Vector3 direction = (forward * v + right * h).normalized;

        if (direction.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            _controller.Move(direction * moveSpeed * Time.deltaTime);
        }

        HandleJump();
        HandleGravity();

        _controller.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0)
        {
            verticalVelocity = jumpForce;
            isJumping = true;
            jumpsRemaining--;
        }
        
        if (isGrounded && isJumping && verticalVelocity <= 0)
        {
            isJumping = false;
        }
    }
    
    public bool IsJumping()
    {
        return isJumping && verticalVelocity > 0;
    }

    private void HandleGravity()
    {
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = 0f;
        }
        else
        {
            verticalVelocity -= gravityScale * Time.deltaTime;
        }
    }
}