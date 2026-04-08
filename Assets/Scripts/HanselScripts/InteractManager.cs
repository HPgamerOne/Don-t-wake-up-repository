using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

/*
Denna skript har som huvudsyfte att sitta på spelaren och skicka ut raycast.
När den träffar objekt som är interagerbara, eg har skriptet "InteractObject"
så kan viss specifik logik utföras för det objektet genom InteractObject skriptet
så som att applicera krafter på objektet, switch:a en state som något annat skript lyssnar på
etc etc. 

TLDR: Skicka raycast, säg till objekt att de är upp plockade/interagerade med
*/

public class InteractManager : MonoBehaviour
{
    [SerializeField] private InputActionReference interactAction;
    

    LayerMask interactableMask;

    [Header("Camera Info")]
    [SerializeField] Transform cameraTransform;
    [SerializeField] float rayDistance = 3f;
    Vector3 cameraPosition;
    Vector3 cameraDirection;

    InteractObject interactObject;
    private bool currentlyHolding = false;

    GameObject grabHand;
    GameObject grabbingHand;

    void Start()
    {
        interactableMask = LayerMask.GetMask("Interactable");
        grabHand = GameObject.Find("GrabHand");
        grabbingHand = GameObject.Find("GrabbingHand");
    }

    void Update()
    {
        cameraPosition = cameraTransform.position;
        cameraDirection = cameraTransform.TransformDirection(Vector3.forward);
        RaycastHit hit;


        if (Physics.Raycast(cameraPosition, cameraDirection, out hit, rayDistance, interactableMask, QueryTriggerInteraction.Collide))
        {
            if (!currentlyHolding)
            {
                interactObject = hit.collider.gameObject.GetComponentInParent<InteractObject>();

                if (interactObject.interactable) {
                    grabHand.SetActive(true);
                }
            }
            Debug.DrawRay(cameraPosition, cameraDirection * rayDistance, Color.yellow);
        }
        else
        {
            if (interactObject == null)
            {
                grabHand.SetActive(false);
                grabbingHand.SetActive(false);
            }
            else if (interactObject.interacted == false)
            {
                grabHand.SetActive(false);
            }

            Debug.DrawRay(cameraPosition, cameraDirection * rayDistance, Color.red);
        }

        if (interactObject != null)
        {
            if (interactAction.action.IsPressed() && interactObject.interactable)
            {
                grabHand.SetActive(false);
                grabbingHand.SetActive(true);

                interactObject.interacted = true;
                if (interactObject.dynamic)
                {
                    interactObject.attractForceActive = true;
                    currentlyHolding = true;
                }
            }
            else
            {
                grabbingHand.SetActive(false);

                interactObject.interacted = false;
                interactObject.attractForceActive = false;
                currentlyHolding = false;
            }
        }
    }
}
