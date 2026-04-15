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
    public static InteractManager Instance;

    [SerializeField] private InputActionReference interactAction;

    private IHighlightable currentObject; // Nino
    [SerializeField] private float highlightStrength = 0.2f; // Nino

    private LayerMask interactableMask;

    [Header("Camera Info")]
    private Transform cameraTransform;
    [SerializeField] private float rayDistance = 3f;
    private Vector3 cameraPosition;
    private Vector3 cameraDirection;

    private InteractObject interactObject;
    private bool currentlyHolding = false;

    [SerializeField] private GameObject grabHand;
    [SerializeField] private GameObject grabbingHand;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        interactableMask = LayerMask.GetMask("Interactable");

        if (grabHand == null)
        {
            grabHand = GameObject.Find("GrabHand");
        }
        if (grabbingHand == null)
        {
            grabbingHand = GameObject.Find("GrabbingHand");
        }
    }

    void Update()
    {
        if (cameraTransform == null)
        {
            cameraTransform = GameObject.Find("Camera").transform;
        }
        cameraPosition = cameraTransform.position;
        cameraDirection = cameraTransform.TransformDirection(Vector3.forward);
        RaycastHit hit;


        if (Physics.Raycast(cameraPosition, cameraDirection, out hit, rayDistance, interactableMask, QueryTriggerInteraction.Collide))
        {
            if (!currentlyHolding)
            {
                interactObject = hit.collider.gameObject.GetComponentInParent<InteractObject>();

                IHighlightable hitObject = hit.collider.GetComponentInParent<IHighlightable>(); // Nino

                if (hitObject != currentObject) // Nino
                {
                    ClearCurrent();

                    currentObject = hitObject;

                    if (currentObject != null)
                    {
                        currentObject.SetHighlight(highlightStrength); // Highlight på
                    }
                }

                if (interactObject.interactable)
                {
                    grabHand.SetActive(true);
                }
            }
            Debug.DrawRay(cameraPosition, cameraDirection * rayDistance, Color.yellow);
        }
        else
        {
            ClearCurrent(); // Nino

            if (interactObject != null && !currentlyHolding)
            {
                interactObject = null;

                grabHand.SetActive(false);
                grabbingHand.SetActive(false);
            }
            if (interactObject == null)
            {
                grabHand.SetActive(false);
                grabbingHand.SetActive(false);
            }

            Debug.DrawRay(cameraPosition, cameraDirection * rayDistance, Color.red);
        }

        // If currently holding
        if (interactObject != null)
        {
            if (interactAction.action.IsPressed() && interactObject.interactable)
            {
                ClearCurrent(); // Nino

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

    void ClearCurrent() // Nino
    {
        if (currentObject != null)
        {
            currentObject.SetHighlight(0f); // Highlight av
            currentObject = null;
        }
    }
}
