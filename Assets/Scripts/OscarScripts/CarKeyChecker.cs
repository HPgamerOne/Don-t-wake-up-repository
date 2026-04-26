using JetBrains.Annotations;
using UnityEngine;

public class CarKeyChecker : MonoBehaviour
{
    public string requiredKeyId;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Keys"))
        {
            //Flytta position och  kör rotations animation

            string id = other.GetComponent<Key>().id;
            if(requiredKeyId == id)
            {
                //driving in circles animation
                //boolean = true
            }
            else
            {
                //play non working key animation
            }
        }
    }
}
