using UnityEngine;

/*
Hantera kamera position med spelare-rörelse.
*/

public class CameraBob : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float newPosition;
    [SerializeField] private float originalPosition;
    [SerializeField] private float timer = 0;
    [SerializeField] private float baseBobSpeed = 10f;
    [SerializeField] private float bobSpeed = 10f;
    [SerializeField] private float bobSpeedMultiplier = 1.5f;
    [SerializeField] private float bobAmount = 0.05f;

    void Start()
    {
        originalPosition = transform.localPosition.y;
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

        if (playerController.CurrentHorizontalSpeed() > 0.1f) // Bob when moving
        {
            transform.localPosition = new Vector3(transform.localPosition.x, newPosition, transform.localPosition.z);
        }
        else // Return cam to original position
        {
            timer = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, originalPosition, Time.deltaTime * bobSpeed), transform.localPosition.z);
        }
    }
}
