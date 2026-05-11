using JetBrains.Annotations;
using UnityEngine;

public class CarKeyChecker : MonoBehaviour
{
    [SerializeField] string requiredKeyId;
    [SerializeField] GameObject door;
    [SerializeField] private GameObject keyPosition;



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided");

        if (other.CompareTag("Keys"))
        {
            Debug.Log("Matched Tag");

            Transform keyT = other.gameObject.GetComponent<Transform>();
            Transform carT = gameObject.GetComponent<Transform>();
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            Debug.Log("Collected Transforms & RB");

            string id = other.GetComponent<Key>().id;
            if(id == requiredKeyId)
            {
                
                Animator animator = other.gameObject.GetComponent<Animator>();
                Debug.Log("Animator");

                rb.isKinematic = true;
                rb.useGravity = false;

                other.gameObject.layer = 0;
                Debug.Log("Accepting Key");

                
                animator.Play("KeySuccess", 0, 0);
                other.transform.position = keyPosition.transform.position;
                other.transform.localRotation = keyPosition.transform.localRotation;

                DoorCondtions(id);
                Debug.Log("Activated door");

                //gameObject.SetActive(false);


            }
            else
            {
                //play non turning key animation
                Debug.Log("Ejecting");
                Vector3 ejectDir =   keyT.position - carT.position;
                rb.AddForce(ejectDir * 10f + Vector3.up*7f, ForceMode.Impulse);
            }
        }
    }
    private void DoorCondtions(string id)
    {
        DoorScript doorS = door.GetComponent<DoorScript>();
        switch (id)
        {
            case "RedKey":
                doorS.RedCar = true;
                break;
            case "BlueKey":
                doorS.BlueCar = true;
                break;
            case "GreenKey":
                doorS.GreenCar = true;
                break;

        }
    }
}
