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

    // Nino
    [SerializeField] private float objectHighlightStrength = 1f;
    private static readonly int highlightStrengthId = Shader.PropertyToID("_HighlightStrength");
    private MaterialPropertyBlock propertyBlock;
    private Renderer rend;
    // -------------------------------------

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

        propertyBlock = new MaterialPropertyBlock(); // Nino
    }

    void Update()
    {
        cameraPosition = cameraTransform.position;
        cameraDirection = cameraTransform.TransformDirection(Vector3.forward);
        RaycastHit hit;


        if (Physics.Raycast(cameraPosition, cameraDirection, out hit, rayDistance, interactableMask, QueryTriggerInteraction.Collide))
        {
            // Nino - Highlight på
            rend = hit.transform.root.GetComponent<Renderer>();

            if (rend != null)
            {
                rend.GetPropertyBlock(propertyBlock);
                propertyBlock.SetFloat(highlightStrengthId, objectHighlightStrength);
                rend.SetPropertyBlock(propertyBlock);
            }
            // -------------------------

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
            // Nino - Highlight av
            if (rend != null)
            {
                rend.GetPropertyBlock(propertyBlock);
                propertyBlock.SetFloat(highlightStrengthId, 0);
                rend.SetPropertyBlock(propertyBlock);
            }
            // -----------------------------------

            if (interactObject != null && !currentlyHolding)
            {
                interactObject = null;

                if (grabHand  != null) // Nino - Saknas referens
                {
                    grabHand.SetActive(false);
                    grabbingHand.SetActive(false);
                }
            }
            if (interactObject == null)
            {
                if (grabHand != null) // Nino - Saknas referens
                {
                    grabHand.SetActive(false);
                    grabbingHand.SetActive(false);
                }
            }

            Debug.DrawRay(cameraPosition, cameraDirection * rayDistance, Color.red);
        }

        // If currently holding
        if (interactObject != null)
        {
            if (interactAction.action.IsPressed() && interactObject.interactable)
            {
                // Nino - Highlight av
                if (rend != null)
                {
                    rend.GetPropertyBlock(propertyBlock);
                    propertyBlock.SetFloat(highlightStrengthId, 0);
                    rend.SetPropertyBlock(propertyBlock);
                }
                // -----------------------------------

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
