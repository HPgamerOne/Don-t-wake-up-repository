using UnityEngine;

public class LiquidWobble : MonoBehaviour
{
    [SerializeField] private Renderer rend;

    [Header("Wobble")]
    [SerializeField] private float wobbleStrength = 0.4f; // Styrkan på wobble, alltså mer velocitet
    [SerializeField] private float wobbleFrequency = 8f; // Hur snabbt vattnet rör sig(typ viskositet)
    [SerializeField] private float wobbleDamping = 1.2f; // Dämpning för wobble, sänker velocitet
    [SerializeField] private float maxWobble = 0.4f; // Hur stor/vinkel wobblen är

    private static readonly int WobbleXId = Shader.PropertyToID("_WobbleX");
    private static readonly int WobbleZId = Shader.PropertyToID("_WobbleZ");

    private MaterialPropertyBlock propertyBlock;

    private float wobbleX;
    private float wobbleZ;
    private float wobbleVelocityX;
    private float wobbleVelocityZ;
    private float dt;

    private Vector3 lastPosition;
    private Quaternion lastRotation;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }
    private void Update()
    {
        rend.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat(WobbleXId, wobbleX);
        propertyBlock.SetFloat(WobbleZId, wobbleZ);
        rend.SetPropertyBlock(propertyBlock);
    }

    private void FixedUpdate()
    {
        dt = Time.fixedDeltaTime;

        if (dt < 0.01f) return;

        Vector3 linearVelocity = (transform.position - lastPosition) / dt;
        Vector3 linearLocalVelocity = transform.InverseTransformDirection(linearVelocity);

        Quaternion deltaRot = transform.rotation * Quaternion.Inverse(lastRotation);
        deltaRot.ToAngleAxis(out float angle, out Vector3 axis);

        if (angle > 180f)
        {
            angle -= 360f;
        }

        Vector3 angularVelocityDegrees = axis * (angle / Time.deltaTime);
        Vector3 angularLocalVelocityDegrees = transform.InverseTransformDirection(angularVelocityDegrees);

        lastPosition = transform.position;
        lastRotation = transform.rotation;

        // Linjär rörelse i X-axeln roterar Z-axeln, rörelse i Z-axeln roterar X-axeln
        // Vinkel rotation i X-axeln roterar Z-planet, rotation i Z-axeln roterar X-planet
        float impulseX = -linearLocalVelocity.z * wobbleStrength + angularLocalVelocityDegrees.x * wobbleStrength * 0.01f;
        float impulseZ = linearLocalVelocity.x * wobbleStrength + angularLocalVelocityDegrees.z * wobbleStrength * 0.01f; 

        // Wobble går mot 0, ska typ "splasha runt/studsa runt" effekt, dämpningen sänker velocitet
        float springX = -(wobbleFrequency * wobbleFrequency) * wobbleX - wobbleDamping * wobbleVelocityX;
        float springZ = -(wobbleFrequency * wobbleFrequency) * wobbleZ - wobbleDamping * wobbleVelocityZ;

        // Lägger ihop impuls-wobble och spring-wobble, lägger in det i WobbleX för mängd rotation i X-axeln och WobbleZ för mängd rotation i Z-axeln
        wobbleVelocityX += (springX + impulseX) * dt;
        wobbleVelocityZ += (springZ + impulseZ) * dt;
        wobbleX += wobbleVelocityX * dt;
        wobbleZ += wobbleVelocityZ * dt;

        wobbleX = Mathf.Clamp(wobbleX, -maxWobble, maxWobble);
        wobbleZ = Mathf.Clamp(wobbleZ, -maxWobble, maxWobble);

        // Skicka värden till shadermaterialet
        rend.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat(WobbleXId, wobbleX);
        propertyBlock.SetFloat(WobbleZId, wobbleZ);
        rend.SetPropertyBlock(propertyBlock);
    }
}

