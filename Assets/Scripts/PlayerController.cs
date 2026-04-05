using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private InputActionReference moveAction, jumpAction, sprintAction;

    [Header("Movement")]
    [SerializeField] private bool isSprinting = false;
    [SerializeField] private float baseMoveSpeed = 10;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float jumpVelocity = 10f;
    [SerializeField] private float currentHorizontalSpeed;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 horizontalVelocity;

    [Header("Gravity")]
    [SerializeField] private float gravity = -9.82f;
    [SerializeField] private float gravityMultiplier = 3;

    [Header("Water")]
    [SerializeField] GameObject water;
    [SerializeField] private float depth;
    [SerializeField] private float waterDrag = 0.9f;
    [SerializeField] private float upforce = 35;
    [SerializeField] private bool inWater = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        water = GameObject.Find("Water");

        moveSpeed = baseMoveSpeed;
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Sprint
        OnSprint();

        // Movement Input
        moveInput = moveAction.action.ReadValue<Vector2>();
        horizontalVelocity = new Vector3(moveInput.x * moveSpeed, 0, moveInput.y * moveSpeed);
        horizontalVelocity = transform.rotation * horizontalVelocity;

        // Buoyancy

        if (water != null)
        {
            InWater();

            if (inWater == true)
            {
                Buoyancy();
            }
        }

        // Gravity, jump
        Gravity();

        velocity.x = horizontalVelocity.x;
        velocity.z = horizontalVelocity.z;

        currentHorizontalSpeed = horizontalVelocity.magnitude;

        characterController.Move(velocity * Time.deltaTime);
    }

    private void OnSprint()
    {
        if (sprintAction.action.ReadValue<float>() != 0)
        {
            moveSpeed = baseMoveSpeed * sprintMultiplier;
            isSprinting = true;
        }
        else
        {
            moveSpeed = baseMoveSpeed;
            isSprinting = false;
        }
    }

    private void Gravity()
    {
        if (inWater == true)
        {
            if (jumpAction.action.ReadValue<float>() != 0)
            {
                velocity.y = jumpVelocity;
            }
            velocity.y += gravity * gravityMultiplier * Time.deltaTime * waterDrag;
        }
        else
        {
            if (characterController.isGrounded)
            {
                velocity.y = -1;

                if (jumpAction.action.ReadValue<float>() != 0)
                {
                    velocity.y = jumpVelocity;
                }
            }
            else
            {
                velocity.y += gravity * gravityMultiplier * Time.deltaTime;
            }
        }
    }

    private void Buoyancy()
    {
        velocity.y += upforce * Time.deltaTime;
        velocity.y *=  waterDrag;
    }

    private void InWater()
    {
        depth = transform.position.y - water.transform.position.y;

        if (depth < 0)
        {
            inWater = true;
        }
        else
        {
            inWater = false;
        }

        /*
        if (difference < waterHeight - inWaterOffset)
        {
            inWater = true;
        }
        else if (difference < waterHeight - outWaterOffset)
        {
            inWater = false;
        }
        */
    }

    public float CurrentHorizontalSpeed()
    {
        return currentHorizontalSpeed;
    }

    public bool IsSprinting()
    {
        return isSprinting;
    }

    public bool IsSwimming()
    {
        return isSprinting;
    }
}