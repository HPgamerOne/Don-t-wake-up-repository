using UnityEngine;

/*
Hantera kamera position med spelare-rörelse.
*/

public class CameraBob : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float baseBobSpeed = 10f;
    private float bobSpeed;
    [SerializeField] private float sprintBobSpeedMultiplier = 1.5f;
    [SerializeField] private float bobAmount = 0.05f;
    private float moveBobSpeedMultiplier = 1;
    private float newPosition;
    private float originalPosition;
    private float timer = 0;
    private Vector3 localPosition;

    void Start()
    {
        originalPosition = transform.localPosition.y;
        localPosition = transform.localPosition;

        bobSpeed = baseBobSpeed;
    }

    void Update()
    {
        HandleHeadBob();
    }

    void HandleHeadBob()
    {
        if (playerController.IsSprinting())
        {
            sprintBobSpeedMultiplier = 1.5f;
        }
        else
        {
            sprintBobSpeedMultiplier = 1f;
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
            localPosition = new Vector3(localPosition.x, Mathf.Lerp(localPosition.y, originalPosition, Time.deltaTime * bobSpeed * sprintBobSpeedMultiplier * moveBobSpeedMultiplier), localPosition.z);
        }

        transform.localPosition = localPosition;
    }


    public void UpdateBob(float moveSpeedMultiplier)
    {
        moveBobSpeedMultiplier = moveSpeedMultiplier;
    }
}
