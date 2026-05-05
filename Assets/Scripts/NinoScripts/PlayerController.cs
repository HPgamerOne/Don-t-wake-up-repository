using UnityEngine;
using UnityEngine.InputSystem;

/*
Hantera spelare-rörelse med CharacterController. Hoppa, springa, tyngdkraften, flyta på vatten(ish).
*/

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private InputActionReference moveAction, jumpAction, sprintAction;

    [Header("Movement")]
    [SerializeField] private bool isSprinting = false;
    [SerializeField] private float baseMoveSpeed = 10;
    [SerializeField] private float baseMoveSpeedMultiplier = 1f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float jumpVelocity = 10f;
    private float moveSpeed;
    private float moveSpeedMultiplier;
    private float currentHorizontalSpeed;
    private Vector2 moveInput;
    private Vector3 velocity;
    private Vector3 horizontalVelocity;

    [Header("Gravity")]
    [SerializeField] private float gravity = -9.82f;
    [SerializeField] private float gravityMultiplier = 3;

    [Header("Water")]
    [SerializeField] private GameObject buoyancyWater;
    [SerializeField] private float waterDrag = 0.9f;
    [SerializeField] private float upforce = 35;
    private bool inBuoyancyWater;
    private float depth;

    [SerializeField] private CameraBob cameraBob;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterController.detectCollisions = false;
        moveSpeed = baseMoveSpeed;
        moveSpeedMultiplier = baseMoveSpeedMultiplier;
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
        horizontalVelocity = new Vector3(moveInput.x * moveSpeed * moveSpeedMultiplier, 0, moveInput.y * moveSpeed * moveSpeedMultiplier);
        horizontalVelocity = transform.rotation * horizontalVelocity;

        // Buoyancy

        if (buoyancyWater != null)
        {
            InWater();

            if (inBuoyancyWater)
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

    void OnTriggerEnter(Collider other)
    {
        Water water = other.GetComponent<Water>();

        if (water != null)
        {
            moveSpeedMultiplier = water.speedMultiplier;
            cameraBob.UpdateBob(moveSpeedMultiplier);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Water water = other.GetComponent<Water>();

        if (water != null)
        {
            moveSpeedMultiplier = baseMoveSpeedMultiplier;
            cameraBob.UpdateBob(moveSpeedMultiplier);
        }
    }

    private void OnSprint()
    {
        if (sprintAction.action.IsPressed())
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
        if (inBuoyancyWater == true)
        {
            if (jumpAction.action.IsPressed())
            {
                velocity.y = jumpVelocity;
            }
            velocity.y += gravity * gravityMultiplier * Time.deltaTime;
        }
        else
        {
            if (characterController.isGrounded)
            {
                velocity.y = -1;

                if (jumpAction.action.IsPressed())
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
        depth = transform.position.y - buoyancyWater.transform.position.y;

        if (depth < 0)
        {
            inBuoyancyWater = true;
        }
        else
        {
            inBuoyancyWater = false;
        }
    }

    public float CurrentHorizontalSpeed()
    {
        return currentHorizontalSpeed;
    }

    public bool IsSprinting()
    {
        return isSprinting;
    }
}