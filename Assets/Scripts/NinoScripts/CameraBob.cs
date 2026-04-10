using UnityEngine;

/*
Hantera kamera position med spelare-rörelse.
*/

public class CameraBob : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float baseBobSpeed = 10f;
    [SerializeField] private float bobSpeed = 10f;
    [SerializeField] private float bobSpeedMultiplier = 1.5f;
    [SerializeField] private float bobAmount = 0.05f;
    private float newPosition;
    private float originalPosition;
    private float timer = 0;
    private Vector3 localPosition;

    void Start()
    {
        originalPosition = transform.localPosition.y;
        localPosition = transform.localPosition;
    }

    void Update()
    {
        HandleHeadBob();
    }

    void HandleHeadBob()
    {
        if (playerController.IsSprinting())
        {
            bobSpeed = baseBobSpeed * bobSpeedMultiplier;
        }
        else
        {
            bobSpeed = baseBobSpeed;
        }

        timer += Time.deltaTime * bobSpeed;
        newPosition = originalPosition + Mathf.Sin(timer) * bobAmount;

        if (playerController.CurrentHorizontalSpeed() > 0.1f)
        {
            localPosition = new Vector3(localPosition.x, newPosition, localPosition.z);
        }
        else
        {
            timer = 0;
            localPosition = new Vector3(localPosition.x, Mathf.Lerp(localPosition.y, originalPosition, Time.deltaTime * bobSpeed), localPosition.z);
        }

        transform.localPosition = localPosition;
    }
}
