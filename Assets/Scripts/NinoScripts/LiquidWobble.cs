using UnityEngine;

public class LiquidWobble : MonoBehaviour
{
    [SerializeField] private Renderer rend;

    private static readonly int WobbleXId = Shader.PropertyToID("_WobbleX");
    private static readonly int WobbleZId = Shader.PropertyToID("_WobbleZ");

    private MaterialPropertyBlock propertyBlock;

    private Vector3 lastPosition;
    // private Vector3 lastRotation;
    private Vector3 velocity;
    // private Vector3 angularVelocity;
    private float wobbleX;
    private float wobbleZ;

    void Start()
    {
        lastPosition = transform.forward;
        rend = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    void Update()
    {
        wobbleX = Mathf.Lerp(wobbleX, 0, Time.deltaTime * 5);
        wobbleZ = Mathf.Lerp(wobbleZ, 0, Time.deltaTime * 5);

        rend.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat(WobbleXId, wobbleX);
        propertyBlock.SetFloat(WobbleZId, wobbleZ);
        rend.SetPropertyBlock(propertyBlock);

        velocity = (lastPosition - transform.position) / Time.deltaTime;
        // angularVelocity = transform.rotation.eulerAngles - lastRotation;

        wobbleX += Mathf.Clamp(velocity.x * 1, -0.01f, 0.01f);
        wobbleZ += Mathf.Clamp(velocity.z * 1, -0.01f, 0.01f);
        // wobbleX += Mathf.Clamp((velocity.x + (angularVelocity.z * 0.2f)) * 0.03f, -0.5f, 0.5f);
        // wobbleZ += Mathf.Clamp((velocity.z + (angularVelocity.x * 0.2f)) * 0.03f, -0.5f, 0.5f);

        lastPosition = transform.forward;
        // lastRotation = transform.rotation.eulerAngles;
    }
}
