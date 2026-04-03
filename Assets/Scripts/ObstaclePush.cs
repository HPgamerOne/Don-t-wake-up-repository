using UnityEngine;

public class ObstaclePush : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float forceMagnitude = 1;
    [SerializeField] private Vector3 forceDirection;
    [SerializeField] private float hitNormal = 0.5f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        if (rigidbody != null && playerController.CurrentHorizontalSpeed() != 0 && hit.normal.y < hitNormal)
        {
            forceDirection = hit.gameObject.transform.position - transform.position;

            forceDirection.y = 0;
            forceDirection.Normalize();

            rigidbody.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);
        }
    }
}

