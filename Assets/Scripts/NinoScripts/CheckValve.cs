using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class CheckValve : MonoBehaviour
{
    [SerializeField] private GameObject valvePosition;
    [SerializeField] private bool valveInPlace = false;
    //private BoxCollider boxCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (valveInPlace == true && interactObject.interacted)
        {


            StartCoroutine(LoadNextScene());
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        Valve valve = other.gameObject.GetComponent<Valve>();

        if (valve != null)
        {
            Animator animator = other.gameObject.GetComponent<Animator>();
            Rigidbody rigidbody = other.gameObject.GetComponent<Rigidbody>();

            /*
            InteractObject interactObject = other.gameObject.GetComponent<InteractObject>();
            ObjectHighlight objectHighlight = other.gameObject.GetComponent<ObjectHighlight>();
            
            interactObject.enabled = false;
            objectHighlight.enabled = false;
            */

            //boxCollider.enabled = false;

            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;

            other.gameObject.layer = 0;
            /*
            interactObject.dynamic = false;
            interactObject.interactable = false;
            */

            other.transform.position = valvePosition.transform.position;
            other.transform.localRotation = valvePosition.transform.localRotation;

            //valveInPlace = true;
     

            animator.Play("ValveTurn", 0, 0);

            valveInPlace = true;
            // StartCoroutine(DoThing());
            gameObject.SetActive(false);
        }
    }

    public bool IsValveInPlace()
    {
        return valveInPlace;
    }

    /*
    private IEnumerator DoThing()
    {
        interactObject.interactable = false;
        animator.Play("ValveTurn", 0, 0);
        yield return new WaitForSeconds(2f);

        yield return null;
    }
    */
}