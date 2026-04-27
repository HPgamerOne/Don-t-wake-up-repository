using UnityEngine;
using UnityEngine.UIElements;

public class BlockChecker : MonoBehaviour
{
    [SerializeField] string requiredID;
    [SerializeField] GameObject toyBox;

    public bool isFilled;

    private void OnTriggerEnter(Collider other)
    {
        WoodBlock block = other.GetComponent<WoodBlock>();
        if (block != null)
        {
            if(block.id == requiredID)
            {
                Debug.LogWarning("Collision works");
                isFilled = true;
                toyBox.GetComponent<ToyBoxScript>().UpdateCheckers();

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        WoodBlock block = other.GetComponent<WoodBlock>();
        if (block != null)
        {
            if (block.id == requiredID)
            {
                isFilled = false;
                toyBox.GetComponent<ToyBoxScript>().UpdateCheckers();
            }
        }
    }
 
   
}
