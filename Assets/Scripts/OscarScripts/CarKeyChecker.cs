using JetBrains.Annotations;
using UnityEngine;

public class CarKeyChecker : MonoBehaviour
{
    public string requiredKeyId;
    [SerializeField] GameObject door;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Keys"))
        {
            Transform keyT = other.gameObject.GetComponent<Transform>();
            Transform carT = gameObject.GetComponent<Transform>();
            keyT.position = new Vector3(carT.position.x, carT.position.y + 3, carT.position.z);


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
                Rigidbody rb = other.GetComponent<Rigidbody>();
                rb.AddForce(new Vector3(Random.Range(-2,2), Random.Range(-2, 2), Random.Range(-2, 2)), ForceMode.Impulse);
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
