using UnityEngine;
using UnityEngine.Rendering;

public class Buoyancy : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject water;
    [SerializeField] float upforce = 15;
    [SerializeField] float airLinearDrag = 0f;
    [SerializeField] float airAngularDrag = 0.05f;
    [SerializeField] float waterLinearDrag = 3f;
    [SerializeField] float waterAngularDrag = 1f;
    [SerializeField] float difference;
    /*
    [SerializeField] private float depthBeforeSubmerged = 1f;
    [SerializeField] private float displacementAmount = 1f;
    [SerializeField] private float displacementMultiplier;
    */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        water = GameObject.Find("Water");
    }

    // Update is called once per frame
    void Update()
    {
        
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
            rb.linearDamping = waterLinearDrag;
            rb.angularDamping = waterAngularDrag;

            rb.AddForceAtPosition(Vector3.up * upforce * Mathf.Abs(difference), transform.position, ForceMode.Force);
            /*
            displacementMultiplier = Mathf.Clamp01(-transform.position.y / depthBeforeSubmerged) * displacementAmount;
            rb.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);
            */
        }
        else
        {
            rb.linearDamping = airLinearDrag;
            rb.angularDamping = airAngularDrag;
        }
    }
}
