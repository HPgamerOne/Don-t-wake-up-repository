using JetBrains.Annotations;
using UnityEngine;

public class CarKeyChecker : MonoBehaviour
{
    [SerializeField] string requiredKeyId;
    [SerializeField] GameObject door;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Keys"))
        {
            Transform keyT = other.GetComponent<Transform>();
            Rigidbody rb = other.GetComponent<Rigidbody>();
            Transform carT = GetComponent<Transform>();
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;

            keyT.position = transform.position + Vector3.up * 3f;
            keyT.rotation = transform.rotation;

            string id = other.GetComponent<Key>().id;
            if(id == requiredKeyId)
            {
                //Key turning animation
                //car driving in circles animation
                DoorCondtions(id);
            }
            else
            {
                //play non turning key animation
                rb.isKinematic = false;
                rb.useGravity = true;
                Vector3 ejectDir = (other.transform.position - transform.position).normalized;
                rb.AddForce(ejectDir * 5f + Vector3.up * 2f, ForceMode.Impulse);
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
