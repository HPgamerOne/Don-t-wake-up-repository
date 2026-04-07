using UnityEngine;


public class InteractObject : MonoBehaviour
{
    public bool interactable = true;
    public bool dynamic = true;

    Transform targetPositionTransform;
    Vector3 targetPosition;

    Transform currentPositionTransform;
    Vector3 currentPosition;

    [Header("Forces")]
    [SerializeField] float pickedUpForce = 100f;
    [SerializeField] float rotationForce = 25f;

    [Header("Damping")]
    [SerializeField] float linearDamping = 10;
    [SerializeField] float angularDamping = 5;

    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        targetPositionTransform = GameObject.Find("PickUpPosition").transform;
    }

    void Update()
    {
        
    }
}
