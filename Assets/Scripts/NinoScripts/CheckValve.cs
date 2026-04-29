using UnityEngine;

public class CheckValve : MonoBehaviour
{
    [SerializeField] private GameObject valvePosition;
    [SerializeField] private bool valveInPlace = false;

    private void OnTriggerEnter(Collider other)
    {
        Valve valve = other.gameObject.GetComponent<Valve>();

        if (valve != null)
        {
            Animator animator = other.gameObject.GetComponent<Animator>();
            Rigidbody rigidbody = other.gameObject.GetComponent<Rigidbody>();

            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;

            other.gameObject.layer = 0;

            other.transform.position = valvePosition.transform.position;
            other.transform.localRotation = valvePosition.transform.localRotation;

            animator.Play("ValveTurn", 0, 0);

            valveInPlace = true;

            gameObject.SetActive(false);
        }
    }

    public bool IsValveInPlace()
    {
        return valveInPlace;
    }
}