using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private InputActionReference moveAction, jumpAction, sprintAction;

    [Header("Movement")]
    [SerializeField] private float baseMoveSpeed = 10;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float jumpVelocity = 10f;
    [SerializeField] Vector2 moveInput;
    [SerializeField] Vector3 velocity;
    [SerializeField] Vector3 horizontalVelocity;

    [Header("Gravity")]
    [SerializeField] private float gravity = -9.82f;
    [SerializeField] private float gravityMultiplier = 3f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        moveSpeed = baseMoveSpeed;

        // Sprint
        if (sprintAction.action.ReadValue<float>() != 0)
        {
            moveSpeed = baseMoveSpeed * sprintMultiplier;
        }
        else
        {
            moveSpeed = baseMoveSpeed;
        }

        // Movement Input
        moveInput = moveAction.action.ReadValue<Vector2>();
        horizontalVelocity = new Vector3(moveInput.x * moveSpeed, 0, moveInput.y * moveSpeed);
        horizontalVelocity = transform.rotation * horizontalVelocity;

        // Gravity
        if (characterController.isGrounded)
        {
            velocity.y = -1;

            // Jump
            if (jumpAction.action.WasPressedThisFrame())
            {
                velocity.y = jumpVelocity;
            }
        }
        else
        {
            velocity.y += gravity * gravityMultiplier * Time.deltaTime;
        }

        velocity.x = horizontalVelocity.x;
        velocity.z = horizontalVelocity.z;

        characterController.Move(velocity * Time.deltaTime);
    }
}