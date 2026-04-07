using UnityEngine;

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
    LayerMask interactableMask;

    [Header("Camera Info")]
    [SerializeField] Transform cameraTransform;
    [SerializeField] float rayDistance = 3f;
    Vector3 cameraPosition;
    Vector3 cameraDirection;

    void Start()
    {
        interactableMask = LayerMask.GetMask("Interactable");
    }

    void Update()
    {
        cameraPosition = cameraTransform.position;
        cameraDirection = cameraTransform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(cameraPosition, cameraDirection, out hit, rayDistance, interactableMask, QueryTriggerInteraction.Collide))
        {
            InteractObject interactObject = hit.collider.gameObject.GetComponentInParent<InteractObject>();


            Debug.DrawRay(cameraPosition, cameraDirection * rayDistance, Color.yellow);
        }
        else
        {
            Debug.DrawRay(cameraPosition, cameraDirection * rayDistance, Color.red);
        }
    }
}
