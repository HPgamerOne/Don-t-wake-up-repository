using UnityEngine;
using UnityEngine.Rendering;

public class Buoyancy : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject water;
    [SerializeField] float upforce = 15;
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
        if (transform.position.y < water.transform.position.y)
        {
            rb.linearDamping = 1;
            rb.angularDamping = 1f;

            rb.AddForce(new Vector3(0, upforce, 0), ForceMode.Acceleration);
            /*
            displacementMultiplier = Mathf.Clamp01(-transform.position.y / depthBeforeSubmerged) * displacementAmount;
            rb.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);
            */
        }
        else
        {
            rb.linearDamping = 0.1f;
            rb.angularDamping = 0.1f;
        }
    }
}
