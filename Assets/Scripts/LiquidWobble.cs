using UnityEngine;

public class LiquidWobble : MonoBehaviour
{
    [SerializeField] private Renderer rend;
    [SerializeField] private Vector3 lastPosition;
    [SerializeField] private Vector3 lastRotation;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 angularVelocity;
    [SerializeField] private float wobbleX;
    [SerializeField] private float wobbleZ;

    void Start()
    {
        lastPosition = transform.forward;
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        wobbleX = Mathf.Lerp(wobbleX, 0, Time.deltaTime * 5);
        wobbleZ = Mathf.Lerp(wobbleZ, 0, Time.deltaTime * 5);

        rend.material.SetFloat("_WobbleX", wobbleX);
        rend.material.SetFloat("_WobbleZ", wobbleZ);
        
        velocity = (lastPosition - transform.position) / Time.deltaTime;
        angularVelocity = transform.rotation.eulerAngles - lastRotation;

        wobbleX += Mathf.Clamp(velocity.x * 1, -0.01f, 0.01f);
        wobbleZ += Mathf.Clamp(velocity.z * 1, -0.01f, 0.01f);
        // wobbleX += Mathf.Clamp((velocity.x + (angularVelocity.z * 0.2f)) * 0.03f, -0.5f, 0.5f);
        // wobbleZ += Mathf.Clamp((velocity.z + (angularVelocity.x * 0.2f)) * 0.03f, -0.5f, 0.5f);

        lastPosition = transform.forward;
        lastRotation = transform.rotation.eulerAngles;
    }
}
