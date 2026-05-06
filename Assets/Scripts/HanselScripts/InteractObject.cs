using UnityEditor;
using UnityEngine;

// Nino - har kommenterat bort för kompatibilitet mellan InteractObject skript och Bouyancy skript
public class InteractObject : MonoBehaviour
{
    private Buoyancy buoyancy; // Nino

    public bool interactable = true;
    public bool dynamic = true;
    public bool attractForceActive = false;
    public bool interacted = false;
    public bool hovering = false;

    Transform targetPositionTransform;
    Vector3 targetPosition;

    Transform currentPositionTransform;
    Vector3 currentPosition;

    [Header("Forces")]
    [SerializeField] float pickedUpForce = 100f;
    [SerializeField] float pickedUpRotationForce = 25f;

    [Header("Damping")]
    [SerializeField] float pickedUpLinearDamping = 10;
    [SerializeField] float pickedUpAngularDamping = 5;
    [SerializeField] float defaultLinearDamping = 0;
    [SerializeField] float defaultAngularDamping = 0.05f;

    Rigidbody rigidBody;
    LayerMask playerMask;

    void Start()
    {
        buoyancy = GetComponent<Buoyancy>();

        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null)
        {
            rigidBody = gameObject.AddComponent<Rigidbody>();
            rigidBody.isKinematic = true;
        }

        playerMask = LayerMask.GetMask("Player");
        targetPositionTransform = GameObject.Find("PickUpPosition").transform;

        currentPositionTransform = transform.Find("HoldPosition");
        if (currentPositionTransform == null )
        {
            currentPositionTransform = transform;
        }
    }

    void FixedUpdate()
    {
        currentPosition = currentPositionTransform.position;
        targetPosition = targetPositionTransform.position;
        Vector3 targetVector = targetPosition - currentPosition;

        if (attractForceActive)
        {
            rigidBody.excludeLayers = playerMask;

            rigidBody.linearDamping = pickedUpLinearDamping;
            rigidBody.angularDamping = pickedUpAngularDamping;

            rigidBody.AddForce(targetVector * pickedUpForce, ForceMode.Acceleration);

            Vector3 currentForward = rigidBody.transform.forward;
            Vector3 targetForward = targetPositionTransform.forward;
            Vector3 forwardRotationAxis = Vector3.Cross(currentForward, targetForward);

            Vector3 currentRight = rigidBody.transform.right;
            Vector3 targetRight = targetPositionTransform.right;
            Vector3 rightRotationAxis = Vector3.Cross(currentRight, targetRight);

            rigidBody.AddTorque(forwardRotationAxis * pickedUpRotationForce, ForceMode.Acceleration);
            rigidBody.AddTorque(rightRotationAxis * pickedUpRotationForce, ForceMode.Acceleration);
        }
        else
        {
            rigidBody.excludeLayers = 0;

            if (buoyancy == null) // Nino
            {
                rigidBody.linearDamping = defaultLinearDamping;
                rigidBody.angularDamping = defaultAngularDamping;
            }
        }
    }
}
