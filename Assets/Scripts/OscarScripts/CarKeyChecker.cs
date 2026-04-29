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
            Transform keyT = other.gameObject.GetComponent<Transform>();
            Transform carT = gameObject.GetComponent<Transform>();
            Rigidbody rb = other.GetComponent<Rigidbody>();
            
            string id = other.GetComponent<Key>().id;
            if(id == requiredKeyId)
            {
                //Key turning animation
                //car driving in circles animation
                transform.Find("Key").GetComponent<MeshRenderer>().enabled = true;
                
                DoorCondtions(id);
                Destroy(other);
            }
            else
            {
                //play non turning key animation
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
