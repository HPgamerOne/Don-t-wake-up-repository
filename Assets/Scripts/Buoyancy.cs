using UnityEngine;
using UnityEngine.Rendering;

/*
Hantera knuff uppåt på objekt med rigidbody i vatten.
*/

public class Buoyancy : MonoBehaviour
{
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] GameObject water;
    [SerializeField] float upforce = 15;
    [SerializeField] float airLinearDamping = 0f;
    [SerializeField] float airAngularDramping = 0.05f;
    [SerializeField] float waterLinearDamping = 3f;
    [SerializeField] float waterAngularDamping = 1f;
    [SerializeField] float difference;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        water = GameObject.Find("Water");
    }

    private void FixedUpdate()
    {
        if (water == null)
        {
            return;
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
