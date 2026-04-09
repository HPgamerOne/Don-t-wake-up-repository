using UnityEngine;
using UnityEngine.Rendering;

/*
Hantera knuff uppåt på objekt med rigidbody i vatten.
*/

public class Buoyancy : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private GameObject water;
    [SerializeField] private InteractObject interactObject;
    [SerializeField] private float upforce = 15;
    [SerializeField] private float airLinearDamping = 0f;
    [SerializeField] private float airAngularDramping = 0.05f;
    [SerializeField] private float waterLinearDamping = 3f;
    [SerializeField] private float waterAngularDamping = 1f;
    private float difference;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        interactObject = GetComponent<InteractObject>();
    }

    private void FixedUpdate()
    {
        if (water == null)
        {
            return;
        }
        
        if (interactObject != null)
        {
            if (interactObject.interacted)
            {
                return;
            }
        }

        difference = transform.position.y - water.transform.position.y;

        if (difference < 0)
        {
            rigidBody.linearDamping = waterLinearDamping;
            rigidBody.angularDamping = waterAngularDamping;

            rigidBody.AddForceAtPosition(Vector3.up * upforce * Mathf.Abs(difference), transform.position, ForceMode.Force);
        }
        else
        {
            rigidBody.linearDamping = airLinearDamping;
            rigidBody.angularDamping = airAngularDramping;
        }
    }
}
