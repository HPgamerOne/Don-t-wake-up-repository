using UnityEngine;

/*
Hantera knuff på objekt med rigidbody ifrån spelare genom kollision.
*/


public class ObstaclePush : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float forceMagnitude = 1;
    [SerializeField] private float hitNormal = 0.5f;
    private Vector3 forceDirection;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        if (rigidbody != null && playerController.CurrentHorizontalSpeed() > 0.01f && hit.normal.y < hitNormal)
        {
            forceDirection = hit.gameObject.transform.position - transform.position;

            forceDirection.y = 0;
            forceDirection.Normalize();

            rigidbody.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);
        }
    }
}

